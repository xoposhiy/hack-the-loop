using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircuitCalc.TParsing
{
	public class TStream
	{
		private IEnumerator<char> e;
		private bool hasNext;
		public int pos;

		public TStream(string input)
			:this(input.Replace(" ", "").GetEnumerator())
		{
			
		}
		public TStream(IEnumerator<char> e)
		{
			this.e = e;
			hasNext = e.MoveNext();
			pos++;
		}

		public bool HasNext()
		{
			return hasNext;
		}

		public char Next()
		{
			if(!hasNext) throw new Exception("empty!");
			try
			{
				return e.Current;
			}finally
			{
				hasNext = e.MoveNext();
				pos++;
			}
		}
	}
	public class TParser
	{
		public Chamber ParseChamber(TStream s)
		{
			return new Chamber
				{
					upper = ParseList<int>(s, ParseNumber),
					isMaster = ParseNumber(s) == 0,
					lower = ParseList<int>(s, ParseNumber),
				};
			
		}
		
		public Matrix[] ParseFuel(TStream s)
		{
			return ParseList<Matrix>(s, ParseMatrix);
		}

		private Matrix ParseMatrix(TStream s)
		{
			return new Matrix(ParseList(s, ss => ParseList<int>(ss, ParseNumber)));
		}

		public Chamber[] ParseCar(string s)
		{
			return ParseChambers(new TStream(s));
		}

		public Chamber[] ParseChambers(TStream s)
		{
			Chamber[] chs = ParseList<Chamber>(s, ParseChamber);
			if (s.HasNext()) throw new Exception("not empty tail!!! " + ReadTail(s));
			return chs;
		}

		private string ReadTail(TStream s)
		{
			var res = "";
			while(s.HasNext())
				res += s.Next();
			return res;
		}

		public T[] ParseList<T>(TStream s, Func<TStream, T> parse)
		{
			var c = s.Next();
			if (c == '0') return new T[0];
			if (c == '1') return new []{parse(s)};
			if(c == '2')
			{
				return ParseElements(s, parse);
			}
			throw new Exception("unknown c: " + c);
		}

		private T[] ParseElements<T>(TStream s, Func<TStream, T> parse)
		{
			var c = s.Next();
			if(c != '2') throw new Exception("2 expected. was: " + c + " pos: " + s.pos);
			var len = ParseNumber(s) + 2;
			var res = new T[len];
			for(int i=0; i<len; i++)
				res[i] = parse(s);
			return res;
		}

		public int ParseNumber(TStream s)
		{
			var c = s.Next();
			if(c == '0') return 0;
			if(c == '1') return ParseDigit(s) + 1;
			if (c == '2')
			{
				var l = ParseElements<int>(s, ParseDigit);
				var n = Decode3(l);
				var p = (int)Math.Round(Enumerable.Range(0, l.Length).Select(i => Math.Pow(3, i)).Sum());
				return p + n;
			}
			throw new Exception("unknown c: " + c);

		}

		private int Decode3(int[] digits)
		{
			return digits.Aggregate(0, (res, d) => res*3 + d);
		}

		private int ParseDigit(TStream s)
		{
			return s.Next() - '0';
		}
	}

	public class Chamber
	{
		public bool isMaster;
		public int[] upper;
		public int[] lower;

		public int[] AllTanks()
		{
			return upper.Concat(lower).ToArray();
		}

		public int TanksCount()
		{
			return AllTanks().Count() == 0 ? 0 : AllTanks().Max() + 1;
		}

		public override string ToString()
		{
			var sb = new StringBuilder("Chamber[" + isMaster + ", ");
			sb.Append("(");
			foreach(var i in upper)
				sb.Append(i + " ");
			sb.Append(")");
			sb.Append(", ");
			sb.Append("(");
			foreach(var i in lower)
				sb.Append(i + " ");
			sb.Append(")");
			sb.Append("]");
			return sb.ToString();
		}
	}
}
