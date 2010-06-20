using System;
using System.Text;
using NUnit.Framework;

namespace CircuitCalc.FuelValidation
{
	[TestFixture]
	public class Validator_Test
	{
		Validator v = new Validator();
		CarsRepo repo = new CarsRepo("../../../Cars.txt");

		[Test]
		public void ShowCarsByTanksCount()
		{
			for(int i = 1; i <= 6; i++)
				Console.WriteLine(i + " " + repo.carsByTanksCount[i].Count);
		}

		[Test]
		public void TestSimpleFuel()
		{
			int c = 0;
			foreach(var carId in repo.carsByTanksCount[1])
			{
				var car = repo.cars[carId];
				try
				{
					if(v.FuelFitsCar(car, "11111"))
					{
						c++;
						Console.WriteLine(carId);
//						car.Print();
					}
				}
				catch
				{
					Console.WriteLine(carId);
					Console.WriteLine(repo.encodedCars[carId]);
					car.Print();
					throw;
				}
			}
			Console.WriteLine("TOTAL: " + c);
		}

	}
}
