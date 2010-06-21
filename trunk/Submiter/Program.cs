using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CircuitCalc;
using CircuitCalc.CircuitBuilding;
using CircuitCalc.FuelValidation;
using CircuitCalc.WebClient;

namespace Submiter
{
	class Program
	{
		private readonly string carsFile;
		private readonly string solvedCarsFile;
		private readonly string fuelsLibDir;
		private IcfpcWebClient client;
		private CarsRepo carsRepo;
		private SolutionsRepo solRepo;

		private Program(string carsFile, string solvedCarsFile, string fuelsLibDir, string sessionId)
		{
			this.carsFile = carsFile;
			this.solvedCarsFile = solvedCarsFile;
			this.fuelsLibDir = fuelsLibDir;
			client = new IcfpcWebClient(sessionId);
		}

		static void Main(string[] args)
		{
			if (args.Length < 4)
			{
				Console.WriteLine("Usage <carsFile> <solvedCarsFile> <fuelsLibDir> <sessionId>");
				Environment.Exit(1);
			}
			new Program(args[0], args[1], args[2], args[3]).Run();
		}

		private void Run()
		{
			while(true)
			{
				try
				{
					DoWork();
				}
				catch(Exception e)
				{
					Console.WriteLine("......ooooopsss!....");
					Console.WriteLine(e);
				}
				Console.WriteLine("sleeeepeeeing....");
				Thread.Sleep(10000);
			}
		}

		private void DoWork()
		{
			Console.Write("Cars in repo: ");
			carsRepo = new CarsRepo(carsFile);
			Console.WriteLine(carsRepo.cars.Count);
			Console.Write("Known solutions: ");
			solRepo = new SolutionsRepo(solvedCarsFile);
			Console.WriteLine(solRepo.solutions.Count);
			var fuels = Directory.GetFiles(fuelsLibDir).Where(file => file.EndsWith(".f")).Select(file => new FuelInfo(file));
			Console.WriteLine("Fuels: " + fuels.Count());
			foreach(var carId in carsRepo.cars.Keys.OrderBy(k => k.Length))
			{
				foreach(var fuelInfo in fuels)
				{
					if(solRepo.solutions.ContainsKey(carId)) break;
					TrySolveWithFuel(carId, fuelInfo);
				}
			}
		}
		
		private void TrySolveWithFuel(string carId, FuelInfo fuelInfo)
		{
			if(fuelInfo.notSolvedCars.Contains(carId)) return;
			var fits = new Validator().FuelFitsCar(carsRepo.cars[carId], fuelInfo.fuel);
			if (!fits)
			{
				fuelInfo.MarkAsNotSolved(carId);
				return;
			}

			var response = client.SubmitFuel(carId, Builder.BuildFactory(fuelInfo.fuel));
			if(!string.IsNullOrEmpty(response.ErrorMessage))
			{
				if (response.ErrorMessage.Contains("submit") || response.ErrorMessage.Contains("Submit") )
				{
					solRepo.AddSolution(carId, fuelInfo.fuel);
				}
				else if (response.FullResponse.Contains("Access is denied"))
				{
					Console.WriteLine("Relogin please! Remote session expired");
					Environment.Exit(0);
				}
				else
				{
					Console.WriteLine("!!!!!!! Unkown error: {0}", response.ErrorMessage);
				}
			}
			else if (!string.IsNullOrEmpty(response.SuccessMessage))
			{
				solRepo.AddSolution(carId, fuelInfo.fuel);
			}
			else if (!string.IsNullOrEmpty(response.SyntaxErrorMessage))
			{
				fuelInfo.MarkAsNotSolved(carId);
			}
			else
			{
				Console.WriteLine("Unrecognized server response... Server is down? Response below:");
				Console.WriteLine(response.FullResponse);
			}
			
		}
	}

	public class FuelInfo
	{
		public FuelInfo(string filename)
		{
			this.filename = filename;
			fuel = File.ReadAllText(filename);
			notSolvedCarsFilename = Path.ChangeExtension(filename, ".notSolvedCars");
			if (!File.Exists(notSolvedCarsFilename))
				File.WriteAllText(notSolvedCarsFilename, "");
			string[] notSolved = File.ReadAllLines(notSolvedCarsFilename);
			notSolvedCars = new HashSet<string>(notSolved);
		}

		public void MarkAsNotSolved(string carId)
		{
			File.AppendAllText(notSolvedCarsFilename, carId + Environment.NewLine);
			notSolvedCars.Add(carId);
			Console.WriteLine("{0} cant be solved by fuel {1}", carId, filename);
		}

		public readonly string fuel;
		public readonly string filename;
		public readonly HashSet<string> notSolvedCars;
		private readonly string notSolvedCarsFilename;
	}
}
