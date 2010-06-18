using System;
using System.Collections.Generic;
using CircuitCalc.Input;

namespace CircuitCalc.PeCalc
{
	class Gate
	{
		public Gate()
		{
			lin = c =>
			      	{
			      		fromL = c;
			      		TryPush();
			      	};
			rin = c =>
			       	{
			       		fromR = c;
			       		TryPush();
			       	};
		}

		private void TryPush()
		{
			if(fromR >= 0 && fromL >= 0)
			{
				lout(L(fromL, fromR));
				rout(R(fromL, fromR));
			}
		}

		public void NextStep()
		{
			fromR = -1;
			fromL = -1;
		}

		public int fromR, fromL;
		public readonly Action<int> lin, rin;
		public Action<int> lout, rout;

		public int R(int leftIn, int rightIn)
		{
			int res = (2 + (leftIn * rightIn)) % 3;
			//Console.WriteLine("R({0}, {1}) = {2}", leftIn, rightIn, res);
			return res;
		}

		public int L(int leftIn, int rightIn)
		{
			int res = (leftIn * (1 + rightIn + rightIn * rightIn) + 2 * rightIn) % 3;
			//Console.WriteLine("L({0}, {1}) = {2}", leftIn, rightIn, res);
			return res;
		}

		public void SetOut(char side, Action<int> push)
		{
			if(side == 'R') rout = push;
			else if(side == 'L') lout = push;
			else throw new Exception("Unknown side " + side);
		}

		public Action<int> GetIn(char side)
		{
			if(side == 'R') return rin;
			if(side == 'L') return lin;
			throw new Exception("Unknown side " + side);
		}
	}

	class Calculator : ISink
	{
		private readonly string input;
		public Dictionary<int, Gate> gates = new Dictionary<int, Gate>();
		public IList<BackWire> backwires = new List<BackWire>();
		private int position;
		private Action<int> pushChar;

		public Calculator(string filename, string input)
		{
			this.input = input;
			new Parser().Parse(filename, this);
		}

		public void PushNext()
		{
			foreach(var gate in gates.Values)
				gate.NextStep();
			foreach(var backWire in backwires)
				backWire.NextStep();
			int ch = (position >= input.Length ? '0' : input[position++]) - '0';
			//Console.WriteLine("input gives " + ch);
			pushChar(ch);
		}

		public void OnGate(int gateIndex, Point leftIn, Point rightIn, Point leftOut, Point rightOut)
		{
			Action<int> theIn = GetTheIn(leftOut, gateIndex);
			GetGate(gateIndex).SetOut('L', theIn);

			theIn = GetTheIn(rightIn, gateIndex);
			GetGate(gateIndex).SetOut('R', theIn);
		}

		private Action<int> GetTheIn(Point point, int gateIndex)
		{
			if(point.gate < 0) return OutputChar;
			Action<int> theIn = GetGate(point.gate).GetIn(point.side);
			if(point.gate <= gateIndex)
			{
				var backWire = new BackWire(theIn);
				backwires.Add(backWire);
				theIn = backWire.Push;
			}
			return theIn;
		}

		public void OnInput(Point to)
		{
			pushChar = GetGate(to.gate).GetIn(to.side);
		}

		private Gate GetGate(int index)
		{
			Gate gate;
			if (!gates.TryGetValue(index, out gate))
			{
				gate = new Gate();
				gates[index] = gate;
			}
			return gate;
		}

		public void OnOutput(Point from)
		{
			GetGate(from.gate).SetOut(from.side, OutputChar);
		}

		private static void OutputChar(int ch)
		{
			Console.Write(ch);
		}
	}

	public class BackWire
	{
		private int mem;
		public Action<int> push;

		public BackWire(Action<int> push)
		{
			this.push = push;
		}

		public void Push(int ch)
		{
			mem = ch;
		}

		public void NextStep()
		{
			push(mem);
			//Console.WriteLine("backwire gives " + mem);
		}
	}
}
