using AttendanceTracker.Models.API;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AttendanceTracker.Controllers
{
    [Route("api/[controller]")]
    public class BaseObjectListController<KeyType,DbType,ApiType> : Controller 
        where DbType : class,new()
        where ApiType : class, IAPIModelFor<ApiType,DbType>
    {
        public DbCtx DbCtx { get; set; }

        private string[] primaryKeys;

        public BaseObjectListController(DbCtx dbCtx, params string[] primaryKeys)
        {
            DbCtx = dbCtx;
            this.primaryKeys = primaryKeys;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiType>> Read(KeyType id)
        {
            var obj = await DbCtx.FindAsync<DbType>(id);
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
        [Route(nameof(Write))]
        public async Task<IActionResult> Write(ApiType value)
        {
            var apiPrimaryKeyProperties = primaryKeys.ToDictionary(name => name, name => typeof(ApiType).GetProperty(name));
            var dbPrimaryKeyProperties = primaryKeys.ToDictionary(name => name, name => typeof(DbType).GetProperty(name));

            var apiPrimaryKeyValues = apiPrimaryKeyProperties.Values.Select(prop => prop.GetValue(value)).ToArray();

            var entry = await DbCtx.FindAsync<DbType>(apiPrimaryKeyValues);

            if (entry == null)
            {
                entry = new DbType();
                
                foreach(var primaryKey in primaryKeys)
                {
                    var entryProperty = dbPrimaryKeyProperties[primaryKey];
                    var inputValue = apiPrimaryKeyProperties[primaryKey].GetValue(value);
                    entryProperty.SetValue(entry, inputValue);
                }

                await DbCtx.AddAsync(entry);
            }
            
            DbCtx.Entry(entry).CurrentValues.SetValues(value);
            
            await DbCtx.SaveChangesAsync();

            return Ok();
        }
    }
}
