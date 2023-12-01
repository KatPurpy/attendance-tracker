using AttendanceTracker.Models.API;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;

namespace AttendanceTracker.Controllers
{
    [Route("api/[controller]")]
    public class BaseObjectListController<KeyType,DbType,ApiType> : Controller 
        where DbType : class, new()
        where ApiType : class, IAPIModelFor<ApiType,DbType>
    {
        public DbCtx DbCtx { get; set; }

        public BaseObjectListController(DbCtx dbCtx)
        {
            DbCtx = dbCtx;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiType>> Read(KeyType id)
        {
            var obj = await GetEntry(id);
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
        public async Task<ActionResult> Delete(KeyType id)
        {
            var obj = await GetEntry(id);
            if (obj == null) return StatusCode((int)HttpStatusCode.NoContent);
            DbCtx.Remove(obj);
            await DbCtx.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route(nameof(Update))]
        public async Task<IActionResult> Update(KeyType id, [FromBody] ApiType value)
        {
            DbType? entry = await GetEntry(id);
            if (entry == null) return StatusCode((int)HttpStatusCode.NoContent);
            CopyGuidToApiValue(entry, value);
            DbCtx.Entry(entry).CurrentValues.SetValues(value);
            
            await DbCtx.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<ActionResult<ApiType>> Create([FromBody] ApiType value)
        {
            var entry = await CreateEntry(value);
            DbCtx.Entry(entry).CurrentValues.SetValues(value);
            await DbCtx.SaveChangesAsync();
            return value;
        }

        private void CopyGuidToApiValue(DbType dbType, ApiType apiType)
        {
            if (dbType is IIntDbKey dbTypeWithGuid && apiType is IIntDbKey dbKeyWithGuid)
            {
                dbKeyWithGuid.Id = dbTypeWithGuid.Id;
            }
        }
        private async Task<DbType> CreateEntry(ApiType value)
        {
            var entry = new DbType();
            await DbCtx.AddAsync(entry);
            return entry;
        }

        private async Task<DbType?> GetEntry(KeyType id)
        {
            return await DbCtx.FindAsync<DbType>(id);
        }
    }
}
