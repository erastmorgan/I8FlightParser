using System;
using Newtonsoft.Json;
using System.Diagnostics;

namespace I8FlightParser.Data.Flights
{
	public class I8SearchResult
	{
        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("gotoPassengers")]
        public bool GotoPassengers { get; set; }

        [JsonProperty("flights")]
        public List<I8Flight> Flights { get; set; }

        [JsonProperty("prices")]
        public Dictionary<long, List<I8Price>>[] Prices { get; set; }
    }
}

