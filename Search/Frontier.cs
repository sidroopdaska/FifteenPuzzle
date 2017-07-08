using System;
using System.Collections.Generic;
using System.Linq;

namespace Search
{
	#region Frontier

	public class Frontier
	{
		private readonly NodeComparator _compareNodes;
		private readonly IDictionary<StateBase, Node> _hashByState;
		private readonly SortedList<Key, Node> _pQueue;

		public Frontier(NodeComparator compareNodes)
		{
			_compareNodes = compareNodes;
			_hashByState = new Dictionary<StateBase, Node>();
			_pQueue = new SortedList<Key, Node>();
		}

		public int Count { get; private set; }

		public void Push(Node node)
		{
			Key key = new Key(node, _compareNodes);

			// add to priority queue
			if (!_pQueue.ContainsKey(key))
			{
				_pQueue.Add(key, node);
				Count++;
			}

			// add to state hash
			if (!_hashByState.ContainsKey(node.State))
			{
				_hashByState[node.State] = node;
			}

		}

		public Node Pop()
		{
			if (Count == 0)
			{
				return null;
			}

			Count--;

			// pop from priority queue
			Node node = _pQueue.First().Value;
			_pQueue.RemoveAt(0);

			// clean up state hash
			_hashByState.Remove(node.State);

			return node;
		}

		public void Replace(Node inFrontier, Node newNode)
		{
			_hashByState.Remove(inFrontier.State);
			_hashByState[newNode.State] = newNode;

			Key oldKey = new Key(inFrontier, _compareNodes);
			if (_pQueue.ContainsKey(oldKey))
			{
				_pQueue.Remove(oldKey);
			}

			Key newKey = new Key(newNode, _compareNodes);
			if (!_pQueue.ContainsKey(newKey))
			{
				_pQueue.Add(newKey, newNode);
			}
		}

		public Node Find(StateBase state)
		{
			return _hashByState.ContainsKey(state) ? _hashByState[state] : null;
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
			return _compareNodes(_node, other._node);
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
