using System;
using System.Collections.Generic;
using System.Linq;

namespace CircuitCalc.TParsing
{
	public class TEncoder
	{
		public string EncodeList<T>(IEnumerable<T> items, Func<T, string> encoder)
		{
			return items.Aggregate(
				GetPrefix(items.Count()),
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

		private string GetPrefix(int number)
		{
			string[] res = new[] {"0", "1", "220", "2210", "2211", "2212", "2222000"};
			return (number >= 0 && number < res.Length) ? res[number] : "<bad>";
		}

		private string EncodeNumber(int number)
		{
			if(number >= 4)
			{
				if(number <= 12)
				{
					return "220" + Conv3(number - 4, 2);
				}
				else if(number <= 39)
				{
					return "2210" + Conv3(number - 13, 3);
				}
				else if(number <= 120)
				{
					return "2211" + Conv3(number - 40, 4);
				}
				else if(number <= 363)
				{
					return "2212" + Conv3(number - 121, 5);
				}
				else if(number <= 1092)
					return "2222000" + Conv3(number - 364, 6);
				else
					return "<unimpl>";
			}
			else if(number >= 0)
			{
				string[] res1 = new[] {"0", "10", "11", "12"};
				return res1[number];
			}
			else
				return "<undef>";
		}
	}
}