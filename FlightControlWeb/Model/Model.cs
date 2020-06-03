using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    public class Model
    {
        private readonly Mutex PlansMutex = new Mutex();

        //class DummyMutex
        //{
        //    public void WaitOne() { }
        //    public void ReleaseMutex() { }
        //}
        //private readonly DummyMutex PlansMutex = new DummyMutex();

        private readonly Dictionary<string, FlightPlan> FltPlans;

        private readonly IGeneratorID GeneratorID;

        public int NumPlans => FltPlans.Count;

        public FlightPlan GetFlightPlan(string id)
        {
            PlansMutex.WaitOne();
            var result = FltPlans[id];
            PlansMutex.ReleaseMutex();
            
            return result;
        }

        public bool HasFlightPlan(string id)
        {
            PlansMutex.WaitOne();
            var result = FltPlans.ContainsKey(id);
            PlansMutex.ReleaseMutex();

            return result;
        }

        /// <summary>
        /// retuns new_id
        /// </summary>
        public string Add(FlightPlan plan)
        {
            string new_id = GeneratorID.GenerateID();

            PlansMutex.WaitOne();
            FltPlans[new_id] = plan;
            PlansMutex.ReleaseMutex();

            return new_id;
        }

        public bool Delete(string id)
        {
            PlansMutex.WaitOne();
            var success = FltPlans.ContainsKey(id);
            if (success) FltPlans.Remove(id);
            PlansMutex.ReleaseMutex();

            return success;
        }

        private bool IsInAir(FlightPlan plan, DateTime time)
        {
            var start = plan.initial_location.date_time;
            var end = start + plan.TotalTime();

            return start <= time && time <= end;
        }

        public IEnumerable<Flight> GetLocalFlights(DateTime time)
        {
            PlansMutex.WaitOne();

            foreach (var (id, plan) in FltPlans)
            {
                if (!IsInAir(plan, time))
                    continue;

                var (lat, lon) = plan.InterpolateLocation(time);

                //lat = Math.Round(lat, 5);
                //lon = Math.Round(lon, 5);

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
            }

            PlansMutex.ReleaseMutex();
        }

        private Model(IGeneratorID ID_Generator)
        {
            this.GeneratorID = ID_Generator;
            this.FltPlans = new Dictionary<string, FlightPlan>();
        }

        public static readonly Model SingletonInstance
            = new Model(new RandomBased_ID_Generator());
    }
}