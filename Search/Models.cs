using System;
using System.Collections.Generic;

namespace Search
{
	public class Node
	{
		public Node(StateBase state, CostFunc cost, string action, Node parent, Func<StateBase, int> hHatGetter)
		{
			State = state;
			Action = action;
			Parent = parent;
			HHat = hHatGetter(state);
			if (parent != null)
			{
				GHat = parent.GHat + cost(parent.State, state);
				Depth = Parent.Depth + 1;
			}
		}

		public StateBase State { get; set; }
		public string Action { get; set; }
		public Node Parent { get; set; }
		public int Depth { get; set; }
		public int GHat { get; set; }
		public int HHat { get; set; }
		public int FHat { get { return GHat + HHat; } }

		public IEnumerable<Node> Successors(CostFunc costFunc, Func<StateBase, int> hHatGetter)
		{
			return 
		}
	}

	public abstract class StateBase
	{

	}

	public class SearchResult
	{
		public SearchResult()
	}
}
