using System;
using NUnit.Framework;

namespace CircuitCalc.PeCalc
{
	[TestFixture]
	public class Calc_Test
	{
		private const string twiceRealOut = "12211010022120012";
		private const string keyPrefix = "11021210112101221";
										  

		[Test]
		public void TestCase()
		{
			var calculator = new Calculator("sample.txt");
			const string sampleIn = "02222220210110011";
			const string realIn = "02102202202202202";
			const string sampleRealOut = "10221220002011011";
			string myOut = calculator.PushString(sampleIn);
			Console.WriteLine();
			//Console.WriteLine(sampleRealOut);
			Console.WriteLine(myOut);
		}

		[Test]
		public void Debug()
		{
			var calculator = new Calculator("tmp.txt");
			Console.WriteLine(calculator.PushString("22022022022022022"));
		}
	}
}
