using System;
using Newtonsoft.Json;

namespace I8FlightParser.Data.Flights
{
    public class I8Price
    {
        [JsonProperty("clientId")]
        public int ClientId { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("originalPrice")]
        public string OriginalPrice { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("requestedBrands")]
        public string RequestedBrands { get; set; }

        [JsonProperty("available")]
        public int Available { get; set; }

        [JsonProperty("promo")]
        public bool Promo { get; set; }
    }
}

