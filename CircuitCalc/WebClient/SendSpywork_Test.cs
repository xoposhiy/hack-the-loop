using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using Submiter;
using System.Linq;

namespace WebClient
{
	[TestFixture]
	public class SendSpywork_Test
	{

		[Test]
		public void GetCars()
		{
			var c = new CircuitCalc.WebClient.IcfpcWebClient("FE686CA522D1A6F6D9AA60EEAF5743AD");
			Console.WriteLine(c.GetCar("219"));
			var cars = c.GetCarsList();
			var sb = new StringBuilder();
			foreach(var car in cars)
			{
				sb.AppendLine(car.Key);
				sb.AppendLine(car.Value);
			}
			File.WriteAllText("../../../Cars2.txt", sb.ToString());
		}

		[Test]
		public void TestCase()
		{
			var sols = new SolutionsRepo("../../../Data/SolvedCars.txt");
			IDictionary<string, string> spywork = Load("../../../submitted_solutions.txt");
			var client = new CircuitCalc.WebClient.IcfpcWebClient("AD07E392CFA0523F9F58A44FC19121FC");
			var filtered = spywork.Where(w => !sols.solutions.ContainsKey(w.Key));
			Console.WriteLine("count: " + filtered.Count());
			foreach(var w in filtered)
			{
				var submitFuelResponse = client.SubmitFuel(w.Key, w.Value);
				if(!submitFuelResponse.FullResponse.Contains("already submitted"))
				{
					Console.Write(w.Key + " ");
					if (submitFuelResponse.SuccessMessage.Contains("Good!")) Console.WriteLine("ok!");
					else
					{
						Console.WriteLine();
						Console.WriteLine(submitFuelResponse);
					}
				}
			}
		}

		private IDictionary<string, string> Load(string spywork)
		{
			var res = new Dictionary<string, string>();
			var lines = File.ReadAllLines(spywork);
			foreach(var line in lines)
			{
				if (line.Trim().StartsWith("#")) continue;
				var delIndex = line.IndexOf(':');
				if (delIndex < 0) continue;
				var id = line.Substring(0, delIndex).Trim();
				var quotedFactory = line.Substring(delIndex + 1).Trim();
				var escapedFactory = quotedFactory.Trim().TrimStart('\'').TrimEnd('\'',',');
				var factory = escapedFactory.Replace("\\n", "\r\n");
				res.Add(id, factory);
			}
			return res;
		}
	}
}