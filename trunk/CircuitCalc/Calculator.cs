using System;
using System.Collections.Generic;
using System.Linq;
using Enumerable = System.Linq.Enumerable;

namespace CircuitCalc
{
	class Calculator
	{
		public void Run(string circuitString, string inputString)
		{
			var circuit = ReadCircuit(circuitString);
			var input = ReadInput(inputString);
			Calc(circuit, input);
		}

		private static void Calc(Circuit circuit, IEnumerable<byte> input)
		{
		}

		private static IEnumerable<byte> ReadInput(string input)
		{
			return Enumerable.ToList<byte>(input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(c => byte.Parse(c)));
		}

		private static Circuit ReadCircuit(string circuit)
		{
			var parts = circuit.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			var nodes = parts.Skip(1).First().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var node in nodes)
			{
				
			}
			return null;
		}
	}
}