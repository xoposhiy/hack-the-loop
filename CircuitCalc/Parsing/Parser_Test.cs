using System;
using NUnit.Framework;

namespace CircuitCalc.Input
{
	[TestFixture]
	public class Parser_Test
	{
		[Test]
		public void TestCase()
		{
			var parser = new Parser();
			parser.Parse("sample.txt", new ConsoleSink());
		}
	}

	public class ConsoleSink : ISink
	{
		public void OnGate(int gateIndex, Point leftIn, Point rightIn, Point leftOut, Point rightOut)
		{
			Console.WriteLine("{0}: {1}, {2} -> {3}, {4}", gateIndex, leftIn, rightIn, leftOut, rightOut);
		}

		public void OnInput(Point to)
		{
			Console.WriteLine(to);
		}

		public void OnOutput(Point from)
		{
			Console.WriteLine(from);
		}
	}
}
