using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : Controller
    {
        //// GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (Model.Model.SingletonInstance.Delete(id))
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpGet]
        public IEnumerable<Flight> Get([FromQuery] DateTime relative_to)
        {
            return Model.Model.SingletonInstance.GetLocalFlights(relative_to);
        }
    }
}
