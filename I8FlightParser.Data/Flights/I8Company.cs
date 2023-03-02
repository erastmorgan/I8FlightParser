using System;
using Newtonsoft.Json;

namespace I8FlightParser.Data.Flights
{
    public class I8Company
    {
        [JsonProperty("code_en")]
        public string CodeEn { get; set; }

        [JsonProperty("name_en")]
        public string NameEn { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

