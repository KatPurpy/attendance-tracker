using AttendanceTracker.Models.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;

namespace AttendanceTracker.Controllers
{
    public class ApiResult
    {
		public enum ErrorCodeEnum
		{
			Unknown,
			NotFound,
			InvalidInput
		}

		public static ApiResult<T> Success<T>(T result)
        {
            return ApiResult<T>.Success(result);
        }

		public static ApiResult Error(ErrorCodeEnum error, string message)
		{
			return ApiResult<object>.Error(error, message);
		}

        public static ApiResult NotFound()
        {
            return Error(ErrorCodeEnum.NotFound, default);
        }
	}

    public class ApiResult<T> : ApiResult
    {


        public ErrorCodeEnum? ErrorCode { get; set; }
        public string? HumanReadableErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Result { get; set; }

		public ApiResult(ErrorCodeEnum? status, T result)
		{
			ErrorCode = status;
            if(status != null)
            {
                HumanReadableErrorCode = status.ToString();
            }
			Result = result;
		}

        public static ApiResult<T> Success(T result)
        {
            return new ApiResult<T>(null, result);
        }

        public static ApiResult<T> Error(ErrorCodeEnum error, string message)
        {
            return new ApiResult<T>(error, default) { ErrorMessage = message};
        }
    }

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
        public async Task<ApiResult> Read(int id)
        {
			var entry = (await GetEntry(DbCtx, id)).Value;
            if (entry == null) return ApiResult.NotFound();
			return ApiResult.Success(Activator.CreateInstance<ApiType>().ConvertToAPI(DbCtx, entry));
        }

        [HttpGet]
        [Route(nameof(ReadAll))]
        public async Task<ApiResult> ReadAll()
        {
            var list = DbCtx.Set<DbType>().ToList();
            return ApiResult.Success(list.Select(dbEnt => Activator.CreateInstance<ApiType>().ConvertToAPI(DbCtx, dbEnt)).ToList());
        }

        [HttpPost]
        [Route(nameof(Delete))]
        public async Task<ApiResult> Delete(int id)
        {
			var entry = (await GetEntry(DbCtx, id)).Value;
            if (entry == null) return ApiResult.NotFound();

			DbCtx.Remove(entry);
            await DbCtx.SaveChangesAsync();
            return ApiResult.Success(true);
        }

        [HttpPost]
        [Route(nameof(Update))]
        public async Task<ApiResult> Update(int id, [FromBody] ApiType value)
        {
            var entry = (await GetEntry(DbCtx, id)).Value;
            if (entry == null) return ApiResult.NotFound();
            
            value.Id = entry.Id;
            DbCtx.Entry(entry).CurrentValues.SetValues(value);
            
            await DbCtx.SaveChangesAsync();

            return ApiResult.Success(value);
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<ApiResult> Create([FromBody] ApiType value)
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
                        return ApiResult.Error(ApiResult.ErrorCodeEnum.InvalidInput, "The resulting object violates ownership constraints");
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
            return ApiResult.Success(value);
        }

        protected static async Task<DbType> CreateEntry(AppDatabaseContext dbCtx, ApiType value)
        {
            var entry = new DbType();
            await dbCtx.AddAsync(entry);
            return entry;
        }

        protected async Task<DatabaseAccessResult<DbType>> GetEntry(AppDatabaseContext dbCtx, int id)
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

		protected async Task AssignUserId(DbType entry)
		{
			var user = await UserManager.GetUserAsync(User);
			await AssignUserId(user, entry);
		}

		protected async Task<bool> UserHasAccess(DbType entry)
        {
            var user = await UserManager.GetUserAsync(User);
            return UserHasAccess(user, DbCtx, entry);
        }
        public abstract bool UserHasAccess(IdentityUser user, AppDatabaseContext context, DbType entry);
        public virtual async Task AssignUserId(IdentityUser user, DbType entry) { }
    }
}
