using AttendanceTracker.Models.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;

namespace AttendanceTracker.Controllers
{
    [Route("api/[controller]")]
    public abstract class BaseObjectListController<DbType,ApiType> : Controller 
        where DbType : class, IIntDbKey, new()
        where ApiType : class, IIntDbKey, IAPIModelFor<ApiType,DbType>
    {
        protected readonly AppDatabaseContext DbCtx;
        protected readonly UserManager<IdentityUser> UserManager;

        public BaseObjectListController(AppDatabaseContext dbCtx, UserManager<IdentityUser> usermanager)
        {
            DbCtx = dbCtx;
            UserManager = usermanager;
        }

        [HttpGet]
        [Route(nameof(Read))]
        public async Task<ActionResult<ApiType>> Read(int id)
        {
            var entry = await GetEntry(DbCtx,id);
            if (entry == null) return StatusCode((int)HttpStatusCode.NoContent);
			if (!await UserHasAccess(entry)) return Forbid();
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
            var entry = await GetEntry(DbCtx, id);
            if (entry == null) return StatusCode((int)HttpStatusCode.NoContent);
			if (!await UserHasAccess(entry)) return Forbid();
			DbCtx.Remove(entry);
            await DbCtx.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route(nameof(Update))]
        public async Task<ActionResult<ApiType>> Update(int id, [FromBody] ApiType value)
        {
            var entry = await GetEntry(DbCtx, id);
            if (entry == null) return StatusCode((int)HttpStatusCode.NoContent);
            if (!await UserHasAccess(entry)) return Forbid();
            value.Id = entry.Id;
            DbCtx.Entry(entry).CurrentValues.SetValues(value);
            
            await DbCtx.SaveChangesAsync();

            return value;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<ActionResult<ApiType>> Create([FromBody] ApiType value)
        {
            var entry = await CreateEntry(DbCtx, value);
            value.Id = default;
            DbCtx.Entry(entry).CurrentValues.SetValues(value);
            await AssignUserId(entry);
            await DbCtx.SaveChangesAsync();
            value.Id = entry.Id;
            return value;
        }

        public static async Task<DbType> CreateEntry(AppDatabaseContext dbCtx, ApiType value)
        {
            var entry = new DbType();
            await dbCtx.AddAsync(entry);
            return entry;
        }

        public static async Task<DbType?> GetEntry(AppDatabaseContext dbCtx, int id)
        {
            return await dbCtx.FindAsync<DbType>(id);
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
