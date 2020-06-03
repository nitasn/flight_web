using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.JsonableObjects;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServersController : Controller
    {
        [HttpGet]
        public IEnumerable<ServerDetails> Get()
        {
            throw new NotImplementedException("todo");
        }

        [HttpPost]
        public ActionResult Post([FromBody] ServerDetails newServer)
        {
            throw new NotImplementedException("todo");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            throw new NotImplementedException("todo");
        }
    }
}
