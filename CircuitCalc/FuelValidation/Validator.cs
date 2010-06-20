using System;
using System.Linq;
using CircuitCalc.TParsing;

namespace CircuitCalc.FuelValidation
{
	public class Validator
	{
		private readonly TParser parser = new TParser();

		public bool FuelFitsCar(string encodedCar, string encodedFuel)
		{
			var car = parser.ParseChambers(new TStream(encodedCar));
			return FuelFitsCar(car, encodedFuel);
		}

		public bool FuelFitsCar(Chamber[] car, string encodedFuel)
		{
			var fuel = parser.ParseFuel(new TStream(encodedFuel));
			return car.All(chamber => ChamberWorks(chamber, fuel));
		}

		private bool ChamberWorks(Chamber chamber, Matrix[] fuel)
		{
			Matrix upperOutput = CalculatePipe(chamber.upper, fuel);
			Matrix lowerOutput = CalculatePipe(chamber.lower, fuel);
			for(int y = 0; y < upperOutput.height; y++)
			{
				for(int x = 0; x < upperOutput.width; x++)
				{
					if(upperOutput.items[y][x] < lowerOutput.items[y][x])
						return false;
				}
				//TODO Main chamber!
			}
			return true;
		}

		private Matrix CalculatePipe(int[] upper, Matrix[] fuel)
		{
			return upper
				.Aggregate(
					MakeOne(fuel[0].height),
					(m, fuelIndex) => Mult(m, fuel[fuelIndex]));
		}

		private static Matrix MakeOne(int size)
		{
			var matrix = new Matrix(size, size);
			for(int i = 0; i < size; i++)
				matrix.items[i][i] = 1;
			return matrix;
		}

		private Matrix Mult(Matrix a, Matrix b)
		{
			var c = new Matrix(a.height, b.width);
			for(int y = 0; y < c.height; y++)
				for(int x = 0; x < c.width; x++)
					for(int k = 0; k < a.width; k++)
						c.items[y][x] += a.items[y][k]*b.items[k][x];
			return c;
		}
	}
}