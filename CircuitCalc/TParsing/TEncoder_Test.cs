using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CircuitCalc.TParsing
{
	[TestFixture]
	public class TEncoder_Test
	{
		[Test]
		public void Numbers()
		{
			var e = new TEncoder();
			for(int i = 1; i < 2000; i++)
			{
				Console.WriteLine(e.EncodeNumber(i));
			}
		}
	}
}
