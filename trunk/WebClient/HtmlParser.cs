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

		public static SubmitFuelResponse ParseSubmitFuelResponse(string response)
		{
			var result = new SubmitFuelResponse();
			var match = SuccessTemplate.Match(response);
			result.SuccessMessage = match.Success ? match.Groups["Message"].Value : string.Empty;
			match = ErrorTemplate.Match(response);
			result.ErrorMessage = match.Success ? match.Groups["Message"].Value : string.Empty;
			match = SyntaxErrorTemplate.Match(response);
			result.SyntaxErrorMessage = match.Success ? match.Groups["Message"].Value : string.Empty;
			return result;
		}

		private static readonly Regex CarTemplate = new Regex(@"<label for=.*?>Car:</label>(?<Car>\d*?)</div>");
		private static readonly Regex CarListItemTemplate = new Regex(@"<tr><td style=.*?>(?<CarId>\d*?)</td><td>(?<NumberOfFuels>\d*?)</td><td>.*?</td></tr>");
		private static readonly Regex ErrorTemplate = new Regex(@"<span id=""solution.errors"" class=""errors"">(?<Message>.*?)</span>", RegexOptions.Singleline);
		private static readonly Regex SyntaxErrorTemplate = new Regex(@"<pre>(?<Message>.*?)</pre><form id=""solution""", RegexOptions.Singleline);
		private static readonly Regex SuccessTemplate = new Regex(@"You have submitted fuel for car \d*? with size \d*?. A special comment for you:<pre>(?<Message>.*?)</pre>", RegexOptions.Singleline);
	}

	public struct SubmitFuelResponse
	{
		public string SuccessMessage;
		public string ErrorMessage;
		public string SyntaxErrorMessage;

		public override string ToString()
		{
			return string.Format("{0}{1}{2}", SuccessMessage, ErrorMessage, SyntaxErrorMessage);
		}
	}
}