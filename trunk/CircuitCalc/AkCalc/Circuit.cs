using System;
using System.Collections.Generic;

namespace CircuitCalc.AkCalc
{
	internal interface ILink
	{
		int GetResult();
	}

	internal interface INode
	{
		int GetOldLeftOutput();
		int GetOldRightOutput();
		int GetNewLeftOutput();
		int GetNewRightOutput();
	}

	class NodeX : INode
	{
		public ILink Link { get; set; }

		public int GetOldLeftOutput()
		{
			return Output;
		}

		public int GetOldRightOutput()
		{
			throw new InvalidOperationException();
		}

		public int GetNewLeftOutput()
		{
			return Output;
		}

		public int GetNewRightOutput()
		{
			throw new InvalidOperationException();
		}

		public int Output { get; set; }
	}

	class Node : INode
	{
		public ILink LeftLink { get; set; }
		public ILink RightLink { get; set; }
		
		public bool Visited { private get; set; }

		public int LeftOutput { get; set; }
		public int RightOutput { get; set; }

		public int NewLeftOutput { get; private set; }
		public int NewRightOutput { get; private set; }

		public int GetOldLeftOutput()
		{
			GetResults();
			return LeftOutput;
		}

		public int GetOldRightOutput()
		{
			GetResults();
			return RightOutput;
		}

		public int GetNewLeftOutput()
		{
			GetResults();
			return NewLeftOutput;
		}

		public int GetNewRightOutput()
		{
			GetResults();
			return NewRightOutput;
		}

		private void GetResults()
		{
			if (!Visited)
			{
				var leftInput = LeftLink.GetResult();
				var rightInput = RightLink.GetResult();
				NewLeftOutput = L(leftInput, rightInput);
				NewRightOutput = R(leftInput, rightInput);
				Visited = true;
			}
		}

		private static int L(int l, int r)
		{
			int res = (l * (1 + r + r * r) + 2 * r) % 3;
			return res;
		}

		private static int R(int l, int r)
		{
			int res = (2 + (l * r)) % 3;
			return res;
		}
	}

	abstract class Link : ILink
	{
		public INode Owner { get; set; }

		public abstract int GetResult();
	}

	class LeftFwdLink : Link
	{
		public override int GetResult()
		{
			return Owner.GetNewLeftOutput();
		}
	}

	class RightFwdLink : Link
	{
		public override int GetResult()
		{
			return Owner.GetNewRightOutput();
		}
	}

	class LeftBwdLink : Link
	{
		public override int GetResult()
		{
			return Owner.GetOldLeftOutput();
		}
	}

	class RightBwdLink : Link
	{
		public override int GetResult()
		{
			return Owner.GetOldRightOutput();
		}
	}

	internal enum Side
	{
		L, R
	}

	class Circuit
	{
		public NodeX X;
		public List<Node> Nodes;
	}
}
