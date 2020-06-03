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
            return Model.Model.SingletonInstance.GetExtServers();
        }

        [HttpPost]
        public ActionResult Post([FromBody] ServerDetails newServer)
        {
            Model.Model.SingletonInstance.AddExtServer(newServer);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            bool success = Model.Model.SingletonInstance.
                RemoveExtServer(id);

            if (success) return Ok();

            return NotFound();
        }
    }
}
