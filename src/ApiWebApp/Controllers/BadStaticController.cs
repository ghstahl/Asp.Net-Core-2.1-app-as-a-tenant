using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BadStaticController : ControllerBase
    {
        /*
         * This is really bad as this static is shared between all the tenants.
         */
        private static List<string> _badStatic;

        private static List<string> BadStaticData => _badStatic ?? (_badStatic = new List<string>());

        // GET: api/BadStatic
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            return BadStaticData;
        }
        // GET: api/GoodSingleton/clear
        [HttpGet]
        [Route("clear")]
        public async Task GetClearAsync()
        {
            BadStaticData.Clear();

        }
        // POST: api/BadStatic
        [HttpPost]
        public async Task PostAsync([FromBody] string value)
        {
            BadStaticData.Add(value);
        }
    }
}
