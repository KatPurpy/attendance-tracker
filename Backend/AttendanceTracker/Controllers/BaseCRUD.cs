using AttendanceTracker.Models.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;

namespace AttendanceTracker.Controllers
{
	public class DatabaseAccessResult<T>
	{
		public enum StatusEnum
        {
			NotFound,
			NotAuthorized,
			Success
		}

        public StatusEnum Status;
        public T? Value;

		public DatabaseAccessResult(DatabaseAccessResult<T>.StatusEnum status, T? value)
		{
			Status = status;
			Value = value;
		}

		public static DatabaseAccessResult<T> NotFound()
        {
            return new(StatusEnum.NotFound, default);
        }
		
        public static DatabaseAccessResult<T> NotAuthorized()
		{
			return new(StatusEnum.NotAuthorized, default);
		}

        public static DatabaseAccessResult<T> Success(T  value)
        {
            return new(StatusEnum.Success, value);
        }
	}

    [Route("api/[controller]")]
    public abstract class BaseCRUD<DbType,ApiType> : Controller 
        where DbType : class, IIntDbKey, new()
        where ApiType : class, IIntDbKey, IAPIModelFor<ApiType,DbType>
    {
        protected readonly AppDatabaseContext DbCtx;
        protected readonly UserManager<IdentityUser> UserManager;

        public BaseCRUD(AppDatabaseContext dbCtx, UserManager<IdentityUser> usermanager)
        {
            DbCtx = dbCtx;
            UserManager = usermanager;
        }

        [HttpGet]
        [Route(nameof(Read))]
        public async Task<ActionResult<ApiType>> Read(int id)
        {
			var entry = (await GetEntry(DbCtx, id)).Value;
			if (entry == null) return NotFound();
			return Activator.CreateInstance<ApiType>().ConvertToAPI(DbCtx, entry);
        }

        [HttpGet]
        [Route(nameof(ReadAll))]
        public async Task<ActionResult<List<ApiType>>> ReadAll()
        {
            var list = DbCtx.Set<DbType>().ToList();
            return list.Select(dbEnt => Activator.CreateInstance<ApiType>().ConvertToAPI(DbCtx, dbEnt)).ToList();
        }

        [HttpPost]
        [Route(nameof(Delete))]
        public async Task<ActionResult> Delete(int id)
        {
			var entry = (await GetEntry(DbCtx, id)).Value;
			if (entry == null) return NotFound();

			DbCtx.Remove(entry);
            await DbCtx.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route(nameof(Update))]
        public async Task<ActionResult<ApiType>> Update(int id, [FromBody] ApiType value)
        {
            var entry = (await GetEntry(DbCtx, id)).Value;
            if (entry == null) return NotFound();
            
            value.Id = entry.Id;
            DbCtx.Entry(entry).CurrentValues.SetValues(value);
            
            await DbCtx.SaveChangesAsync();

            return value;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<ActionResult<ApiType>> Create([FromBody] ApiType value)
        {
            using (var transaction = await DbCtx.Database.BeginTransactionAsync())
            {
                try 
                {
                    var entry = await CreateEntry(DbCtx, value);
                    value.Id = default;
                    DbCtx.Entry(entry).CurrentValues.SetValues(value);
                    await AssignUserId(entry);
                    
                    // check if this entry is valid
                    if(!await UserHasAccess(entry))
                    {
                        transaction.Rollback();
                        return BadRequest();
                    }

                    transaction.Commit();
                    await DbCtx.SaveChangesAsync();
                    value.Id = entry.Id;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return value;
        }

        public static async Task<DbType> CreateEntry(AppDatabaseContext dbCtx, ApiType value)
        {
            var entry = new DbType();
            await dbCtx.AddAsync(entry);
            return entry;
        }

        public async Task<DatabaseAccessResult<DbType>> GetEntry(AppDatabaseContext dbCtx, int id)
        {
            var entry = await dbCtx.FindAsync<DbType>(id);
            if(entry == null)
            {
                return DatabaseAccessResult<DbType>.NotFound();
            }

            if(await UserHasAccess(entry))
            {
                return DatabaseAccessResult<DbType>.Success(entry);
            }
            else
            {
                return DatabaseAccessResult<DbType>.NotAuthorized();
            }
        }

		public async Task AssignUserId(DbType entry)
		{
			var user = await UserManager.GetUserAsync(User);
			await AssignUserId(user, entry);
		}

		public async Task<bool> UserHasAccess(DbType entry)
        {
            var user = await UserManager.GetUserAsync(User);
            return UserHasAccess(user, DbCtx, entry);
        }
        public abstract bool UserHasAccess(IdentityUser user, AppDatabaseContext context, DbType entry);
        public virtual async Task AssignUserId(IdentityUser user, DbType entry) { }
    }
}
