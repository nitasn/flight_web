using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlightControlWeb
{
    public class FlightPlan
    {
        [JsonPropertyName("passengers")]
        public int passengers { get; set; }

        [JsonPropertyName("company_name")]
        public string company_name { get; set; }

        [JsonPropertyName("initial_location")]
        public InitialPosition initial_location { get; set; }

        [JsonPropertyName("segments")]
        public List<FlightSegment> segments { get; set; }
    }

    public class FlightSegment
    {
        [JsonPropertyName("longitude")]
        public double longitude { get; set; }

        [JsonPropertyName("latitude")]
        public double latitude { get; set; }

        [JsonPropertyName("timespan_seconds")]
        public double timespan_seconds { get; set; }
    }

    public class InitialPosition
    {
        [JsonPropertyName("longitude")]
        public double longitude { get; set; }

        [JsonPropertyName("latitude")]
        public double latitude { get; set; }

        [JsonPropertyName("date_time")]
        public DateTime date_time { get; set; }
    }
}
