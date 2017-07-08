using System;
using System.Collections.Generic;
using System.Linq;

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
			return State.Successors().Select(x => new Node(x.Value, costFunc, x.Key, this, hHatGetter));
		}
	}

	public abstract class StateBase: IEquatable<StateBase>
	{
		public abstract bool Equals(StateBase other);
		public abstract override bool Equals(object obj);
		public abstract override int GetHashCode();
		public abstract IDictionary<string, StateBase> Successors();

	}

	public class SearchResult
	{
		public SearchResult(Node resultNode, int nodesGen, int nodesPrevGen, int nodesOnFrontier,
						   int nodesOnExplored, string algName, IHeuristic heuristic)
		{
			ResultNode = resultNode;
			NodesGen = nodesGen;
			NodesPrevGen = nodesPrevGen;
			NodesOnFrontier = nodesOnFrontier;
			NodesOnExplored = nodesOnExplored;
			AlgName = algName;
			HeuristicName = heuristic?.Name;

		}

		public Node ResultNode { get; private set; }
		public int NodesGen { get; private set; }
		public int NodesPrevGen { get; private set; }
		public int NodesOnFrontier { get; private set; }
		public int NodesOnExplored { get; private set; }
		public string AlgName { get; private set; }
		public string HeuristicName { get; private set; }

		public override string ToString()
		{
			string name = AlgName + (HeuristicName == null ? ":" : "(" + HeuristicName + "):");

			Stack<string> moves = new Stack<string>();
			Node node = ResultNode;

			while (node != null)
			{
				if (node.Action != null)
				{
					moves.Push(node.Action);
				}
				node = node.Parent;
			}

			string movesString = moves.Count == 0 ? "FAIL" : string.Join(" ", moves);

			string result = $"Moves: ({movesString}), NodesGen: {NodesGen}, NodesPrevGen: {NodesPrevGen}, NodesOnFrontier: {NodesOnFrontier}, " +
				$"NodesOnExplored: {NodesOnExplored}";
			
			return string.Format("{0}{1}{2}", name, Environment.NewLine, result);
		}
	}
}
