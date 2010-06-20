using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace WebClient
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
			string sessionId = args[0];
			var c = new WebClient(sessionId);
			MainImpl(c);
		}

		private static void MainImpl(WebClient client)
		{
			//var error = client.SubmitFuel("2416", fuel);
			//Console.WriteLine(error);
			
			//GetCars(client);

			SubmitFuelForEachCar(client, fuel);
		}

		private static void SubmitFuelForEachCar(WebClient client, string factory)
		{
			var cars = client.GetCarIdsList();
			var sb = new StringBuilder();
			foreach (var car in cars.OrderBy(c => int.Parse(c)))
			{
				var response = client.SubmitFuel(car, factory);
				sb.AppendLine(car);
				sb.AppendLine(response.ToString());
				sb.AppendLine("====");
				sb.AppendLine();
			}
			File.WriteAllText(string.Format("bf-fuel-submit.txt"), sb.ToString());
		}

		private static void GetCars(WebClient c)
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
			File.WriteAllText(string.Format("cars-{0}.txt", DateTime.Now.ToString("yyyy-MM-dd-hh-mm")), sb.ToString());
		}

		private const string success = "Good! The car can use this fuel.";
		private const string errorAlreadySubmited = "You have already submitted this solution.";

		private const string fuel = @"35L:
0L2R0#0L1R,
1R0R0#2R1L,
3L1L0#35R0R,
4L3R0#2L3R,
5L4R0#3L4R,
7L5R0#4L5R,
6L7R0#6L7R,
9L6R0#5L6R,
8L9R0#8L9R,
12L8R0#7L8R,
10L12R0#10L11R,
11R10R0#12R11L,
13L11L0#9L10R,
14L13R0#12L13R,
16L14R0#13L14R,
15L16R0#15L16R,
19L15R0#14L15R,
17L19R0#17L18R,
18R17R0#19R18L,
21L18L0#16L17R,
20L21R0#20L21R,
22L20R0#19L20R,
24L22R0#21L22R,
23L24R0#23L24R,
25L23R0#22L23R,
26L25R0#24L25R,
28L26R0#25L26R,
27L28R0#27L28R,
30L27R0#26L27R,
29L30R0#29L30R,
31L29R0#28L29R,
33L31R0#30L31R,
32L33R0#32L33R,
34L32R0#31L32R,
35R34R0#33L34R,
X2L0#X34L:
35L";
	}
}
