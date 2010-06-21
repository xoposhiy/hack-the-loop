using System;
using System.Collections.Generic;
using System.IO;

namespace Submiter
{
	public class SolutionsRepo
	{
		private readonly string solvedCarsFile;
		public readonly IDictionary<string, string> solutions = new Dictionary<string, string>();
		public SolutionsRepo(string solvedCarsFile)
		{
			this.solvedCarsFile = solvedCarsFile;
			if (!File.Exists(solvedCarsFile))
				File.WriteAllText(solvedCarsFile, "");
			var lines = File.ReadAllLines(solvedCarsFile);
			foreach(var line in lines)
			{
				var parts = line.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries);
				var carId = parts[0];
				var fuel =  parts[1];
				solutions.Add(carId, fuel);
			}
		}

		public void AddSolution(string carId, string fuel)
		{
			if(solutions.ContainsKey(carId)) return;
			File.AppendAllText(solvedCarsFile, carId + ' ' + fuel + Environment.NewLine);
			solutions.Add(carId, fuel);
			Console.WriteLine("Solved {0} : fuel len = {1}", carId, fuel);
		}
	}
}