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
        public IEnumerable<string> Get()
        {
            return BadStaticData;
        }

        // POST: api/BadStatic
        [HttpPost]
        public void Post([FromBody] string value)
        {
            BadStaticData.Add(value);
        }
    }
}
