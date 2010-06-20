using System;
using System.Linq;
using CircuitCalc.PeCalc;

namespace CircuitCalc.CircuitBuilding
{
	public class Builder
	{
		private readonly byte[,] o_kt;
		private readonly byte[] o_t0;
		private readonly int n;

		public static string Build(string input, string wantedOutput)
		{
			return new CircuitSerializer().Serialize(new Builder(input, wantedOutput).Build());
		}

		public Builder(string input, string wantedOutput)
		{
			n = wantedOutput.Length;
			o_t0 = new byte[n];
			o_kt = new byte[n,n];
			for(int t=0; t<n; t++)
				o_kt[0, t] = GetRightInput(Char2Byte(input[t]), Char2Byte(wantedOutput[t]));
			o_t0[0] = o_kt[0, 0];
		}

		private byte Char2Byte(char ch)
		{
			return (byte) (ch - '0');
		}

		// http://clip2net.com/clip/m21010/1276953465-clip-2kb.png
		private byte GetRightInput(byte leftIn, byte leftOut)
		{
			return (byte) ((leftIn - leftOut + 3)%3); //rin(lin, lout) = Lout(lin, lout)! wow!
		}

		/// <returns>i-ый элемент - это out(i, 0) - 0, 1 или 2</returns>
		public byte[] Build()
		{
			for (int k=1; k<n; k++)
			{
				for (int t = 0; t < n-k; t++)
					o_kt[k, t] = GetWantedInput(k-1, t + 1);
				o_t0[k] = o_kt[k, 0];
			}
			return o_t0;
		}

		private byte GetWantedInput(int k, int t)
		{
			byte hType = o_kt[k, 0];
			var wantedOut = o_kt[k, t];
			string prefix = BuildPrefix(k, t - 1);
			var wantedChar = (char) (wantedOut + '0');
			var schemeName = string.Format("h{0}.txt", hType);
			if (LastCharFor(schemeName, prefix + '0') == wantedChar) return 0;
			if (LastCharFor(schemeName, prefix + '1') == wantedChar) return 1;
			if (LastCharFor(schemeName, prefix + '2') == wantedChar) return 2;
			throw new Exception("все плохо. h1, prefix:" + prefix + ", wantedChar:" + wantedChar);
		}

		private char LastCharFor(string filename, string inp)
		{
			return new Calculator(filename).PushString(inp).Last();
		}

		private string BuildPrefix(int k, int lastT)
		{
			string s = "0";
			for(int t=1; t<=lastT; t++)
			{
				var in_kt = o_kt[k + 1, t-1];
				s += (char)('0' + in_kt);
			}
			return s;
		}
	}
}
