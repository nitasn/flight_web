using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    public class Model
    {
        //private Mutex plansMutex = new Mutex();

        private Dictionary<string, FlightPlan> plans;

        private I_ID_Generator ID_Generator;

        public FlightPlan GetFlightPlan(string id)
        {
            return plans[id]; // todo mutex?
        }

        public bool HasFlightPlan(string id)
        {
            return plans.ContainsKey(id);
        }

        /// <summary>
        /// retuns new_id
        /// </summary>
        public string Add(FlightPlan plan)
        {
            string new_id = ID_Generator.GenerateID();

            //plansMutex.WaitOne();
            plans[new_id] = plan;
            //plansMutex.ReleaseMutex();

            return new_id;
        }

        private bool IsInAir(FlightPlan plan, DateTime time)
        {
            var start = plan.initial_location.date_time;
            var end = start + plan.TotalTime();

            return start <= time && time <= end;
        }

        public IEnumerable<Flight> GetLocalFlights(DateTime time)
        {
            //plansMutex.WaitOne();
            //var _plans = plans.Values.ToList();
            //plansMutex.ReleaseMutex();

            foreach (var (id, plan) in plans)
            {
                if (!IsInAir(plan, time))
                    continue;

                var (lat, lon) = plan.InterpolateLocation(time);

                lat = Math.Round(lat, 5);
                lon = Math.Round(lon, 5);

                yield return new Flight()
                {
                    company_name = plan.company_name,
                    date_time = plan.initial_location.date_time,
                    flight_id = id,
                    is_external = false,
                    passengers = plan.passengers,
                    latitude = lat,
                    longitude = lon
                };
            };
        }

        //private Flight FlightObj(DateTime time, string id)
        //{
        //    //plansMutex.WaitOne(); // todo is this needed?
        //    var plan = plans[id];
        //    //plansMutex.ReleaseMutex();

        //    (var latitude, var longtitude) = (3, 3);

        //    return new Flight()
        //    {
        //        company_name = plan.company_name,
        //        flight_id = id,
        //        is_external = true,
        //        passengers = plan.passengers,
        //        date_time = plan.initial_location.date_time,
                
        //    };
        //}

        private Model(I_ID_Generator ID_Generator)
        {
            this.ID_Generator = ID_Generator;
            this.plans = new Dictionary<string, FlightPlan>();
        }

        public static readonly Model SingletonInstance
            = new Model(new RandomBased_ID_Generator());
    }
}