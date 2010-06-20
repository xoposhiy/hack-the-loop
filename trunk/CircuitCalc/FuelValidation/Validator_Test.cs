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
			foreach(var carId in repo.carsByTanksCount[2])
			{
				var car = repo.cars[carId];
				try
				{
					if(v.FuelFitsCar(car, "2202202201010220101022022010102201010"))
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

		[Test]
		public void TestFuelOnAllCars()
		{
			int c = 0;
			foreach (var carId in repo.encodedCars.Keys)
			{
				var car = repo.cars[carId];
				try
				{
					if (v.FuelFitsCar(car, "2210221022102200000221002200002210002200022102210120022100120221000122210221011002210011022100011"))
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
