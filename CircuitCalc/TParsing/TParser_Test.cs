using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CircuitCalc.TParsing
{
	[TestFixture]
	public class TParser_Test
	{
		[Test]
		public void ParseCar()
		{
			Console.WriteLine(new TParser().ParseNumber(new TStream("22022")));
			var chambers = new TParser().ParseChambers(new TStream("12200010112"));
			foreach(var chamber in chambers) Console.WriteLine(chamber);
		}
		[Test]
		public void ParseFuel()
		{
			var ms = new TParser().ParseFuel(new TStream("11110"));
			foreach(var m in ms)
			{
				Console.WriteLine(m.ToString());
				Console.WriteLine();
			}
		}
	}
}
