using System;
using System.Collections.Generic;
using System.Linq;

namespace Search
{
	#region Frontier

	public class Frontier
	{
		private readonly NodeComparator _compareNodes;
		private readonly IDictionary<StateBase, List<Node>> _hashByState;
		private readonly SortedList<Key, List<Node>> _pQueue;

		public Frontier(NodeComparator compareNodes)
		{
			_compareNodes = compareNodes;
			_hashByState = new Dictionary<StateBase, List<Node>>();
			_pQueue = new SortedList<Key, List<Node>>();
		}

		public int Count { get; private set; }

		public void Push(Node node)
		{
			Key key = new Key(node, _compareNodes);

			Count++;

			// add to priority queue
			if (!_pQueue.ContainsKey(key))
			{
				_pQueue[key] = new List<Node>();
			}
			_pQueue[key].Add(node);

			// add to state hash
			if (!_hashByState.ContainsKey(node.State))
			{
				_hashByState[node.State] = new List<Node>();
			}

			_hashByState[node.State].Add(node);
		}

		public Node Pop()
		{
			if (Count == 0)
			{
				return null;
			}

			Count--;

			// pop from priority queue
			List<Node> nodes = _pQueue.First().Value;
			Node node = nodes.First();

			if (nodes.Count > 1)
			{
				nodes.RemoveAt(0);
			}
			else
			{
				_pQueue.RemoveAt(0);	
			}

			// clean up state hash
			_hashByState[node.State].Remove(node);

			return node;
		}

		public void Replace(Node inFrontier, Node newNode)
		{
			_hashByState[inFrontier.State].Remove(inFrontier);
			_hashByState[newNode.State].Add(newNode);

			Key oldKey = new Key(inFrontier, _compareNodes);
			if (_pQueue.ContainsKey(oldKey))
			{
				_pQueue[oldKey].Remove(inFrontier);
			}

			Key newKey = new Key(newNode, _compareNodes);
			if (!_pQueue.ContainsKey(newKey))
			{
				_pQueue[newKey] = new List<Node>();
			}
			_pQueue[newKey].Add(newNode);
		}

		public Node Find(StateBase state)
		{
			return _hashByState.ContainsKey(state) && _hashByState[state].Count > 0 ? _hashByState[state].First() : null;
		}
	}

	#endregion Frontier

	#region Key

	public class Key: IComparable<Key>, IEquatable<Key>
	{
		private readonly Node _node;
		private readonly NodeComparator _compareNodes;

		public Key(Node node, NodeComparator comparator)
		{
			_node = node;
			_compareNodes = comparator;
		}

		public int CompareTo(Key other)
		{
			int val = _compareNodes(_node, other._node);
			return val;
		}

		public bool Equals(Key other)
		{
			return _node.State.Equals(other._node.State);
		}

		// Overrides Object.Equals() function
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals(obj as Key);
		}

		public override int GetHashCode()
		{
			return _node.State.GetHashCode();
		}
	}

	#endregion Key
}
