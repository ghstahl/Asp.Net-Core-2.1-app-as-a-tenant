using System.Collections.Generic;
using System.Threading.Tasks;
using ApiWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodSingleton2Controller : ControllerBase
    {
        private ISingletonObjectCache<GoodSingleton2Controller, Dictionary<string, object>> _objectCache;

        public GoodSingleton2Controller(ISingletonObjectCache<GoodSingleton2Controller, Dictionary<string, object>> objectCache)
        {
            _objectCache = objectCache;
            if (_objectCache.Value == null)
            {
                _objectCache.Value = new Dictionary<string, object>();
            }
        }

        // GET: api/GoodSingleton
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            var dictionaryCache = _objectCache.Value;
            if (dictionaryCache.TryGetValue("my_data", out var result))
            {
                return result as List<string>;
            }

            return new List<string>();
        }
        // GET: api/GoodSingleton
        [HttpGet]
        [Route("clear")]
        public async Task GetClearAsync()
        {
            var dictionaryCache = _objectCache.Value;
            dictionaryCache.Clear();

        }
        // POST: api/GoodSingleton
        [HttpPost]
        public async Task PostAsync([FromBody] string value)
        {
            var dictionaryCache = _objectCache.Value;
            dictionaryCache["my_data"] = new List<string>() {value};
        }
    }
}