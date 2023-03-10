using System.Diagnostics;
using System.Globalization;
using System.Text;
using I8FlightParser;
using I8FlightParser.Abstract;
using I8FlightParser.Data;
using I8FlightParser.Data.Flights;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

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

var telegramKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "telegram_key.json");
if (!System.IO.File.Exists(telegramKeyPath))
{
    await System.IO.File.WriteAllTextAsync(telegramKeyPath, JsonConvert.SerializeObject(new TelegramKey()));
}

var telegramKey = JsonConvert.DeserializeObject<TelegramKey>(await System.IO.File.ReadAllTextAsync(telegramKeyPath));
var bot = new TelegramBotClient(telegramKey.Token);

Func<I8Segment, I8Price, string> getFlightStrFormatted = (flightInfo, priceInfo) =>
{
    var flightStr = $"{flightInfo.DepartureDate} {flightInfo.DepartureTime}: " +
        $"{flightInfo.OriginPort} - {flightInfo.DestinationPort}";

    var currencyStr = $": {priceInfo.Price} {priceInfo.Currency}";

    var seatsStr = priceInfo.Available < 9 ? $": number of seats is {priceInfo.Available}" : string.Empty;

    return $"{flightStr}{currencyStr}{seatsStr}";
};

var cheapestResults = searchCriteries.ToDictionary(x => x.SearchId, x => new List<(string Flight, I8Price Price)>());

try
{
    while (true)
    {
        var notificationBuilder = new StringBuilder();

        var searchService = new I8FlightSearchService();
        foreach (I8SearchCriteria searchCriteria in searchCriteries)
        {
            var resultTextBuilder = new StringBuilder();

            var searchResult = await searchService.Search(searchCriteria);

            var returnDateStr = searchCriteria.ReturnDate.HasValue
                ? $" - {searchCriteria.ReturnDate.Value.ToString("dd.MM.yyyy")}"
                : string.Empty;
            var dateStr = $"{searchCriteria.DepartureDate.ToString("dd.MM.yyyy")}{returnDateStr}";
            var routeStr = $"{searchCriteria.DepartureIata} - {searchCriteria.ArrivalIata}";

            var header = $"-------- last update {DateTime.Now}: date {dateStr}: route {routeStr} --------";
            resultTextBuilder.AppendLine(header);

            var cheapestVariants = new List<(string Flight, I8Price Price)>();

            var chains = searchResult.Flights.OrderBy(x =>
                DateTime.ParseExact(x.Segments.First().DepartureDate, "dd.MM.yyyy", CultureInfo.InvariantCulture));

            foreach (var chain in chains)
            {
                foreach (var flight in chain.Segments)
                {
                    var prices = searchResult.Prices.First(x => x.ContainsKey(flight.Id)).SelectMany(x => x.Value)
                        .OrderBy(x => x.Price);
                    foreach (var price in prices)
                    {
                        //resultTextBuilder.AppendLine(getFlightStrFormatted(flight, price));
                    }

                    var cheapestPrice = prices.First();
                    cheapestVariants.Add((getFlightStrFormatted(flight, cheapestPrice), cheapestPrice));
                }

                if (searchResult.Flights.Count > 1)
                {
                    //resultTextBuilder.AppendLine();
                }
            }

            resultTextBuilder.AppendLine();
            resultTextBuilder.AppendLine("Cheapest variants:");

            foreach (var variant in cheapestVariants)
            {
                resultTextBuilder.AppendLine(variant.Flight);
            }

            resultTextBuilder.AppendLine(
                $"Total sum: {cheapestVariants.Sum(x => x.Price.Price)} {cheapestVariants[0].Price.Currency}");

            resultTextBuilder.AppendLine(new string('-', header.Length));
            resultTextBuilder.AppendLine();

            var prevCheapestVariants = cheapestResults[searchCriteria.SearchId];
            cheapestResults[searchCriteria.SearchId] = cheapestVariants;

            if (prevCheapestVariants.Count != cheapestVariants.Count
                || prevCheapestVariants.Count == 0
                || cheapestVariants.Count == 0)
            {
                notificationBuilder.Append(resultTextBuilder);
            }
            else
            {
                for (var i = 0; i < cheapestVariants.Count; i++)
                {
                    if (prevCheapestVariants[i].Flight != cheapestVariants[i].Flight)
                    {
                        notificationBuilder.Append(resultTextBuilder);
                        break;
                    }
                }
            }
        }

        var result = notificationBuilder.ToString();
        Console.Write(result);

        if (result.Length > 0)
        {
            await bot.SendTextMessageAsync(telegramKey.ChanelId, result);
        }

        // 1 hours * 3600 sec * 1000ms.
        await Task.Delay(1 * 3600 * 1000);
    }
}
catch (Exception ex)
{
    await bot.SendTextMessageAsync(telegramKey.ChanelId, $"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
}
