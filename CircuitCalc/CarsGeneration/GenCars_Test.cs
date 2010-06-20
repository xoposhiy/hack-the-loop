using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CircuitCalc.TParsing;
using NUnit.Framework;

namespace CircuitCalc.CarsGeneration
{
	[TestFixture]
	public class GenCars_Test
	{
		[Test]
		public void FindAB()
		{
			while(true)
			{
				var a = Matrix.Random(2);
				var b = Matrix.Random(2);
				var ab_ba = a.Mult(b).Sub(b.Mult(a));
				if (ab_ba.IsNonNegative() && ab_ba.NonZero())
				{
					Console.WriteLine("AB - BA > 0");
					Console.WriteLine(a);
					Console.WriteLine(b);
					Console.WriteLine("AB = ");
					Console.WriteLine(a.Mult(b));
					Console.WriteLine("BA = ");
					Console.WriteLine(b.Mult(a));
					break;
				}
			}
		}
	}
}
