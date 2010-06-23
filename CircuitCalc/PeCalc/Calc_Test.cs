using System;
using System.IO;
using NUnit.Framework;

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
			var stepsSkipped = (servInp.Length - 17) / 2;
			Console.WriteLine("skip {0} steps", stepsSkipped);
			for (int i = stepsSkipped; i < 300; i++)
			{
				var factories = new string[3, 3];
				bool[,] responses = FillTable(i, servInp, factories);
				int x2 = -1, y2 = -1;
				for (int x = 0; x < 3; x++)
					for (int y = 0; y < 3; y++)
					{
						if (responses[x, y])
							x2 = x;
					}
				if (x2 < 0) throw new Exception();
				for (int y = 0; y < 3; y++)
					if (!responses[x2, y]) y2 = y;
				var f = factories[x2, y2];
				servInp += FindServInputSuffix(servInp, f);
				Console.WriteLine("EXTENDED INPUT: " + servInp);
			}
		}

		private string FindServInputSuffix(string servInp, string f)
		{
			for (char c1 = '0'; c1 <= '2'; c1++)
			{
				for (char c2 = '0'; c2 <= '2'; c2++)
				{
					var o = Calculator.For(f).PushString(servInp + c1 + c2);
					if (o.EndsWith("22")) return "" + c1 + c2;
				}
			}
			throw new Exception();
		}

		private bool[,] FillTable(int i, string servInp, string[,] factories)
		{
			var responses = new bool[3, 3];
			for (char c1 = '0'; c1 <= '2'; c1++)
			{
				for (char c2 = '0'; c2 <= '2'; c2++)
				{
					var factory = Builder.Build(servInp + "00", Consts.keyPrefix + new string('2', 2 * i) + c1 + c2);
					responses[c1 - '0', c2 - '0'] = Send2(factory, i * 2 + 2);
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

namespace CircuitCalc.PeCalc
{

	
	
	[TestFixture]
	public class Calc_Test
	{
		[Test]
		public void ExtendServerInput1()
		{
			new ServerInputFinder("D1B8399DD701D0A8FFAA32345AE455B0").Run();
		}

		[Test]
		public void SubmitFuel1()
		{
			var lines = File.ReadAllLines(@"c:\work\icfpc2010\others\tbd\simulator\data\car_data_sorted");
			foreach(var line in lines)
			{
				var parts = line.Split(',');
				if(parts.Length > 1 && parts[1].Trim().StartsWith("1"))
				{
					Console.WriteLine(parts[0].Trim());
				}
			}
		}

		[Test]
		public void T()
		{
			Console.WriteLine(new Calculator("tmp.txt")
				.PushString(Consts.serverInput));
		}

		[Test]
		public void TestCase()
		{
			var calculator = new Calculator("sample.txt");
			const string sampleIn = "02222220210110011";
			const string realIn = "01202101210201202";
			const string sampleRealOut = "10221220002011011";
			string myOut = calculator.PushString(sampleIn);
			Console.WriteLine();
			//Console.WriteLine(sampleRealOut);
			Console.WriteLine(myOut);
		}

		[Test]
		public void TestXOR()
		{
			var input = "01202101210201202";
			//Console.WriteLine(new Calculator("simple.txt").PushString(input));
			/*            Console.WriteLine(new Calculator(new []
                                                 {
                                                     "0R:",
                                                     "2LX0#1LX,",
                                                     "0L2R0#2L2R,",
                                                     "1L1R0#0L1R:",
                                                     "0R"
                                                 }).PushString(input));*/

			/*            Console.WriteLine(new Calculator(new []
                                                             {
                                                                 "0R:2LX0#1LX,0L2R0#2L2R,1L1R0#0L1R:0R"
                                                             }).PushString(input));*/


			/*            Console.WriteLine(new Calculator(new []
                                                 {
                                                    "19L:",
                                                    "12R13R0#1R12R,",
                                                    "14R0L0#4R9L,",
                                                    "9R10R0#3L8L,",
                                                    "2L17R0#5L9R,",
                                                    "15R1L0#10R13R,",
                                                    "3L18R0#6L15L,",
                                                    "5L11R0#13L12L,",
                                                    "19R16R0#11R8R,",
                                                    "2R7R0#11L10L,",
                                                    "1R3R0#18L2L,",
                                                    "8R4L0#16L2R,",
                                                    "8L7L0#15R6R,",
                                                    "6R0R0#14L0L,",
                                                    "6L4R0#14R0R,",
                                                    "12L13L0#17L1L,",
                                                    "5R11L0#16R4L,",
                                                    "10L15L0#17R7R,",
                                                    "14L16L0#18R3R,",
                                                    "9L17L0#19R5R,",
                                                    "X18L0#X7L:",
                                                    "19L"
                                                 }).PushString(input));*/

			/*            Console.WriteLine(new Calculator(new[]
                                                 {
                                                     "2R:",
                                                     "2L1R0#1L1R,",
                                                     "0L0R0#2L0R,",
                                                     "1LX0#0LX:",
                                                     "2R"
                                                 }).PushString(input));*/
			/*++
            Console.WriteLine(new Calculator(new[]
                                                 {
                                                     "0L:",
                                                     "X0R0#1R0R,",
                                                     "10L0X#1LX:",
                                                     "1R"
                                                 }).PushString(input));
            --*/
		}

		[Test]
		public void Debug()
		{
			var calculator = new Calculator("tmp.txt");
			Console.WriteLine(calculator.PushString("22022022022022022"));
		}

		[Test]
		public void Search()
		{
			const string input = "01202101210201202";
			//const string input = "11021210112101221";
			const string target = "11021210112101221";
			var files = new string[]
				{
					"generated.txt",
					//                                        @"D:\fact4.txt",
					@"D:\fact5.txt"
				};
			string s;
			string collect = "";
			bool any = false;
			for(int i = 0; i < files.Length; i++)
			{
				Console.WriteLine("Parsing " + files[i]);
				var sr = new StreamReader(files[i]);
				while((s = sr.ReadLine()) != null)
				{
					if(s.Length > 0)
					{
						collect += s + "\n";
					}
					else
					{
						collect = collect.Substring(0, collect.Length - 1);
						string res = new Calculator(collect.Split('\n')).PushString(input);
						if(res == target)
						{
							any = true;
							Console.WriteLine(collect);
							Console.WriteLine("-----------------------------------");
						}
						collect = "";
					}
				}
			}
			if(!any)
			{
				Console.WriteLine("No factories");
			}
		}
	}
}