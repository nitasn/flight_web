using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightPlanController : Controller
    {
        /// <summary>
        /// adds a flight-plan to the model,
        /// and returns the plan's new unique id.
        /// </summary>
        [HttpPost]
        public ActionResult<string> Post([FromBody] FlightPlan plan)
        {
            var new_id = Model.Model.SingletonInstance.Add(plan);

            return new_id;
        }

        [HttpGet("{id}")]
        public ActionResult<FlightPlan> GetFlightPlan(string id)
        {
            if (Model.Model.SingletonInstance.HasFlightPlan(id))
            {
                return Model.Model.SingletonInstance.GetFlightPlan(id);
            }

            return NotFound();
        }
    }
}
