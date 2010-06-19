using System;
using System.Text;
using NUnit.Framework;
using System.Linq;

namespace CircuitCalc
{
	class Trit
	{
		public static int ToDec(string s)
		{
			int res = 0;
			int pow = 1;
			for (var i = s.Length - 1; i >= 0; i--)
			{
				res += pow*byte.Parse(s[i].ToString());
				pow *= 3;
			}
			return res;
		}

		public static string ToTrits(int n)
		{
			var sb = new StringBuilder();
			while (n > 0)
			{
				sb.Append(n%3);
				n /= 3;
			}
			return new string(sb.ToString().ToCharArray().Reverse().ToArray());
		}
	}

	[TestFixture]
	public class Trit_Test
	{
		[Test]
		public void Calc()
		{
			Console.WriteLine(Trit.ToDec("122000010"));
			Console.WriteLine(Trit.ToDec("12200001"));
			Console.WriteLine(Trit.ToDec("1220000"));
			Console.WriteLine(Trit.ToDec("122000"));
			Console.WriteLine(Trit.ToDec("12200"));
			Console.WriteLine(Trit.ToDec("1220"));
			Console.WriteLine(Trit.ToDec("122"));
			Console.WriteLine(Trit.ToDec("12"));
			Console.WriteLine(Trit.ToDec("122"));
			Console.WriteLine(Trit.ToDec("220"));
			Console.WriteLine(Trit.ToDec("221"));
			Console.WriteLine(Trit.ToDec("222"));
			Console.WriteLine(Trit.ToDec("10022111"));
		}

		[Test]
		public void ToTrits()
		{
			Console.WriteLine(Trit.ToTrits(2416));
			Console.WriteLine(Trit.ToTrits(2711));
		}
	}
}