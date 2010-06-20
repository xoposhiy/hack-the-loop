using System.Collections.Generic;
using System.IO;
using System.Linq;
using CircuitCalc.TParsing;

namespace CircuitCalc
{
	public class CarsRepo
	{
		public readonly IDictionary<string, Chamber[]> cars = new Dictionary<string, Chamber[]>();
		public readonly IDictionary<string, string> encodedCars = new Dictionary<string, string>();
		public readonly List<string>[] carsByTanksCount = new List<string>[7];
		private readonly TParser parser = new TParser();

		public CarsRepo(string file)
		{
			var lines = File.ReadAllLines(file);
			for(int i=0; i<lines.Length; i+=2)
			{
				var id = lines[i];
				var encodedCar = lines[i + 1];
				cars.Add(id, parser.ParseCar(encodedCar));
				encodedCars.Add(id, encodedCar);
			}
			for(int tanksCount = 1; tanksCount <= 6; tanksCount++)
			{
				carsByTanksCount[tanksCount] = new List<string>();
				foreach(var car in cars)
				{
					var chambers = car.Value;
					var lastTankIndex = tanksCount - 1;
					var isSmall = chambers.All(ch => ch.AllTanks().All(i => i <= lastTankIndex) && ch.AllTanks().Any(i => i == lastTankIndex));
					if(isSmall)
						carsByTanksCount[tanksCount].Add(car.Key);
				}
			}
		}
	}
}