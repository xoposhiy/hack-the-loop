using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CircuitCalc.CircuitBuilding;
using CircuitCalc.FuelValidation;
using CircuitCalc.TParsing;
using CircuitCalc.WebClient;
using NUnit.Framework;

namespace CircuitCalc.CarsSolving
{
	[TestFixture]
	public class SolveCars_Test
	{
		CarsRepo repo = new CarsRepo("../../../Cars.txt");
		Validator v = new Validator();
		private TEncoder encoder = new TEncoder();
		IcfpcWebClient c = new IcfpcWebClient("4AB889070AB4FB8F4CD6DFE08B66C9FD");

		[Test]
		public void Solve()
		{
			var carsIds = repo.carsByTanksCount[4];
			foreach(var carId in carsIds)
			{
				var car = repo.cars[carId];
				Console.WriteLine(carId);
				BruteForce(carId, car);
			}
		}

		private void BruteForce(string carId, Chamber[] car)
		{
			var maxF = 7;
			for(int f0=1; f0<maxF; f0++)
			for(int f1=1; f1<maxF; f1++)
				for(int f2 = 1; f2 < maxF; f2++)
					for(int f3 = 1; f3 < maxF; f3++)
					{
				var encodedFuel = encoder.EncodeSimpleFuel(f0, f1, f2, f3);
				if (v.FuelFitsCar(car, encodedFuel))
				{
					Console.WriteLine("car {0} solved!", carId);
					Console.WriteLine("{0} {1} {2} {3}", f0, f1, f2, f3);
					Console.WriteLine("encoded: " + encodedFuel);
					Console.WriteLine("sending...");
					var factory = Builder.BuildFactory(encodedFuel);
					var error = c.SubmitFuel(carId, factory);
					Console.WriteLine(error);
					return;
				}
			}
		}
	}
}
