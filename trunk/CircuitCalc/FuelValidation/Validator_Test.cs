﻿using System;
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
			for (int i = 1; i <= 6; i++)
				Console.WriteLine(i + " " + repo.carsByTanksCount[i].Count);
		}

		[Test]
		public void TestSimpleFuel()
		{
			int c = 0;
			foreach (var carId in repo.carsByTanksCount[2])
			{
				var car = repo.cars[carId];
				try
				{
					if (v.FuelFitsCar(car, "2202202201010220101022022010102201010"))
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
			var fuels = new[]
			            	{
			            		"110",//(1)
			            		"220220220100220010220220100220010",//(((1,0)(0,1))((1,0)(0,1)))
			            		"2210221022101000221001002210001022102210100022100100221000102210221010002210010022100010",//(((1,0,0)(0,1,0)(0,0,1))((1,0,0)(0,1,0)(0,0,1))((1,0,0)(0,1,0)(0,0,1)))
			            		"22112211221110000221101000221100100221100010221122111000022110100022110010022110001022112211100002211010002211001002211000102211221110000221101000221100100221100010",//(((1,0,0,0)(0,1,0,0)(0,0,1,0)(0,0,0,1))((1,0,0,0)(0,1,0,0)(0,0,1,0)(0,0,0,1))((1,0,0,0)(0,1,0,0)(0,0,1,0)(0,0,0,1))((1,0,0,0)(0,1,0,0)(0,0,1,0)(0,0,0,1)))
			            		"2212221222121000002212010000221200100022120001002212000010221222121000002212010000221200100022120001002212000010221222121000002212010000221200100022120001002212000010221222121000002212010000221200100022120001002212000010221222121000002212010000221200100022120001002212000010",//(((1,0,0,0,0)(0,1,0,0,0)(0,0,1,0,0)(0,0,0,1,0)(0,0,0,0,1))((1,0,0,0,0)(0,1,0,0,0)(0,0,1,0,0)(0,0,0,1,0)(0,0,0,0,1))((1,0,0,0,0)(0,1,0,0,0)(0,0,1,0,0)(0,0,0,1,0)(0,0,0,0,1))((1,0,0,0,0)(0,1,0,0,0)(0,0,1,0,0)(0,0,0,1,0)(0,0,0,0,1))((1,0,0,0,0)(0,1,0,0,0)(0,0,1,0,0)(0,0,0,1,0)(0,0,0,0,1)))
			            		"2222000222200022220001000000222200001000002222000001000022220000001000222200000001002222000000001022220002222000100000022220000100000222200000100002222000000100022220000000100222200000000102222000222200010000002222000010000022220000010000222200000010002222000000010022220000000010222200022220001000000222200001000002222000001000022220000001000222200000001002222000000001022220002222000100000022220000100000222200000100002222000000100022220000000100222200000000102222000222200010000002222000010000022220000010000222200000010002222000000010022220000000010",//(((1,0,0,0,0,0)(0,1,0,0,0,0)(0,0,1,0,0,0)(0,0,0,1,0,0)(0,0,0,0,1,0)(0,0,0,0,0,1))((1,0,0,0,0,0)(0,1,0,0,0,0)(0,0,1,0,0,0)(0,0,0,1,0,0)(0,0,0,0,1,0)(0,0,0,0,0,1))((1,0,0,0,0,0)(0,1,0,0,0,0)(0,0,1,0,0,0)(0,0,0,1,0,0)(0,0,0,0,1,0)(0,0,0,0,0,1))((1,0,0,0,0,0)(0,1,0,0,0,0)(0,0,1,0,0,0)(0,0,0,1,0,0)(0,0,0,0,1,0)(0,0,0,0,0,1))((1,0,0,0,0,0)(0,1,0,0,0,0)(0,0,1,0,0,0)(0,0,0,1,0,0)(0,0,0,0,1,0)(0,0,0,0,0,1))((1,0,0,0,0,0)(0,1,0,0,0,0)(0,0,1,0,0,0)(0,0,0,1,0,0)(0,0,0,0,1,0)(0,0,0,0,0,1)))
								"2210221022102200000221002200002210002200022102210120022100120221000122210221011002210011022100011",
			            	};
			foreach (var fuel in fuels)
				CheckFuelOnAllCars(fuel);
		}

		private void CheckFuelOnAllCars(string encodedFuel)
		{
			int c = 0;
			foreach (var carId in repo.encodedCars.Keys)
			{
				var car = repo.cars[carId];
				try
				{
					if (v.FuelFitsCar(car, encodedFuel))
					{
						c++;
					}
				}
				catch
				{
					Console.WriteLine(carId);
					Console.WriteLine(encodedFuel);
					Console.WriteLine(repo.encodedCars[carId]);
					car.Print();
					throw;
				}
			}
			Console.WriteLine("TOTAL: " + c);
		}
	}
}