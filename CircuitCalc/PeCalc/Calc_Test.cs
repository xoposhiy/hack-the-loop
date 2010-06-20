using System;
using System.IO;
using NUnit.Framework;

namespace CircuitCalc.PeCalc
{
	[TestFixture]
	public class Calc_Test
	{
		[Test]
		public void ExtendServerInput1()
		{
			new ServerInputFinder().Run();
		}

		[Test]
		public void SubmitFuel1()
		{
			var lines = File.ReadAllLines(@"c:\work\icfpc2010\others\tbd\simulator\data\car_data_sorted");
			foreach(var line in lines)
			{
				var parts = line.Split(',');
				if(parts.Length > 1 && parts[1].Trim().StartsWith("1"))
				{
					Console.WriteLine(parts[0].Trim());
				}
			}
		}

		[Test]
		public void T()
		{
			Console.WriteLine(new Calculator("tmp.txt")
				.PushString(Consts.serverInput));
		}

		[Test]
		public void TestCase()
		{
			var calculator = new Calculator("sample.txt");
			const string sampleIn = "02222220210110011";
			const string realIn = "01202101210201202";
			const string sampleRealOut = "10221220002011011";
			string myOut = calculator.PushString(sampleIn);
			Console.WriteLine();
			//Console.WriteLine(sampleRealOut);
			Console.WriteLine(myOut);
		}

		[Test]
		public void TestXOR()
		{
			var input = "01202101210201202";
			//Console.WriteLine(new Calculator("simple.txt").PushString(input));
			/*            Console.WriteLine(new Calculator(new []
                                                 {
                                                     "0R:",
                                                     "2LX0#1LX,",
                                                     "0L2R0#2L2R,",
                                                     "1L1R0#0L1R:",
                                                     "0R"
                                                 }).PushString(input));*/

			/*            Console.WriteLine(new Calculator(new []
                                                             {
                                                                 "0R:2LX0#1LX,0L2R0#2L2R,1L1R0#0L1R:0R"
                                                             }).PushString(input));*/


			/*            Console.WriteLine(new Calculator(new []
                                                 {
                                                    "19L:",
                                                    "12R13R0#1R12R,",
                                                    "14R0L0#4R9L,",
                                                    "9R10R0#3L8L,",
                                                    "2L17R0#5L9R,",
                                                    "15R1L0#10R13R,",
                                                    "3L18R0#6L15L,",
                                                    "5L11R0#13L12L,",
                                                    "19R16R0#11R8R,",
                                                    "2R7R0#11L10L,",
                                                    "1R3R0#18L2L,",
                                                    "8R4L0#16L2R,",
                                                    "8L7L0#15R6R,",
                                                    "6R0R0#14L0L,",
                                                    "6L4R0#14R0R,",
                                                    "12L13L0#17L1L,",
                                                    "5R11L0#16R4L,",
                                                    "10L15L0#17R7R,",
                                                    "14L16L0#18R3R,",
                                                    "9L17L0#19R5R,",
                                                    "X18L0#X7L:",
                                                    "19L"
                                                 }).PushString(input));*/

			/*            Console.WriteLine(new Calculator(new[]
                                                 {
                                                     "2R:",
                                                     "2L1R0#1L1R,",
                                                     "0L0R0#2L0R,",
                                                     "1LX0#0LX:",
                                                     "2R"
                                                 }).PushString(input));*/
			/*++
            Console.WriteLine(new Calculator(new[]
                                                 {
                                                     "0L:",
                                                     "X0R0#1R0R,",
                                                     "10L0X#1LX:",
                                                     "1R"
                                                 }).PushString(input));
            --*/
		}

		[Test]
		public void Debug()
		{
			var calculator = new Calculator("tmp.txt");
			Console.WriteLine(calculator.PushString("22022022022022022"));
		}

		[Test]
		public void Search()
		{
			const string input = "01202101210201202";
			//const string input = "11021210112101221";
			const string target = "11021210112101221";
			var files = new string[]
				{
					"generated.txt",
					//                                        @"D:\fact4.txt",
					@"D:\fact5.txt"
				};
			string s;
			string collect = "";
			bool any = false;
			for(int i = 0; i < files.Length; i++)
			{
				Console.WriteLine("Parsing " + files[i]);
				var sr = new StreamReader(files[i]);
				while((s = sr.ReadLine()) != null)
				{
					if(s.Length > 0)
					{
						collect += s + "\n";
					}
					else
					{
						collect = collect.Substring(0, collect.Length - 1);
						string res = new Calculator(collect.Split('\n')).PushString(input);
						if(res == target)
						{
							any = true;
							Console.WriteLine(collect);
							Console.WriteLine("-----------------------------------");
						}
						collect = "";
					}
				}
			}
			if(!any)
			{
				Console.WriteLine("No factories");
			}
		}
	}
}