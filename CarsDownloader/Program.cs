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
					var carIdsList = client.GetCarIdsList(20);
					Console.WriteLine(carIdsList.Count());
					foreach(var carId in carIdsList)
					{
						if(!repo.cars.ContainsKey(carId))
						{
							Console.WriteLine("new car " + carId);
							var car = client.GetCar(carId);
							var tempCarsFile = carsFile + "~";
							if (File.Exists(carsFile))
								File.Delete(tempCarsFile);
							else
							{
								throw new Exception("no cars file?!??!?!?! WTF?");
							}
							File.Copy(carsFile, tempCarsFile);
							File.AppendAllText(tempCarsFile, carId + Environment.NewLine + car + Environment.NewLine);
							TryMove(tempCarsFile);
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

		private void TryMove(string tempCarsFile)
		{
			for(int i = 0; i < 10; i++ )
				try
				{
					if (File.Exists(carsFile))
						File.Delete(carsFile);
					File.Move(tempCarsFile, carsFile);
					return;
				}
				catch(Exception e)
				{
					Console.WriteLine("Cant move file: " + e.Message);
					Thread.Sleep(100);
				}
			throw new Exception("Cant move file :(");
		}
	}
}
