using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CircuitCalc.PeCalc
{
	[TestFixture]
	public class Calc_Test
	{
		[Test]
		public void TestCase()
		{
			var calculator = new Calculator("sample.txt", "0222220210110011");
			for(int i = 0; i < 17; i++) calculator.PushNext();
		}
	}
}
