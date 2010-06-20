using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit;

namespace CircuitCalc.CarCreator
{
	[TestFixture]
	class Car_Tests
	{
		[Test]
		public void ConnectednessTest()
		{
			var rep = new CarsRepo("../../cars1.txt");
			for (var i = 0; i < 7; i++)
			{
				if (rep.carsByTanksCount[i] == null)
				{
					continue;
				}
				foreach (var carString in rep.carsByTanksCount[i])
				{
					var car = new Car(rep.cars[carString], i);
					Assert.True(car.IsConnected(), "Машинка не сцепленна...");
				}
			}
		}

	}
}
