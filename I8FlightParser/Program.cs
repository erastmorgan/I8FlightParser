using System.Diagnostics;
using System.Globalization;
using I8FlightParser;
using I8FlightParser.Abstract;
using I8FlightParser.Data.Flights;
using Newtonsoft.Json;

var searchCriteries = new ISearchCriteria[]
{
    new I8SearchCriteria
    {
        DepartureDate = new DateTime(2023, 04, 28),
        ReturnDate = new DateTime(2023, 05, 02),
        DepartureIata = "LED",
        ArrivalIata = "IJK"
    },
    new I8SearchCriteria
    {
        DepartureDate = new DateTime(2023, 07, 12),
        ReturnDate = new DateTime(2023, 07, 16),
        DepartureIata = "LED",
        ArrivalIata = "IJK"
    }
};

while (true)
{
    var searchService = new I8FlightSearchService();
    foreach (I8SearchCriteria searchCriteria in searchCriteries)
    {
        var searchResult = await searchService.Search(searchCriteria);

        //var flight = searchResult.Flights.
        //var cheapestPrice = searchResult.Prices.fi

        var returnDateStr = searchCriteria.ReturnDate.HasValue
            ? $" - {searchCriteria.ReturnDate.Value.ToString("dd.MM.yyyy")}"
            : string.Empty;
        var dateStr = $"{searchCriteria.DepartureDate.ToString("dd.MM.yyyy")}{returnDateStr}";
        var routeStr = $"{searchCriteria.DepartureIata} - {searchCriteria.ArrivalIata}";

        var header = $"-------- last update {DateTime.Now}: date {dateStr}: route {routeStr} --------";
        Console.WriteLine(header);

        var cheapestVariants = new List<(string Flight, decimal Price)>();

        var chains = searchResult.Flights.OrderBy(x =>
            DateTime.ParseExact(x.Segments.First().DepartureDate, "dd.MM.yyyy", CultureInfo.InvariantCulture));

        foreach (var chain in chains)
        {
            foreach (var flight in chain.Segments)
            {
                var flightStr = $"{flight.DepartureDate} {flight.DepartureTime}: " +
                    $"{flight.OriginPort} - {flight.DestinationPort}";

                var prices = searchResult.Prices.First(x => x.ContainsKey(flight.Id)).SelectMany(x => x.Value)
                    .OrderBy(x => x.Price);
                foreach (var price in prices)
                {
                    Console.WriteLine($"{flightStr}: {price.Price} RUB");
                }

                var cheapestPrice = prices.First().Price;
                cheapestVariants.Add(($"{flightStr}: {cheapestPrice} RUB", cheapestPrice));
            }

            if (searchResult.Flights.Count > 1)
            {
                Console.WriteLine();
            }
        }

        Console.WriteLine();
        Console.WriteLine("Cheapest variants:");

        foreach (var variant in cheapestVariants)
        {
            Console.WriteLine(variant.Flight);
        }

        Console.WriteLine($"Total sum: {cheapestVariants.Sum(x => x.Price)} RUB");

        Console.WriteLine(new string('-', header.Length));
        Console.WriteLine();
    }

    // 6 hours * 3600 sec * 1000ms.
    await Task.Delay(6 * 3600 * 1000);
}