using System;
using CircuitCalc.PeCalc;
using NUnit.Framework;
using System.Linq;

namespace CircuitCalc.FindTheInput
{
	public class Fact{
		public Fact(string filename, string knownOutput)
		{
			this.filename = filename;
			this.knownOutput = knownOutput;
		}
		public bool IsGoodInput(string input)
		{
			Console.WriteLine(filename);
			string realOut = new Calculator(filename).PushString(input);
			bool isGoodInput = knownOutput.StartsWith(realOut);
			if (!isGoodInput) Console.WriteLine("{0} -> {1}, expected: {2}   ({3})", input, realOut, knownOutput, filename);
			return isGoodInput;
		}

		private string filename;
		private string knownOutput;
	}
	[TestFixture]
	public class FindInput_Test
	{
		[Test]
		public void Simple2()
		{
			string input = "01202101210201202";
			string res = new Calculator("simple2.txt").PushString(input);
			Console.WriteLine("{0} -> {1}", input, res);
		}
		//01202101210201202
		[Test]
		public void TestCase()
		{
			string prefix = "";
			for(int i = 0; i< 17; i++)
			{
				bool r0 = Try(prefix+'0');
				bool r1 = Try(prefix+'1');
				bool r2 = Try(prefix+'2');
				int c = 0;
				if(r0) c++;
				if(r1) c++;
				if(r2) c++;
				if (c != 1)
				{
					Console.WriteLine("FAIL! prefix: {0}({1}{2}{3})", prefix, r0?"0":"", r1?"1":"", r2?"2":"");
					break;
				}
				else
				{
					if(r0) prefix += '0';
					if(r1) prefix += '1';
					if(r2) prefix += '2';
					Console.WriteLine("new prefix: {0}", prefix);
				}
			}
		}
		private readonly Fact[] facts = new[]
			{
				new Fact("simple.txt",  "02120112100002120"), 
				new Fact("simple2.txt", "01210221200001210"),
				new Fact("simple3.txt", "22022022022022022"), 
				new Fact("sample.txt", "10221220002011011"), 
			};

		private bool Try(string input)
		{
			Console.WriteLine("TRY: {0}", input);
			return facts.All(f => f.IsGoodInput(input));
		}
	}
}
