using System;
using CircuitCalc.PeCalc;
using NUnit.Framework;

namespace CircuitCalc.FindTheInput
{
	[TestFixture]
	public class FindInput_Test
	{
		[Test]
		public void TestCase()
		{
			Func<Calculator> createSimple = () => new Calculator("simple.txt");
			Func<Calculator> createSimple2 = () => new Calculator("simple2.txt");
			var simpleResult = "02120112100002120";
			var simple2Result = "01210221200001210";
			string prefix = "";
			for(int i = 0; i< simpleResult.Length; i++)
			{
				prefix = TryImprovePrefix(simple2Result, prefix, createSimple2);
				if(prefix.Length == i)
					prefix = TryImprovePrefix(simpleResult, prefix, createSimple);
				if(prefix.Length == i)
				{
					Console.WriteLine("FAIL");
					break;
				}
			}
		}

		private string TryImprovePrefix(string actualResult, string prefix, Func<Calculator> createScheme)
		{
			for(char next = '0'; next < '3'; next++)
			{
				string candidate = prefix+next;
				string result = createScheme().PushString(candidate);
				if (actualResult.StartsWith(result))
				{
					prefix = candidate;
					Console.WriteLine("PREFIX IS: {0} on OUTPUT {1}", prefix, result);
					break;
				}
			}
			return prefix;
		}
	}
}
