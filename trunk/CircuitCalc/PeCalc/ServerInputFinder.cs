using System;
using System.IO;
using System.Net;
using System.Text;
using CircuitCalc.CircuitBuilding;
using CircuitCalc.WebClient;

namespace CircuitCalc.PeCalc
{
	internal class ServerInputFinder
	{
		private readonly string sessionId;

		public ServerInputFinder(string sessionId)
		{
			this.sessionId = sessionId;
		}

		public void Run()
		{
			var servInp = Consts.serverInput.Substring(0, Consts.serverInput.Length - 2);
			var stepsSkipped = (servInp.Length - 17)/2;
			Console.WriteLine("skip {0} steps", stepsSkipped);
			for(int i = stepsSkipped; i < 300; i++)
			{
				var factories = new string[3,3];
				bool[,] responses = FillTable(i, servInp, factories);
				int x2 = -1, y2 = -1;
				for(int x = 0; x < 3; x++)
					for(int y = 0; y < 3; y++)
					{
						if(responses[x, y])
							x2 = x;
					}
				if(x2 < 0) throw new Exception();
				for(int y = 0; y < 3; y++)
					if(!responses[x2, y]) y2 = y;
				var f = factories[x2, y2];
				servInp += FindServInputSuffix(servInp, f);
				Console.WriteLine("EXTENDED INPUT: " + servInp);
			}
		}

		private string FindServInputSuffix(string servInp, string f)
		{
			for(char c1 = '0'; c1 <= '2'; c1++)
			{
				for(char c2 = '0'; c2 <= '2'; c2++)
				{
					var o = Calculator.For(f).PushString(servInp + c1 + c2);
					if(o.EndsWith("22")) return "" + c1 + c2;
				}
			}
			throw new Exception();
		}

		private bool[,] FillTable(int i, string servInp, string[,] factories)
		{
			var responses = new bool[3,3];
			for(char c1 = '0'; c1 <= '2'; c1++)
			{
				for(char c2 = '0'; c2 <= '2'; c2++)
				{
					var factory = Builder.Build(servInp + "00", Consts.keyPrefix + new string('2', 2*i) + c1 + c2);
					responses[c1 - '0', c2 - '0'] = Send2(factory, i*2 + 2);
					factories[c1 - '0', c2 - '0'] = factory;
					//throw new Exception("dobreak");
				}
			}
			return responses;
		}

		private bool Send(string factory, int need2AtIndex)
		{
			var response = new IcfpcWebClient(sessionId).TestFactory(factory);
			Console.WriteLine(response);
			return 
				response
				.Contains(
					string.Format("\"input\" (line 1, column {0}):", need2AtIndex)
					);

		}

		private bool Send2(string factory, int need2AtIndex)
		{
			var webRequest = (HttpWebRequest)WebRequest.Create("http://icfpcontest.org/icfp10/instance/2416/solve");
			webRequest.CookieContainer = new CookieContainer();
			webRequest.CookieContainer.Add(new Cookie("JSESSIONID", sessionId, "/icfp10", "icfpcontest.org"));
			webRequest.Method = "POST";
			webRequest.ContentType = "application/x-www-form-urlencoded";
			var s = webRequest.GetRequestStream();
			//Console.WriteLine(factory);
			var buffer = Encoding.ASCII.GetBytes("contents=" + Escape(factory));
			s.Write(buffer, 0, buffer.Length);
			s.Close();
			var responseStream = webRequest.GetResponse().GetResponseStream();
			string str = new StreamReader(responseStream).ReadToEnd();
			//Console.WriteLine(str);
			responseStream.Close();
			//if (str.Contains("not connected")) Console.WriteLine("NOT CONNECTED!!!");
			//Console.WriteLine(str);
			var contains = str.Contains(string.Format("\"input\" (line 1, column {0}):", need2AtIndex));
			if (contains) Console.WriteLine(".");
			return contains;
		}

		private string Escape(string factory)
		{
			return factory.Replace("\r\n", "%0D%0A").Replace("#", "%23").Replace(",", "%2C").Replace(":", "%3A");
		}
	}
}