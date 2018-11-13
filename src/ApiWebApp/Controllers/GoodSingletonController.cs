using System.Collections.Generic;
using System.Threading.Tasks;
using ApiWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodSingletonController : ControllerBase
    {
        private ISingletonDictionaryCache<GoodSingletonController> _dictionaryCache;

 
        public GoodSingletonController(ISingletonDictionaryCache<GoodSingletonController> dictionaryCache)
        {
            _dictionaryCache = dictionaryCache;
        }

        // GET: api/GoodSingleton
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            var result = _dictionaryCache.Get("my_data") as IEnumerable<string>;
            return result ?? new List<string>();
        }
        // GET: api/GoodSingleton/clear
        [HttpGet]
        [Route("clear")]
        public async Task GetClearAsync()
        {
            _dictionaryCache.Clear();
           
        }
        // POST: api/GoodSingleton
        [HttpPost]
        public async Task PostAsync([FromBody] string value)
        {
            _dictionaryCache.Set("my_data",new List<string>(){value});
        }
    }
}