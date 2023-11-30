using AttendanceTracker.Models.API;
using Microsoft.AspNetCore.Mvc;
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
            return Activator.CreateInstance<ApiType>().ConvertToAPI(obj);
        }

        [HttpGet]
        [Route(nameof(ListAll))]
        public async Task<ActionResult<List<ApiType>>> ListAll()
        {
            var list = DbCtx.Set<DbType>().ToList();
            return list.Select(dbEnt => Activator.CreateInstance<ApiType>().ConvertToAPI(dbEnt)).ToList();
        }

        [HttpPost]
        [Route(nameof(Update))]
        public async Task<IActionResult> Update(KeyType id, ApiType value)
        {
            DbType? entry = await GetEntry(id);
            if (entry == null) return StatusCode((int)HttpStatusCode.NoContent);

            DbCtx.Entry(entry).CurrentValues.SetValues(value);
            
            await DbCtx.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<ActionResult<ApiType>> Create(ApiType value)
        {
            var entry = await CreateEntry(value);
            DbCtx.Entry(entry).CurrentValues.SetValues(value);
            await DbCtx.SaveChangesAsync();
            return value;
        }

        async Task<DbType> CreateEntry(ApiType value)
        {
            var entry = new DbType();
            if(entry is IGuidDbKey entryWithGuid && value is IGuidDbKey valueWithGuid)
            {
                var newGuid = Guid.NewGuid();
                valueWithGuid.Guid = newGuid;
                entryWithGuid.Guid = newGuid;
            }
            else if(entry is IStringNameDbKey entryWithName && value is IStringNameDbKey name)
            {
                entryWithName.Name = name.Name;
            }
            else
            {
                throw new InvalidOperationException("Unknown key type. " + typeof(KeyType));
            }
            await DbCtx.AddAsync(entry);
            return entry;
        }

        async Task<DbType?> GetEntry(KeyType id)
        {
            if(id is Guid guid)
            {
                return DbCtx.Set<DbType>().FirstOrDefault(s => ((IGuidDbKey)s).Guid == guid);
            }
            else 
            {
                return await DbCtx.FindAsync<DbType>(id);
            }
        }
    }
}
