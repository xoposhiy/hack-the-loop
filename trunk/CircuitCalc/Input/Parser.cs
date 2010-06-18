using System;
using System.IO;
using System.Linq;

namespace CircuitCalc.Input
{
	internal class Parser
	{
		public void Parse(string filename, ISink sink)
		{
			Parse(File.ReadAllLines(filename), sink);
		}

		public void Parse(string[] lines, ISink sink)
		{
			string s = lines[0].Trim().TrimEnd(':');
			char side = s.Last();
			int index = int.Parse(s.Substring(0, s.Length - 1));
			sink.OnInput(new Point(index, side));
			for(int i = 1; i < lines.Length - 1; i++)
				ParseLine(i-1, lines[i], sink);
			s = lines.Last().Trim();
			side = s.Last();
			index = int.Parse(s.Substring(0, s.Length - 1));
			sink.OnOutput(new Point(index, side));
		}

		private static void ParseLine(int gateIndex, string line, ISink sink)
		{
			string[] parts = line.Split(new[] {"0#"}, StringSplitOptions.None);
			Point lin, rin, lout, rout;
			ParsePart(parts[0], out lin, out rin);
			ParsePart(parts[1], out lout, out rout);
			sink.OnGate(gateIndex, lin, rin, lout, rout);
		}

		private static void ParsePart(string part, out Point left, out Point right)
		{
			int i = 0;
			if(part[i] == 'X')
			{
				left = new Point(-1, 'X');
			}
			else
			{
				while(char.IsDigit(part[i])) i++;
				left = new Point(int.Parse(part.Substring(0, i)), part[i]);
			}
			int start = i+1;
			i = 0;
			if(part[start + i] == 'X')
			{
				right = new Point(-1, 'X');
			}
			else
			{
				while(char.IsDigit(part[start + i])) i++;
				right = new Point(int.Parse(part.Substring(start, i)), part[start + i]);
			}
		}
	}

	///<summary><c>side = R | L | X (for external gate)</c>
	/// <para><c>gateIndex = -1</c> for external gate.</para>
	/// </summary>
	public struct Point
	{
		public Point(int gate, char side)
		{
			this.gate = gate;
			this.side = side;
		}

		public override string ToString()
		{
			return gate >= 0 ? string.Format("{0}{1}", gate, side) : side.ToString();
		}

		public int gate;
		public char side;
	}

	public interface ISink
	{
		void OnGate(int gateIndex,
			Point leftIn, Point rightIn,
			Point leftOut, Point rightOut
			);

		void OnInput(Point to);
		void OnOutput(Point from);
	}
}