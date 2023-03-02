using System;
using Newtonsoft.Json;

namespace I8FlightParser.Data.Flights
{
	public class I8Flight
	{
        [JsonProperty("chainIndex")]
        public int ChainIndex { get; set; }

        [JsonProperty("chainId")]
        public string ChainId { get; set; }

        [JsonProperty("flights")]
        public List<I8Segment> Segments { get; set; }

        [JsonProperty("connections")]
        public List<object> Connections { get; set; }

        [JsonProperty("flightTime")]
        public string FlightTime { get; set; }
    }
}

