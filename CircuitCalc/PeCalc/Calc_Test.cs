using System;
using NUnit.Framework;

namespace CircuitCalc.PeCalc
{
	[TestFixture]
	public class Calc_Test
	{
		[Test]
		public void TestCase()
		{
			var calculator = new Calculator("sample.txt");
			const string sampleIn = "02222220210110011";
			const string realIn = "02102202202202202";
			const string sampleRealOut = "10221220002011011";
			string myOut = calculator.PushString(realIn);
			Console.WriteLine();
			Console.WriteLine(sampleRealOut);
			Console.WriteLine(myOut);
		}
	}
}
