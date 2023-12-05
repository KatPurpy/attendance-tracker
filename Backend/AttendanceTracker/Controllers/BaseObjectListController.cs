using AttendanceTracker.Models.API;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;

namespace AttendanceTracker.Controllers
{
    [Route("api/[controller]")]
    public class BaseObjectListController<DbType,ApiType> : Controller 
        where DbType : class, IIntDbKey, new()
        where ApiType : class, IIntDbKey, IAPIModelFor<ApiType,DbType>
    {
        public DbCtx DbCtx { get; set; }

        public BaseObjectListController(DbCtx dbCtx)
        {
            DbCtx = dbCtx;
        }

        [HttpGet]
        [Route(nameof(Read))]
        public async Task<ActionResult<ApiType>> Read(int id)
        {
            var obj = await GetEntry(DbCtx,id);
            if (obj == null) return StatusCode((int)HttpStatusCode.NoContent);
            return Activator.CreateInstance<ApiType>().ConvertToAPI(DbCtx, obj);
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
            var obj = await GetEntry(DbCtx, id);
            if (obj == null) return StatusCode((int)HttpStatusCode.NoContent);
            DbCtx.Remove(obj);
            await DbCtx.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route(nameof(Update))]
        public async Task<ActionResult<ApiType>> Update(int id, [FromBody] ApiType value)
        {
            DbType? entry = await GetEntry(DbCtx, id);
            if (entry == null) return StatusCode((int)HttpStatusCode.NoContent);
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
            await DbCtx.SaveChangesAsync();
            value.Id = entry.Id;
            return value;
        }

        public static async Task<DbType> CreateEntry(DbCtx dbCtx, ApiType value)
        {
            var entry = new DbType();
            await dbCtx.AddAsync(entry);
            return entry;
        }

        public static async Task<DbType?> GetEntry(DbCtx dbCtx, int id)
        {
            return await dbCtx.FindAsync<DbType>(id);
        }
    }
}
