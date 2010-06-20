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
		private TParser parser = new TParser();

		[Test]
		public void ParseStuff()
		{
			parser.ParseFuel(new TStream("1211222111")); 
			foreach(var i in parser.ParseList<int>(new TStream("11021210112101221"), parser.ParseNumber))
			{
				Console.Write(i + " ");
			}
			Console.WriteLine();
		}

		[Test]
		public void ParseCar()
		{
			var chambers = parser.ParseChambers(new TStream("122000010"));
			foreach(var chamber in chambers) Console.WriteLine(chamber);
		}

		[Test]
		public void ParseFuel()
		{
			var ms = new TParser().ParseFuel(new TStream("1211222111"));
			foreach(var m in ms)
			{
				Console.WriteLine(m.ToString());
				Console.WriteLine();
			}
		}
	}
}
