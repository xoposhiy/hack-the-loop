using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircuitCalc.CarCreator
{
	class CarFactory
	{
		public CarFactory()
		{
	
		}
		
		public Car GetCar(int numOfTanks, int seed)
		{
			return new Car(null, 1);
		}
	}
}
