using System;
using System.Collections.Generic;
using System.Linq;

namespace CircuitCalc.AkCalc
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
			foreach (var b in input)
			{
				circuit.X.Output = b;
				var res = circuit.X.Link.GetResult();
				Console.Write(res);
				NextStep(circuit);
			}
		}

		private static void NextStep(Circuit circuit)
		{
			foreach(var node in circuit.Nodes)
			{
				node.Visited = false;
				node.LeftOutput = node.NewLeftOutput;
				node.RightOutput = node.NewRightOutput;
			}
		}

		private static IEnumerable<byte> ReadInput(string input)
		{
			return input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(c => byte.Parse(c)).ToList();
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