using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CircuitCalc.WebClient
{
	public class IcfpcWebClient
	{
		public static string HttpPost(string uri, string parameters)
		{
			WebRequest webRequest = WebRequest.Create(uri);
			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.Method = "POST";
			byte[] bytes = Encoding.ASCII.GetBytes(parameters);
			Stream os = null;
			try
			{ // send the Post
				webRequest.ContentLength = bytes.Length;   //Count bytes to send
				os = webRequest.GetRequestStream();
				os.Write(bytes, 0, bytes.Length);         //Send it
			}
			catch (WebException ex)
			{
				Console.WriteLine(ex.Message, "HttpPost: Request error");
			}
			finally
			{
				if (os != null)
				{
					os.Close();
				}
			}

			try
			{ // get the response
				WebResponse webResponse = webRequest.GetResponse();
				if (webResponse == null)
				{ return null; }
				StreamReader sr = new StreamReader(webResponse.GetResponseStream());
				return sr.ReadToEnd().Trim();
			}
			catch (WebException ex)
			{
				Console.WriteLine(ex.Message, "HttpPost: Response error");
			}
			return HttpPost(uri, parameters);
		} // end HttpPost 

		//TODO: login почему-то не фурычит
		public IcfpcWebClient(string sessionId)
		{
			this.sessionId = string.IsNullOrEmpty(sessionId.Trim()) ? Login() : sessionId;
		}

		public IEnumerable<string> GetCarIdsList()
		{
			return GetCarIdsList(10000);
		}

		public IEnumerable<string> GetCarIdsList(int maxPages)
		{
			var carIds = new HashSet<string>();
			for(int page = 1; page < maxPages; page++)
			{
				var response = GetResponse(string.Format(getPagedCarsList, page));
				if(response.Contains("No cars found."))
				{
					Console.WriteLine("pages: {0}", page - 1);
					break;
				}
				foreach(var carId in HtmlParser.ParseCarsList(response))
				{
					carIds.Add(carId);
				}
			}
			return carIds.ToArray();
		}

		public Dictionary<string, string> GetCarsList()
		{
			var carIds = GetCarIdsList();
			var cars = new Dictionary<string, string>();
			foreach(var carId in carIds)
			{
				var car = GetCar(carId);
				cars.Add(carId, car);
			}
			return cars;
		}

		public string GetCar(string carId)
		{
			var response = GetResponse(string.Format(getCar, carId));
			//File.WriteAllText(string.Format("car-{0}.html", carId), response);
			var car = HtmlParser.ParseCar(response);
			return car;
		}

		public SubmitFuelResponse SubmitFuel(string carId, string factory)
		{
			try
			{
				var response = Post(string.Format(submitFuel, carId), Escape(factory));
				File.WriteAllText(string.Format("submit-fuel-for-car-{0}.html", carId), response);
				var result = HtmlParser.ParseSubmitFuelResponse(response);
				return result;
			}
			catch(Exception e)
			{
				return new SubmitFuelResponse {SyntaxErrorMessage = e.Message, FullResponse = e.ToString(), ErrorMessage = "", SuccessMessage = ""};
			}
		}

		public string TestFactory(string factory)
		{
			return Post(submitFuelFake, Escape(factory), "G1");
		}

		private static string Escape(string factory)
		{
			return factory.Replace("\r\n", "%0D%0A").Replace("#", "%23").Replace(",", "%2C").Replace(":", "%3A");
		}

		private string Post(string requestUri, string data)
		{
			return Post(requestUri, data, "contents");
		}

		private string Post(string requestUri, string data, string paramName)
		{
			var req = CreateWebRequest(requestUri);
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			using(var s = req.GetRequestStream())
			{
				var buffer = Encoding.ASCII.GetBytes(paramName + "=" + data);
				s.Write(buffer, 0, buffer.Length);
			}
			using(var resp = req.GetResponse())
				return GetContent(resp.GetResponseStream());
		}

		private static string Login()
		{
			var s1 = HttpPost("http://icfpcontest.org/icfp10/static/j_spring_security_check", "j_username=hack_the_loop&j_password=722750249482275797203643486818027156264822953884882105712868");
			var pos1 = s1.IndexOf(";jsessionid=") + ";jsessionid=".Length;
			var pos2 = s1.IndexOf("\"", pos1);
			var sessionId = s1.Substring(pos1, pos2 - pos1);
			return sessionId;
		}

		private string GetResponse(string requestUri)
		{
			var req = CreateWebRequest(requestUri);
			using(var resp = req.GetResponse())
				return GetContent(resp.GetResponseStream());
		}

		private WebRequest CreateWebRequest(string requestUri)
		{
			var req = WebRequest.Create(requestUri);
			req.Headers.Add("Cookie", string.Format("JSESSIONID={0}", sessionId));
			return req;
		}

		private static string GetContent(Stream stream)
		{
			using(var sr = new StreamReader(stream, Encoding.UTF8))
				return sr.ReadToEnd();
		}

		private readonly string sessionId;
		private const string root = @"http://icfpcontest.org/icfp10";
		private const string login = @"http://icfpcontest.org/icfp10/login";
		private const string getCarsList = @"http://icfpcontest.org/icfp10/score/instanceTeamCount";
		private const string getPagedCarsList = @"http://icfpcontest.org/icfp10/score/instanceTeamCount?page={0}&size=10";
		private const string submitCar = @"http://icfpcontest.org/icfp10/instance/form";
		private const string getCar = @"http://icfpcontest.org/icfp10/instance/{0}/solve/form";
		private const string submitFuel = @"http://icfpcontest.org/icfp10/instance/{0}/solve";
		private const string submitFuelFake = @"http://nfa.imn.htwk-leipzig.de/icfpcont/?G0=0";
	}
}