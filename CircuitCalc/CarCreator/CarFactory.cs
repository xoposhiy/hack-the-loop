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

		/// <summary>
		/// Генератор машинок со сложными ограничениями. В частности:
		/// Степенное ограничение: одно топливо должно быть в точностью некоей степенью другого топлива
		/// "Круговое" ограничение то есть граф зависимости, при попытке вычислить какое топливо зависит от которого, является циклическим
		/// Смотри car1 из CreationTesting
		/// </summary>
		/// <param name="pow1">Первое степенное ограничение</param>
		/// <param name="pow2">Второе степенное ограничение</param>
		/// <param name="pow3">Третье степенное ограничение</param>
		public Car GetCarLoopWithLenearStricts(int pow1, int pow2, int pow3)
		{
			int baseMatrix = 2;
			decimal fuel1 = 10000*100000000000;
			fuel1 *= fuel1;
			Console.WriteLine(fuel1);
			return new Car(null, 1);
		}
	}
}
