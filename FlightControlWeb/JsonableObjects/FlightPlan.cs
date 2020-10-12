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
        // FIXME there is a huge bug!!!
        //  in the automatic parsing iso-string dates, the time-zone is lost!
        //  In C#, DateTime.Parse("2020-10-12T23:30:33.594Z", ...).ToString() == "10/12/2020 11:30:33 PM"
        //  But in JS, new Date('2020-10-12T23:30:33.594Z').toString() === "Tue Oct 13 2020 02:30:33 GMT+0300 (Israel Daylight Time)"
        //
        //  possible fixes: the property date_time can be a string... we can parse it ourselves. 
        //  or, to use unix time stamps instead of iso-strings. can C#'s DateTime deal with that? probably
        
    }
}
