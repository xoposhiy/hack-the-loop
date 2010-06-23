using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CircuitCalc;
using CircuitCalc.AkCalc;
using CircuitCalc.CircuitBuilding;
using CircuitCalc.WebClient;
using NUnit.Framework;
using Submiter;
using System.Linq;
using Calculator = CircuitCalc.PeCalc.Calculator;

namespace WebClient
{
	[TestFixture]
	public class SendSpywork_Test
	{
		IcfpcWebClient c = new CircuitCalc.WebClient.IcfpcWebClient("88BE9F55CB16C44769FE7E99D90F9821");
		SolutionsRepo sols = new SolutionsRepo("../../../Data/SolvedCars.txt");

		[Test]
		public void GetCar()
		{
			var car = c.GetCar("0");
		}

		[Test]
		public void GetCars()
		{
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
		public void Resend222222()
		{
			var ss = sols.solutions.ToArray().Where(s => s.Value == "2222000111111111111111111111111").Reverse().ToArray();
			Console.WriteLine(ss.Count());
			int count = 0;
			foreach (var solution in ss)
			{
				try
				{
					Console.WriteLine("{0} {1}", ++count, c.SubmitFuel(solution.Key, Builder.BuildFactory("2222000111111121111111211111111")));
				}catch(Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}

		[Test]
		public void FromMonkeys()
		{
			var files = Directory.GetFiles(@"c:\work\cars\others\codingmonkeys-2010\src\data\submit_fuel\fuels");
			foreach (var carFile in files)
			{
				var carId = Path.GetFileName(carFile);
				if (sols.solutions.ContainsKey(carId)) continue;
				int tmp;
				if (!int.TryParse(carId, out tmp)) continue;
				Console.WriteLine(carId);
				var fuel = File.ReadAllText(carFile).Trim().TrimEnd('.');
				if (fuel.Length > 150 || fuel.Length <= 6)
				{
					Console.WriteLine("fuel with len {1} for car {0} skipped", carId, fuel.Length);
					continue;
				}
				var submitFuelResponse = c.SubmitFuel(carId, Builder.BuildFactory(fuel));
				if (submitFuelResponse.SuccessMessage.Contains("Good!"))
				{
					//sols.AddSolution(carId, fuel);
					Console.WriteLine("ok!");
				}
				else
				{
					if (!submitFuelResponse.ErrorMessage.Contains("already submitted")) 
						Console.WriteLine(submitFuelResponse);
					else
					{
						//sols.AddSolution(carId, fuel);
					}
				}

			}
		}

		[Test]
		public void FromTBD()
		{
			IDictionary<string, string> spywork = Load(@"c:\work\cars\others\icfpc2010-tbd\simulator\data\submitted_solutions.txt ");
			var filtered = spywork.Where(w => !sols.solutions.ContainsKey(w.Key));
			Console.WriteLine("count: " + filtered.Count());
			foreach(var w in filtered.Reverse())
			{
				var carId = w.Key;
				var factory = w.Value;
				var submitFuelResponse = c.SubmitFuel(carId, factory);
				if(!submitFuelResponse.FullResponse.Contains("already submitted"))
				{
					Console.Write(carId + " ");
					if (submitFuelResponse.SuccessMessage.Contains("Good!"))
					{
						//sols.AddSolution(carId, "unknown");
						Console.WriteLine("ok!");
					}
					else
					{
						Console.WriteLine();
						Console.WriteLine(submitFuelResponse);
					}
				}
				else
				{
					//sols.AddSolution(carId, "unknown");
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