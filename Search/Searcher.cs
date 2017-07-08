using System;
using System.Collections;
using System.Linq;

namespace Search
{
	public delegate int CostFunc(StateBase from, StateBase to);

	public delegate int NodeComparator(Node node1, Node node2);

	public class Searcher
	{
		private readonly StateBase _initialState;
		private readonly StateBase _goalState;

		public Searcher(StateBase initialState, StateBase goalState)
		{
			_initialState = initialState;
			_goalState = goalState;
		}

		//public SearchResult BFS()
		//{

		//}

		//public SearchResult DFS()
		//{

		//}
		
		public void GenericSearch()
		{

		}
	}
}
