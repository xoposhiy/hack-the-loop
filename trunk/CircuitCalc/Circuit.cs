using System.Collections.Generic;

namespace CircuitCalc
{
	internal enum Side
	{
		L, R
	}
	
	class Node
	{
		public int index;
		public KeyValuePair<Node, Side> nextLeft;
		public KeyValuePair<Node, Side> nextRight;
		private int leftRegister;
		private int rightRegister;
	}

	class Circuit
	{
		public Node X;
		public List<Node> Nodes;
	}
}