using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CircuitCalc;
using CircuitCalc.WebClient;

namespace CarsDownloader
{
	class Program
	{
		private readonly string carsFile;
		private IcfpcWebClient client;
		private CarsRepo repo;

		private Program(string carsFile, string sessionId)
		{
			this.carsFile = carsFile;
			client = new IcfpcWebClient(sessionId);
		}

		static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("Usage <carsFile> <sessionId>");
				Environment.Exit(1);
			}
			string carsFile = args[0];
			string sessionId = args[1];
			new Program(carsFile, sessionId).Run();
		}

		private void Run()
		{
			while(true)
			{
				try
				{
					Console.Write("Cars in repo: ");
					repo = new CarsRepo(carsFile);
					Console.WriteLine(repo.cars.Count);
					Console.Write("Cars on server: ");
					var carIdsList = client.GetCarIdsList();
					Console.WriteLine(carIdsList.Count());
					foreach(var carId in carIdsList)
					{
						if(!repo.cars.ContainsKey(carId))
						{
							Console.Write("new car " + carId + ": len=");
							var car = client.GetCar(carId);
							File.AppendAllText(carsFile, carId + Environment.NewLine + car + Environment.NewLine);
							Console.WriteLine(car.Length);
						}
					}
				}catch(Exception e)
				{
					Console.WriteLine("..........Oooops!..........");
					Console.WriteLine("\t" + e.Message);
				}
				Console.WriteLine("sleeeeeeep.....");
				Thread.Sleep(10000);
			}
		}
	}
}
