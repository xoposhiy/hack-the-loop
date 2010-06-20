using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CircuitCalc.CircuitBuilding;
using CircuitCalc.FuelValidation;
using CircuitCalc.Input;
using CircuitCalc.TParsing;
using CircuitCalc.WebClient;
using NUnit.Framework;
using CircuitCalc;

namespace CircuitCalc.CarsSolving
{
	[TestFixture]
	public class SolveCars_Test
	{
		CarsRepo repo = new CarsRepo("../../../Cars.txt");
		Validator v = new Validator();
		private TEncoder encoder = new TEncoder();
		IcfpcWebClient c = new IcfpcWebClient("FE686CA522D1A6F6D9AA60EEAF5743AD");

		[Test]
		public void ShowCar()
		{
			new TParser().ParseCar("22102200010221100101022111111111110222200011110011112222000111110100002222002001111001111").Print();
			//repo.cars[""].Print();
		}

		[Test]
		public void Solve()
		{
			int solved = 0;
			for(int tanksCount = 2; tanksCount <= 6; tanksCount++)
			{
				int carIndex = 0;
				var carsIds = repo.carsByTanksCount[tanksCount];
				foreach(var carId in carsIds)
				{
					var car = repo.cars[carId];
					BruteForce(carId, car, tanksCount);
					Console.WriteLine(carIndex++);
				}
				Console.WriteLine("SOLVED: " + solved);
			}
		}

		private void BruteForce(string carId, Chamber[] car, int tanksCount)
		{
			var fuel = new Stack<int>();
			DoBruteforce(carId, car, fuel, tanksCount);
		}

		private int maxF = 4;

		private bool DoBruteforce(string carId, Chamber[] car, Stack<int> fuel, int tanksCount)
		{
			for(int f=1; f<maxF; f++)
			{
				fuel.Push(f);
				if (tanksCount == 1)
				{
					if(CheckFuel(fuel, car, carId)) return true;
				}
				else
				{
					if (DoBruteforce(carId, car, fuel, tanksCount - 1)) return true;
				}
				fuel.Pop();
			}
			return false;
		}

		private int solved = 0;
		private bool CheckFuel(Stack<int> fuel, Chamber[] car, string carId)
		{
			var ms = fuel.Select(f => Matrix.Simple(f)).ToArray();
/*
			foreach(var f in fuel)
			{
				Console.Write(f + " ");
			}
			Console.WriteLine();
*/
			if(v.FuelFitsCar(car, ms))
			{
				var encodedFuel = new TEncoder().EncodeFuel(ms);
				var factory = Builder.BuildFactory(encodedFuel);
				Console.WriteLine("sending...");
				var error = c.SubmitFuel(carId, factory);
				if(error.SuccessMessage != "")
				{
					ms.Print();
					Console.WriteLine("car {0} solved!", carId);
					Console.WriteLine("encoded: " + encodedFuel);
					solved++;
				}
				return true;
				//else Console.WriteLine(error);
			}
			return false;
		}
	}
}
