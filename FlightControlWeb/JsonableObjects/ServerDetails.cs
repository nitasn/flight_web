using System;
using System.Text.Json.Serialization;

namespace FlightControlWeb.JsonableObjects
{
    public class ServerDetails
    {
        [JsonPropertyName("ServerId")]
        public string ServerID { get; set; }

        [JsonPropertyName("ServerURL")]
        public string ServerURL { get; set; }
    }
}
