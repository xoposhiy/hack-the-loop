using System;
using System.Collections.Generic;
using System.Linq;

namespace CircuitCalc.TParsing
{
	public class TEncoder
	{
		public string EncodeList<T>(IEnumerable<T> items, Func<T, string> encoder)
		{
			var len = items.Count();
			if(len == 0) return "0";
			if(len == 1) return "1" + encoder(items.First());
			var prefix = "22" + EncodeNumber(len - 2);
			return items.Aggregate(
				prefix,
				(value, nextM) => value + encoder(nextM));
		}

		public string EncodeSimpleFuel(params int[] ms)
		{
			return EncodeFuel(ms.Select(v => Matrix.Simple(v)).ToArray());
		}

		public string EncodeFuel(Matrix[] ms)
		{
			return EncodeList(
				ms,
				matrix =>
					EncodeList(matrix.items, row => EncodeList(row, EncodeNumber)));
		}
		public  string EncodeCar(Chamber[] car)
		{
			return EncodeList(car, EncodeChamber);
		}
		public string EncodeChamber(Chamber chmbr)
		{
			var res = EncodeList(chmbr.upper, EncodeNumber);
			var master = 1;
			if(chmbr.isMaster)
			{
				master = 0;
			}
			res += EncodeNumber(master);
			res += EncodeList(chmbr.lower, EncodeNumber);
			return res;
		}

		private string Conv3(int number, int pad)
		{
			string result = "";
			while(number > 0)
			{
				result = (number%3) + result;
				number /= 3;
			}
			return result.PadLeft(pad, '0');
		}

//		private string GetPrefix(int number)
//		{
//			string[] res = new[] {"0", "1", "220", "2210", "2211", "2212", "2222000"};
//			return (number >= 0 && number < res.Length) ? res[number] : "<bad>";
//		}

		public string EncodeNumber(int number)
		{
			int p = 0;
			for(int i=0; i<10000; i++)
			{
				int p_prev = p;
				p += (int)Math.Round(Math.Pow(3, i));
				if (number < p)
				{
					var digits = EncodeBase3(number - p_prev, i);
					return EncodeList(digits, EncodeDigit);
				}
			}
			throw new Exception("!");
		}

		private IEnumerable<int> EncodeBase3(int n, int digits)
		{
			var acc = new List<int>();
			for(int i=0; i<digits; i++)
			{
				var d = n/3;
				var m = n%3;
				n = d;
				acc.Add(m);
			}
			if (n != 0) throw new Exception(n.ToString());
			acc.Reverse();
			return acc;
		}

		private static string EncodeDigit(int arg)
		{
			return arg.ToString();
		}

//		public string EncodeNumber(int number)
//		{
//			if(number >= 4)
//			{
//				if(number <= 12)
//				{
//					return "220" + Conv3(number - 4, 2);
//				}
//				else if(number <= 39)
//				{
//					return "2210" + Conv3(number - 13, 3);
//				}
//				else if(number <= 120)
//				{
//					return "2211" + Conv3(number - 40, 4);
//				}
//				else if(number <= 363)
//				{
//					return "2212" + Conv3(number - 121, 5);
//				}
//				else if(number <= 1092)
//					return "2222000" + Conv3(number - 364, 6);
//				else
//					return "<unimpl>";
//			}
//			else if(number >= 0)
//			{
//				string[] res1 = new[] {"0", "10", "11", "12"};
//				return res1[number];
//			}
//			else
//				return "<undef>";
//		}
	}
}