using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CircuitCalc
{
	class Program
	{
		static void Main(string[] args)
		{
			CodeMonkeysGen(args);
		}

		private static void CodeMonkeysGen(string[] args)
		{
			int size = 10;
			if (args.Length > 0)
			{
				size = int.Parse(args[0]);
			}
			string fOut = "output.txt";
			if (args.Length > 1)
			{
				fOut = args[1];
			}
			var pw = new StringBuilder();
			pw.AppendFormat("{0}L:", size);
			pw.AppendLine();
			pw.AppendFormat("{0}R{1}L0#{2}R{3}R,", 1, 0, 0, size);
			pw.AppendLine();
			for (int i = 1; i < size; i++)
			{
				pw.AppendFormat("{0}R{1}L0#{2}R{3}L,", i + 1, i, i, i - 1);
				pw.AppendLine();
			}
			pw.AppendFormat("X{0}R0#X{1}L:", 0, size - 1);
			pw.AppendLine();
			pw.AppendFormat("{0}L", size);
			pw.AppendLine();
			File.WriteAllText(fOut, pw.ToString());
		}
	}
}
