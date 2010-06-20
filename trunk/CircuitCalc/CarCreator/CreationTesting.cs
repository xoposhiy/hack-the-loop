using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CircuitCalc.TParsing;
using NUnit.Framework;

namespace CircuitCalc.CarCreator
{
	class CreationTesting
	{
		// Кольцевая машинка
		private Chamber[] car1 = new Chamber[]
		                         	{
		                         		new Chamber() {isMaster = true, lower = new int[] {0}, upper = new int[] {0, 1}},
		                         		new Chamber() {isMaster = true, lower = new int[] {1}, upper = new int[] {1, 2}},
		                         		new Chamber() {isMaster = true, lower = new int[] {2}, upper = new int[] {2, 3}},
		                         		new Chamber() {isMaster = true, lower = new int[] {3}, upper = new int[] {3, 4}},
		                         		new Chamber() {isMaster = true, lower = new int[] {4}, upper = new int[] {4, 5}},
		                         		new Chamber() {isMaster = true, lower = new int[] {5}, upper = new int[] {5, 0}},
		                         	};
		// Топливо ? подойдут 6 любых матриц, которые не уменьшают
		private Matrix[] fuel1 = new Matrix[]
		                         	{
		                         		new Matrix(new int[][] {new int[] {2}}),
		                         		new Matrix(new int[][] {new int[] {2}}),
		                         		new Matrix(new int[][] {new int[] {2}}),
		                         		new Matrix(new int[][] {new int[] {2}}),
		                         		new Matrix(new int[][] {new int[] {2}}),
		                         		new Matrix(new int[][] {new int[] {2}}),
		                         	}
		                ;
		TEncoder encoder = new TEncoder();
		[Test]
		public void SimpleCar()
		{
			Console.WriteLine(encoder.EncodeCar(car1));
			Console.WriteLine(CircuitBuilding.Builder.BuildFactory(encoder.EncodeFuel(fuel1)));
		}

	}
}
