﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CircuitCalc.TParsing;
using NUnit.Framework;
using CarlJohansen;

namespace CircuitCalc.CarCreator
{
	class CreationTesting
	{

		public string car46318 = "22102200010221100101022111111111110222200011110011112222000111110100002222002001111001111";
		public string car12785 = "2222001100222212212201010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101110222212212211212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212200002222122122222001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012222000010111222000220011022220000101112220002200122221221221101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010222212212221212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212121212101112222122200022001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200122001220012200110122000";
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
		[Test]
		public void Decode4618()
		{
			TParser parser = new TParser();
			var car = parser.ParseCar(car12785);
			Console.WriteLine(car);

		}
		[Test]
		public void CompareTest()
		{
//			Console.WriteLine(String.Compare("02", "1", StringComparison.Ordinal));
			var bad = "2222000220010010220101201102201122001011122012220000112220220001101220002202200100122001";
			var good = "2222000220022001010220102200001102201112011122012100112220220000012200022022001110122001";

			Console.WriteLine(String.Compare(bad, good) >= 0);
		}

		[Test]
		public void Factory()
		{
			var fact = new CarFactory();
			var car = fact.GetCarLoopWithLenearStricts(2, 3, 4, 6, 4);
			//Console.WriteLine(car);

			var enc = new TEncoder();
			var parcer = new TParser();
			var chmrs = car.GetChambers();
//			Console.WriteLine(enc.EncodeCar(chmrs));
			car.Normalize();
//			Console.WriteLine(car.IsConnected());
			chmrs = car.GetChambers();
			var code = enc.EncodeCar(chmrs);
			Console.WriteLine(code);
			parcer.ParseCar(code);

			Console.WriteLine(CircuitBuilding.Builder.BuildFactory(enc.EncodeFuel(car.GetFuel())));
		}
	}
}
