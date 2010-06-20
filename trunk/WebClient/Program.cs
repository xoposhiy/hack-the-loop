using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using CircuitCalc;
using CircuitCalc.CircuitBuilding;
using CircuitCalc.FuelValidation;
using CircuitCalc.WebClient;

namespace WebClientNS
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Need session ID!");
				return;
			}
			var watch = Stopwatch.StartNew();
			string sessionId = args[0];
			var c = new IcfpcWebClient(sessionId);
			MainImpl(c, args);
			watch.Stop();
			Console.WriteLine("Elapsed time: {0}", watch.FormatElapsedTime());
		}


		private static void MainImpl(IcfpcWebClient client, string[] args)
		{
			//var error = client.SubmitFuel("2416", fuel);
			//Console.WriteLine(error);

			//GetCars(client);

			var fuel = fuelSample;
			if (args.Length > 1)
				fuel = args[1];

			//Console.WriteLine(factory);
			//client.SubmitFuel("10220", factory);

			var validator = new Validator();
			var carIds = new CarsRepo("cars.txt").encodedCars.Where(kvp => validator.FuelFitsCar(kvp.Value, fuel)).Select(kvp => kvp.Key);

			SubmitFuelForEachCar(client, fuel, carIds);
		}

		private static void SubmitFuelForEachCar(IcfpcWebClient client, string fuel, IEnumerable<string> carIds)
		{
			var dir = string.Format("bf-{0}", DateTime.Now.ToString("yyyy-MM-dd-HH-mm"));
			Directory.CreateDirectory(dir);
			Directory.SetCurrentDirectory(dir);
			File.WriteAllText("h0.txt", h0);
			File.WriteAllText("h1.txt", h1);
			File.WriteAllText("h2.txt", h2);
			var successes = 0;
			var logFilename = string.Format("bf-fuel-submit.log");
			using (var sw = new StreamWriter(logFilename, true))
			{
				sw.WriteLine(string.Format("Submiting fuel: {0}", fuel));
				sw.WriteLine();
				var factory = Builder.BuildFactory(fuel);
				sw.WriteLine(string.Format("Its factory: {0}", factory));
				sw.WriteLine();
				foreach (var carId in carIds.OrderBy(c => int.Parse(c)))
				{
					var response = client.SubmitFuel(carId, factory);
					sw.WriteLine(carId);
					sw.WriteLine("====");
					sw.WriteLine(response.ToString());
					sw.WriteLine("====");
					sw.WriteLine();
					if (!string.IsNullOrEmpty(response.SuccessMessage)) successes++;
				}
				sw.WriteLine(string.Format("Number of successful submits: {0}", successes));
			}
		}

		private static void GetCars(IcfpcWebClient c)
		{
			var cars = c.GetCarsList();
			PrintCars(cars);
		}

		private static void PrintCars(Dictionary<string, string> cars)
		{
			var sb = new StringBuilder();
			foreach (var car in cars.OrderBy(kvp => int.Parse(kvp.Key)))
			{
				sb.AppendLine(car.Key);
				sb.AppendLine(car.Value);
			}
			File.WriteAllText(string.Format("Cars.txt", DateTime.Now.ToString("yyyy-MM-dd-HH-mm")), sb.ToString());
		}

		private const string success = "Good! The car can use this fuel.";
		private const string errorAlreadySubmited = "You have already submitted this solution.";

		private const string fuelSample = @"2210221022102200000221002200002210002200022102210120022100120221000122210221011002210011022100011";

		private const string h0 = @"0L:
X0R0#X0R:
0L";
		private const string h1 = @"1L:
0L1R0#0L1R,
X0R0#X0R:
1L";
		private const string h2 = @"2L:
0L2R0#0L1R,
1R0R0#2R1L,
X1L0#X0R:
2L";
	}
}
