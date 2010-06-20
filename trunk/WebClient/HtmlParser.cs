using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebClient
{
	static class HtmlParser
	{
		public static IEnumerable<string> ParseCarsList(string response)
		{
			var matches = CarListItemTemplate.Matches(response);
			foreach (Match match in matches)
			{
				var carId = match.Groups["CarId"].Value;
				yield return carId;
			}
		}

		public static string ParseCar(string response)
		{
			var match = CarTemplate.Match(response);
			var car = match.Groups["Car"].Value;
			return car;
		}

		public static string ParseSubmitFuelResponse(string response)
		{
			var match = ErrorTemplate.Match(response);
			var error = match.Success ? match.Groups["Error"].Value : string.Empty;
			if (string.IsNullOrEmpty(error))
			{
				match = SyntaxErrorTemplate.Match(response);
				error = match.Success ? match.Groups["Error"].Value : string.Empty;
			}
			return error;
		}

		private static readonly Regex CarTemplate = new Regex(@"<label for=.*?>Car:</label>(?<Car>\d*?)</div>");
		private static readonly Regex CarListItemTemplate = new Regex(@"<tr><td style=.*?>(?<CarId>\d*?)</td><td>(?<NumberOfFuels>\d*?)</td><td>.*?</td></tr>");
		private static readonly Regex ErrorTemplate = new Regex(@"<span id=""solution.errors"" class=""errors"">(?<Error>.*?)</span>", RegexOptions.Singleline);
		private static readonly Regex SyntaxErrorTemplate = new Regex(@"<pre>(?<Error>.*?)</pre><form id=""solution""", RegexOptions.Singleline);
	}
}