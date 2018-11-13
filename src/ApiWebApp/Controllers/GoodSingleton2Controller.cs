using System.Collections.Generic;
using ApiWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodSingleton2Controller : ControllerBase
    {
        private ISingletonDictionaryCache<GoodSingleton2Controller> _dictionaryCache;

 
        public GoodSingleton2Controller(ISingletonDictionaryCache<GoodSingleton2Controller> dictionaryCache)
        {
            _dictionaryCache = dictionaryCache;
        }

        // GET: api/GoodSingleton
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _dictionaryCache.Get("my_data") as IEnumerable<string>;
        }

        // POST: api/GoodSingleton
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _dictionaryCache.Set("my_data",new List<string>(){value});
           
        }
    }
}