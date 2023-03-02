using System;
using Newtonsoft.Json;

namespace I8FlightParser.Data.Flights
{
    public class I8Segment
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("company")]
        public I8Company Company { get; set; }

        [JsonProperty("carrier")]
        public string Carrier { get; set; }

        [JsonProperty("racenumber")]
        public string RaceNumber { get; set; }

        [JsonProperty("departuredate")]
        public string DepartureDate { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("departuretime")]
        public string DepartureTime { get; set; }

        [JsonProperty("arrivaldate")]
        public string ArrivalDate { get; set; }

        [JsonProperty("arrivaltime")]
        public string ArrivalTime { get; set; }

        [JsonProperty("airplane")]
        public string Airplane { get; set; }

        [JsonProperty("airplaneName")]
        public string AirplaneName { get; set; }

        [JsonProperty("vehicleCodeEn")]
        public string VehicleCodeEn { get; set; }

        [JsonProperty("destinationport")]
        public string DestinationPort { get; set; }

        [JsonProperty("destinationportName")]
        public string DestinationPortName { get; set; }

        [JsonProperty("destinationcity")]
        public string DestinationCity { get; set; }

        [JsonProperty("destinationcityName")]
        public string DestinationCityName { get; set; }

        [JsonProperty("originport")]
        public string OriginPort { get; set; }

        [JsonProperty("originportName")]
        public string OriginPortName { get; set; }

        [JsonProperty("origincity")]
        public string OriginCity { get; set; }

        [JsonProperty("origincityName")]
        public string OriginCityName { get; set; }

        [JsonProperty("originTerminal")]
        public string OriginTerminal { get; set; }

        [JsonProperty("flighttime")]
        public string FlightTime { get; set; }

        [JsonProperty("delaydays")]
        public int DelayDays { get; set; }

        [JsonProperty("departuredayshift")]
        public int DepartureDayShift { get; set; }

        [JsonProperty("arrivaldayshift")]
        public int ArrivalDayShift { get; set; }

        [JsonProperty("landings")]
        public List<object> Landings { get; set; }
    }
}

