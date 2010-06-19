using System.Linq;
using System.Text;

namespace CircuitCalc.CircuitBuilding
{
	class CircuitSerializer
	{
		//элементы primitives кодируют последовательность базовых примитивов H0, H1, H2
		public string Serialize(byte[] primitives)
		{
			var primitiveSizes = primitives.Select(p => GetPrimitiveSize(p)).ToList();
			var size = primitiveSizes.Sum() + 1;
			var sb = new StringBuilder();
			var lastIdx = size - 1;
			sb.AppendLine(string.Format("{0}L:", lastIdx));
			var idxIn = 0;
			var idxOut = 0;
			for (var k = 0; k < lastIdx; k++)
			{
				idxIn = k == lastIdx - 1 ? lastIdx : primitiveSizes.Take(k + 2).Sum() - 1;
				idxOut = k == 0 ? lastIdx : primitiveSizes.Take(k).Sum() - 1;
				switch (primitives[k])
				{
					case 0:
						SerializeH0(sb, k, idxIn, idxOut);
						break;
					case 1:
						SerializeH1(sb, k, idxIn, idxOut);
						break;
					case 2:
						SerializeH2(sb, k, idxIn, idxOut);
						break;
				}
			}
			var firstPrimitiveOutIdx = GetPrimitiveSize(primitives.First(p => p != 0)) - 1;
			var lastPrimitiveInIdx = (size - 1) - GetPrimitiveSize(primitives.Last(p => p != 0));
			sb.AppendLine(string.Format("X{0}L0#X{1}L:", firstPrimitiveOutIdx, lastPrimitiveInIdx));
			sb.AppendLine(string.Format("{0}L", lastIdx));
			return sb.ToString();
		}

		private static void SerializeH0(StringBuilder sb, int k, int idxIn, int idxOut)
		{
			sb.AppendLine(string.Format("{0}L{1}R0#{2}L{1}R,", idxIn, k, idxOut));
		}

		private static void SerializeH1(StringBuilder sb, int k, int idxIn, int idxOut)
		{
			sb.AppendLine(string.Format("{0}L{1}R0#{0}L{1}R,", k, k + 1));
			sb.AppendLine(string.Format("{0}L{1}R0#{2}L{1}R,", idxIn, k, idxOut));
		}

		private static void SerializeH2(StringBuilder sb, int k, int idxIn, int idxOut)
		{
			sb.AppendLine(string.Format("{0}L{1}R0#{0}L{2}R,", k, k + 2, k + 1));
			sb.AppendLine(string.Format("{0}R{1}R0#{2}R{0}L,", k+1, k, k + 2));
			sb.AppendLine(string.Format("{0}L{1}L0#{2}L{3}R,", idxIn, k + 1, idxOut, k));
		}

		private static int GetPrimitiveSize(byte p)
		{
			return p + 1;
		}
	}
}