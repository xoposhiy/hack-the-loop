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
		public void TestCase()
		{
			var chambers = new TParser().ParseChambers(new TStream("122000010"));
			foreach(var chamber in chambers)
			{
				Console.WriteLine(chamber);
			}
		}
	}
}
