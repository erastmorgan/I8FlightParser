using System;
using System.Net;
using I8FlightParser.Abstract;
using I8FlightParser.Data.Flights;
using Newtonsoft.Json;

namespace I8FlightParser
{
	public class I8FlightSearchService
	{
		private const string SEARCH_URL = "https://booking.izhavia.su/websky/json/search-variants-mono-brand-cartesian";

        public async Task<I8SearchResult> Search(ISearchCriteria searchCriteria)
		{
			var formData = searchCriteria.ToFormUrlEncodedContent();

			//var qwe = await formData.ReadAsStringAsync();
			//Console.WriteLine(WebUtility.UrlDecode(string.Join(Environment.NewLine, qwe.Split('&'))));

            var response = await new HttpClient().PostAsync(SEARCH_URL, formData);

            var responseMsg = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<I8SearchResult>(responseMsg);
        }
	}
}

