using System.Collections.Generic;

namespace Search
{
	public class Explored
	{
		private readonly Dictionary<StateBase, Node> _hashByState = new Dictionary<StateBase, Node>();

		public int Count {get; private set; }

		public void Push(Node node)
		{
			if (!_hashByState.ContainsKey(node.State))
			{
				_hashByState[node.State] = node;
				Count++;
			}
		}

		public Node Find(StateBase state)
		{
			return state != null && _hashByState.ContainsKey(state) ? _hashByState[state] : null;	
		}

		public void Remove(Node node)
		{
			if (_hashByState.ContainsKey(node.State))
			{
				_hashByState.Remove(node.State);
			}
		}

	}
}
