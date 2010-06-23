using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CircuitCalc.PeCalc;
using NUnit.Framework;

namespace CircuitCalc.CircuitBuilding
{
	[TestFixture]
	public class Builder_Test
	{
		[Test]
		[STAThreadAttribute]
		public void H1()
		{
			var bytes = new Builder(Consts.serverInput, "2222000111111121111111211111111").Build();
			//var bytes = new Builder("0120210121020120200000000000", "11021210112101221110000000").Build();
			//var bytes = new Builder("0", "1").Build();
			foreach (var b in bytes)
			{
				Console.Write(b + " ");
			}
			Console.WriteLine();
			var serialize = new CircuitSerializer().Serialize(bytes);
			Console.WriteLine(serialize);
			Clipboard.SetText(serialize);
		}

		[Test]
		public void Requirement_description()
		{
			Console.WriteLine(new Calculator("car0.txt").PushString("01202101210201202000000000000000000000000"));
		}

	}
}
