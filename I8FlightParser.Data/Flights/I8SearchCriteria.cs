using System;
using I8FlightParser.Abstract;

namespace I8FlightParser.Data.Flights
{
    public class I8SearchCriteria : ISearchCriteria
    {
        public string SearchGroupId { get; set; } = "standard";

        public int SegmentsCount => ReturnDate.HasValue ? 2 : 1;

        public DateTime DepartureDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public string DepartureIata { get; set; }

        public string ArrivalIata { get; set; }

        public int AdultsCount { get; set; } = 1;

        public int ChildrenCount { get; set; }

        public int InfantsWithSeatCount { get; set; }

        public int InfantsWithoutSeatCount { get; set; }

        public FormUrlEncodedContent ToFormUrlEncodedContent()
        {
            var data = new Dictionary<string, string>
            {
                { "searchGroupId", SearchGroupId },
                { "segmentsCount", SegmentsCount.ToString() },
                { "date[0]", DepartureDate.ToString("dd.MM.yyyy") },
                { "origin-city-code[0]", DepartureIata },
                { "destination-city-code[0]", ArrivalIata },
                { "adultsCount", AdultsCount.ToString() },
                { "childrenCount", ChildrenCount.ToString() },
                { "infantsWithSeatCount", InfantsWithSeatCount.ToString() },
                { "infantsWithoutSeatCount", InfantsWithoutSeatCount.ToString() }
            };

            if (ReturnDate.HasValue)
            {
                data.Add("date[1]", ReturnDate.Value.ToString("dd.MM.yyyy"));
                data.Add("origin-city-code[1]", ArrivalIata);
                data.Add("destination-city-code[1]", DepartureIata);
            }

            return new FormUrlEncodedContent(data);
        }
    }
}

