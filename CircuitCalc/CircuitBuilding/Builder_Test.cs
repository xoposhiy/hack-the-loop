using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CircuitCalc.PeCalc;
using NUnit.Framework;

namespace CircuitCalc.CircuitBuilding
{
	[TestFixture]
	public class Builder_Test
	{
		[Test]
		public void H1()
		{
			var bytes = new Builder("01202101210201202", "11021210112101221").Build();
			foreach (var b in bytes)
			{
				Console.Write(b + " ");
			}
			var serialize = new CircuitSerializer().Serialize(bytes);
			Console.WriteLine(serialize);
		}

		[Test]
		public void Requirement_description()
		{
			Console.WriteLine(new Calculator("h2.txt").PushString("01202101210201202"));
		}

	}
}
