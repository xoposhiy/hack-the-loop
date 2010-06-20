using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

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
			IDictionary<string, string> spywork = Load("../../../spywork");
			var client = new CircuitCalc.WebClient.IcfpcWebClient("9DF6E16DBD869B8D83A8FEC9892F2273");
			foreach(var w in spywork)
			{
				Console.WriteLine(client.SubmitFuel(w.Key, w.Value));
			}
		}

		private IDictionary<string, string> Load(string spywork)
		{
			var res = new Dictionary<string, string>();
			var lines = File.ReadAllLines(spywork);
			foreach(var line in lines)
			{
				var delIndex = line.IndexOf(':');
				var id = line.Substring(0, delIndex).Trim();
				var quotedFactory = line.Substring(delIndex + 1).Trim();
				var escapedFactory = quotedFactory.Substring(1, quotedFactory.Length - 3);
				var factory = escapedFactory.Replace("\\n", "\r\n");
				res.Add(id, factory);
			}
			return res;
		}
	}
}