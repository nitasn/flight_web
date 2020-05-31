using System;
using System.Text.Json.Serialization;

public class Flight
{
    [JsonPropertyName("flight_id")]
    public string flight_id { get; set; }

    [JsonPropertyName("longitude")]
    public double longitude { get; set; }

    [JsonPropertyName("latitude")]
    public double latitude { get; set; }

    [JsonPropertyName("passengers")]
    public int passengers { get; set; }

    [JsonPropertyName("company_name")]
    public string company_name { get; set; }

    [JsonPropertyName("date_time")]
    public DateTime date_time { get; set; }

    [JsonPropertyName("is_external")]
    public bool is_external { get; set; }
}