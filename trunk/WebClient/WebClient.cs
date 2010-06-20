using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace WebClient
{
	public class WebClient
	{
		private string sessionId = "JSESSIONID=0618F74BE5535FF09182497A418B2B36";

		public void GetCarsList()
		{
			var sb = new StringBuilder();
			var response = GetResponse(getCarsList);
			var carIds = HtmlParser.ParseCarsList(response);
			foreach (var carId in carIds)
			{
				var car = GetCar(carId);
				sb.AppendLine(carId);
				sb.AppendLine(car);
			}
			File.WriteAllText("cars.txt", sb.ToString());
		}

		public string GetCar(string carId)
		{
			var response = GetResponse(string.Format(getCar, carId));
			File.WriteAllText("car-response.txt", response);
			var car = HtmlParser.ParseCar(response);
			return car;
		}

		private string GetResponse(string requestUri)
		{
			var req = WebRequest.Create(requestUri);
			req.Headers.Add("Cookie", sessionId);
			using (var resp = req.GetResponse())
				return GetContent(resp.GetResponseStream());
		}

		private static string GetContent(Stream stream)
		{
			using (var sr = new StreamReader(stream, Encoding.UTF8))
				return sr.ReadToEnd();
		}

		private const string login = @"http://icfpcontest.org/icfp10/login";
		private const string getCarsList = @"http://icfpcontest.org/icfp10/score/instanceTeamCount";
		private const string submitCar = @"http://icfpcontest.org/icfp10/instance/form";
		private const string getCar = @"http://icfpcontest.org/icfp10/instance/{0}/solve/form";
		private const string submitFuel = @"http://icfpcontest.org/icfp10/instance/{0}/solve";
	}

	static class HtmlParser
	{
		public static IEnumerable<string> ParseCarsList(string response)
		{
			var matches = CarListItemTemplate.Matches(response);
			foreach (Match match in matches)
			{
				string carId = match.Groups["CarId"].Value;
				yield return carId;
			}
		}

		public static string ParseCar(string response)
		{
			var match = CarTemplate.Match(response);
			string car = match.Groups["Car"].Value;
			return car;
		}

		private static readonly Regex CarTemplate = new Regex(@"<label for=.*?>Car:</label>(?<Car>\d*?)</div>");
		private static readonly Regex CarListItemTemplate = new Regex(@"<tr><td style=.*?>(?<CarId>\d*?)</td><td>(?<NumberOfFuels>\d*?)</td><td>.*?</td></tr>");
	}
}
