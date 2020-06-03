using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlightControlWeb.JsonableObjects;
using Newtonsoft.Json;

namespace FlightControlWeb.Model
{
    public class Model
    {
        private readonly Mutex PlansMutex = new Mutex();

        private readonly Mutex ServersMutex = new Mutex();


        /// <summary> id -> url mapping </summary>
        private readonly Dictionary<string, string> ExtServersURLs;

        private readonly Dictionary<string, FlightPlan> FltPlans;

        private readonly IGeneratorID GeneratorID;

        public int NumPlans => FltPlans.Count;


        public void AddExtServer(ServerDetails newServer)
        {
            ServersMutex.WaitOne();
            ExtServersURLs[newServer.ServerID] = newServer.ServerURL;
            ServersMutex.ReleaseMutex();
        }

        /// <summary>
        /// returns true on success
        /// </summary>
        public bool RemoveExtServer(string id)
        {
            ServersMutex.WaitOne();

            bool found = ExtServersURLs.ContainsKey(id);

            if (found)
            {
                ExtServersURLs.Remove(id);
            }

            ServersMutex.ReleaseMutex();

            return found;
        }

        public IEnumerable<ServerDetails> GetExtServers()
        {
            ServersMutex.WaitOne();

            foreach (var (id, url) in ExtServersURLs)
            {
                yield return new ServerDetails()
                {
                    ServerID = id,
                    ServerURL = url
                };
            }

            ServersMutex.ReleaseMutex();
        }

        public FlightPlan GetFlightPlan(string id)
        {
            FlightPlan result = null;

            PlansMutex.WaitOne();

            if (FltPlans.ContainsKey(id))
                result = FltPlans[id];

            PlansMutex.ReleaseMutex();

            if (result == null) // we'll have to ask external servers
            {
                result = FetchExtFltPlan(id);
            }

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

        private FlightPlan FetchExtFltPlan(string id)
        {
            ServersMutex.WaitOne();
            var remote_urls = new List<string>(ExtServersURLs.Values);
            ServersMutex.ReleaseMutex();

            foreach (var server_url in remote_urls)
            {
                string url = $"{server_url}/api/FlightPlan/{id}";

                var requestObj = WebRequest.Create(url);
                requestObj.Method = "GET";

                try
                {
                    var response = (HttpWebResponse)requestObj.GetResponse();
                    using var stream = response.GetResponseStream();
                    string json = new StreamReader(stream).ReadToEnd();

                    var plan = JsonConvert.
                        DeserializeObject<FlightPlan>(json);

                    return plan;
                }
                catch (WebException) { /* if key not found, keep on */ }
            }

            return null;
        }

        private IEnumerable<Flight> FetchExtFlights(DateTime dateTime)
        {
            var results = new List<Flight>();

            ServersMutex.WaitOne();
            var remote_urls = new List<string>(ExtServersURLs.Values);
            ServersMutex.ReleaseMutex();

            foreach (var server_url in remote_urls)
            {
                string iso = dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string url = $"{server_url}/api/Flights?relative_to={iso}";

                var requestObj = WebRequest.Create(url);
                requestObj.Method = "GET";

                try
                {
                    var response = requestObj.GetResponse();
                    using var stream = response.GetResponseStream();
                    string json = new StreamReader(stream).ReadToEnd();
                    var flights = JsonConvert.
                        DeserializeObject<List<Flight>>(json);

                    foreach (var flight in flights)
                    {
                        flight.is_external = true;
                    }

                    results.AddRange(flights);
                }
                catch (Exception e)
                {
                    // if someone gave us a fauly ext server, just log for now
                    Console.Error.WriteLine(
                        $"couldn't get ext. flights from {server_url}; '{e}'");
                }
            }

            return results;
        }

        public IEnumerable<Flight> GetLocalAndExtFlights(DateTime time)
        {
            foreach (var flight in GetLocalFlights(time))
            {
                yield return flight;
            }

            foreach (var flight in FetchExtFlights(time))
            {
                yield return flight;
            }
        }

        private Model(IGeneratorID ID_Generator)
        {
            this.GeneratorID = ID_Generator;
            this.FltPlans = new Dictionary<string, FlightPlan>();
            this.ExtServersURLs = new Dictionary<string, string>();
        }

        public static readonly Model SingletonInstance
            = new Model(new RandomBased_ID_Generator());
    }
}
/*
 this is the main one!!!
 */