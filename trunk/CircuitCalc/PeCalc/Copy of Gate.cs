using System;
using System.Collections.Generic;
using CircuitCalc.Input;

namespace CircuitCalc.PeCalc
{
	class Gate
	{
		public Gate()
		{
			lout = () => L(lin(), rin());
			rout = () => R(lin(), rin());
		}

		public Func<int> lin, rin;
		public readonly Func<int> lout, rout;

		public int R(int leftIn, int rightIn)
		{
			return (2 + (leftIn * leftIn)) % 3;
		}

		public int L(int leftIn, int rightIn)
		{
			return (leftIn * (1 + rightIn + rightIn * rightIn) + 2 * rightIn) % 3;
		}

		public void SetIn(char side, Func<int> get)
		{
			if(side == 'R') rin = get;
			else if(side == 'L') lin = get;
			else throw new Exception("Unknown side " + side);
		}

		public Func<int> GetOut(char side)
		{
			if(side == 'R') return rout;
			if(side == 'L') return lout;
			throw new Exception("Unknown side " + side);
		}
	}

	class Calculator : ISink
	{
		private readonly string input;
		public Dictionary<int, Gate> gates = new Dictionary<int, Gate>();
		private Func<int> outputChar;
		private int position;

		public Calculator(string filename, string input)
		{
			this.input = input;
			new Parser().Parse(filename, this);
		}

		public int Next()
		{
			return outputChar();
		}

		public void OnGate(int gateIndex, Point leftIn, Point rightIn, Point leftOut, Point rightOut)
		{
			Func<int> theOut = GetTheOut(leftIn, gateIndex);
			GetGate(gateIndex).SetIn('L', theOut);

			theOut = GetTheOut(rightIn, gateIndex);
			GetGate(gateIndex).SetIn('R', theOut);
		}

		private Func<int> GetTheOut(Point leftIn, int gateIndex)
		{
			if(leftIn.gate < 0) return ReadNext;
			Func<int> theOut = GetGate(leftIn.gate).GetOut(leftIn.side);
			if(leftIn.gate <= gateIndex) theOut = new BackWire(theOut).Get;
			return theOut;
		}

		public void OnInput(Point to)
		{
			GetGate(to.gate).SetIn(to.side, ReadNext);
		}

		private int ReadNext()
		{
			if (position >= input.Length) return 0;
			return input[position++] - '0';
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
			outputChar = GetGate(from.gate).GetOut(from.side);
		}
	}

	public class BackWire
	{
		private int mem = 0;
		public Func<int> get;

		public BackWire(Func<int> get)
		{
			this.get = get;
		}

		public int Get()
		{
			int result = mem;
			mem = get();
			return result;
		}
	}
}
