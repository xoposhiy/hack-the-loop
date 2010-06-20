using System;
using CircuitCalc.TParsing;

namespace CircuitCalc
{
	public static class CarExtensions
	{
		public static void Print(this Chamber[] car)
		{
			foreach(var chamber in car)
			{
				Console.WriteLine(chamber.ToString());
			}
		}
		public static void Print(this Matrix[] fuel)
		{
			foreach(var m in fuel)
			{
				Console.WriteLine(m.ToString());
			}
		}

	}
}