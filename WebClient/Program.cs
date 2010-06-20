using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using CircuitCalc;
using CircuitCalc.CircuitBuilding;
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

			var factory = factorySample;
			if (args.Length > 1)
				factory = GetFactory(args[1]);

			//Console.WriteLine(factory);
			//client.SubmitFuel("10220", factory);

			SubmitFuelForEachCar(client, factory);
		}

		private static string GetFactory(string fuel)
		{
			var bytes = new Builder(Consts.serverInput, Consts.keyPrefix + fuel).Build();
			var factory = new CircuitSerializer().Serialize(bytes);
			return factory;
		}

		private static void SubmitFuelForEachCar(IcfpcWebClient client, string factory)
		{
			var dir = string.Format("bf-{0}", DateTime.Now.ToString("yyyy-MM-dd-HH-mm"));
			Directory.CreateDirectory(dir);
			Directory.SetCurrentDirectory(dir);
			var cars = client.GetCarIdsList();
			var sb = new StringBuilder();
			var successes = 0;
			foreach (var car in cars.OrderBy(c => int.Parse(c)))
			{
				var response = client.SubmitFuel(car, factory);
				sb.AppendLine(car);
				sb.AppendLine("====");
				sb.AppendLine(response.ToString());
				sb.AppendLine("====");
				sb.AppendLine();
				if (!string.IsNullOrEmpty(response.SuccessMessage)) successes++;
			}
			sb.AppendLine(string.Format("Number of successful submit: {0}", successes));
			File.WriteAllText(string.Format("bf-fuel-submit.txt"), sb.ToString());
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
			File.WriteAllText(string.Format("cars-{0}.txt", DateTime.Now.ToString("yyyy-MM-dd-HH-mm")), sb.ToString());
		}

		private const string success = "Good! The car can use this fuel.";
		private const string errorAlreadySubmited = "You have already submitted this solution.";

		private const string factorySample = @"99L:
0L2R0#0L1R,
1R0R0#2R1L,
3L1L0#99R0R,
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
29L27R0#26L27R,
30L29R0#28L29R,
33L30R0#29L30R,
31L33R0#31L32R,
32R31R0#33R32L,
34L32L0#30L31R,
36L34R0#33L34R,
35L36R0#35L36R,
38L35R0#34L35R,
37L38R0#37L38R,
41L37R0#36L37R,
39L41R0#39L40R,
40R39R0#41R40L,
44L40L0#38L39R,
42L44R0#42L43R,
43R42R0#44R43L,
47L43L0#41L42R,
45L47R0#45L46R,
46R45R0#47R46L,
48L46L0#44L45R,
50L48R0#47L48R,
49L50R0#49L50R,
53L49R0#48L49R,
51L53R0#51L52R,
52R51R0#53R52L,
56L52L0#50L51R,
54L56R0#54L55R,
55R54R0#56R55L,
58L55L0#53L54R,
57L58R0#57L58R,
59L57R0#56L57R,
62L59R0#58L59R,
60L62R0#60L61R,
61R60R0#62R61L,
63L61L0#59L60R,
66L63R0#62L63R,
64L66R0#64L65R,
65R64R0#66R65L,
67L65L0#63L64R,
70L67R0#66L67R,
68L70R0#68L69R,
69R68R0#70R69L,
73L69L0#67L68R,
71L73R0#71L72R,
72R71R0#73R72L,
76L72L0#70L71R,
74L76R0#74L75R,
75R74R0#76R75L,
79L75L0#73L74R,
77L79R0#77L78R,
78R77R0#79R78L,
81L78L0#76L77R,
80L81R0#80L81R,
82L80R0#79L80R,
83L82R0#81L82R,
85L83R0#82L83R,
84L85R0#84L85R,
88L84R0#83L84R,
86L88R0#86L87R,
87R86R0#88R87L,
91L87L0#85L86R,
89L91R0#89L90R,
90R89R0#91R90L,
92L90L0#88L89R,
94L92R0#91L92R,
93L94R0#93L94R,
97L93R0#92L93R,
95L97R0#95L96R,
96R95R0#97R96L,
98L96L0#94L95R,
99R98R0#97L98R,
X2L0#X98L:
99L
";
	}
}
