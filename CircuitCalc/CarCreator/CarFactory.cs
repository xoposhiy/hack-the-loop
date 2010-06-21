using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarlJohansen;
using CircuitCalc.TParsing;

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
		/// <param name="pow0">Первое степенное ограничение</param>
		/// <param name="pow1">Второе степенное ограничение</param>
		/// <param name="pow2">Третье степенное ограничение</param>
		/// <param name="pow3">как и pow5 параметры для ограничения типа fuel1^pow4 = fuel2^pow5</param>
		/// <param name="pow4">как и pow4 параметры для ограничения типа fuel1^pow4 = fuel2^pow5</param>
		public Car GetCarLoopWithLenearStricts(int pow0, int pow1, int pow2, int pow3, int pow4)
		{
			var chambers = new List<Chamber>();

			// Кольцо. топливо подходит, если в двух баках подряд не единичные матрицы
			chambers.Add(new Chamber() {isMaster = true, lower = new int[] {0}, upper = new int[] {0, 1}});
			chambers.Add(new Chamber() { isMaster = true, lower = new int[] { 1 }, upper = new int[] { 1, 2 } });
			chambers.Add(new Chamber() { isMaster = true, lower = new int[] { 2 }, upper = new int[] { 2, 3 } });
			chambers.Add(new Chamber() { isMaster = true, lower = new int[] { 3 }, upper = new int[] { 3, 4 } });
			chambers.Add(new Chamber() { isMaster = true, lower = new int[] { 4 }, upper = new int[] { 4, 5 } });
			chambers.Add(new Chamber() { isMaster = true, lower = new int[] { 5 }, upper = new int[] { 5, 0 } });

			// Степени для ограничений вида FuelN*336 <= FuelN+3  
			var pows = new[] {pow0, pow1, pow2, pow3, pow4};
			// Инициируем топливо
			var fuels = new Matrix[]
			            	{
			            		new Matrix(new[] {new int[] {1, 0}, new int[] {1, 1}}),
			            		new Matrix(new[] {new int[] {1, 0}, new int[] {pow4, 1}}),
			            		new Matrix(new[] {new int[] {1, 0}, new int[] {pow3, 1}}),
			            		new Matrix(new[] {new int[] {1, 0}, new int[] {pow0, 1}}),
			            		new Matrix(new[] {new int[] {1, 0}, new int[] {pow4*pow1, 1}}),
			            		new Matrix(new[] {new int[] {1, 0}, new int[] {pow3*pow2, 1}})
			            	};
			Chamber acc;
			// ограничений вида FuelN*336 == FuelN+3
			for (var i = 0; i < 3; i++)
			{
				// FuelN+2 <= FuelN^powN
				acc = new Chamber();
				acc.isMaster = false;
				acc.lower = new int[]{i+3};
				acc.upper = Enumerable.Repeat<int>(i, pows[i]).ToArray();
				chambers.Add(acc);
				// FuelN^(powN-1) < Fuel+2
				acc = new Chamber();
				acc.isMaster = true;
				acc.upper = new int[] { i+3 };
				acc.lower = Enumerable.Repeat<int>(i, pows[i]-1).ToArray();
				chambers.Add(acc);
			}
			/*
			// Ограничение вида fuel1^pow3 = fuel2^pow4
			// сделаем-ка два неравенства
			// Fuel1^pow3 <= Fuel2^pow4
			acc = new Chamber();
			acc.isMaster = false;
			acc.lower = Enumerable.Repeat<int>(1, pows[3]).ToArray();
			acc.upper = Enumerable.Repeat<int>(2, pows[4]).ToArray();
			chambers.Add(acc);
			// Fuel2^pow4 <= Fuel1^pow3
			acc = new Chamber();
			acc.isMaster = true;
			acc.upper = Enumerable.Repeat<int>(1, pows[3]).ToArray();
			acc.lower = Enumerable.Repeat<int>(2, pows[4]).ToArray();
			chambers.Add(acc);
			*/

			return new Car(chambers.ToArray(), 6, fuels);

		}
		/// <summary>
		/// Заполяем массив одним значением
		/// </summary>
		/// <typeparam name="T">тип массива</typeparam>
		/// <param name="array">массив</param>
		/// <param name="element">значение</param>
		private void  FillArray<T>(T[] array, T element)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = element;
			}
		}
	}
}
