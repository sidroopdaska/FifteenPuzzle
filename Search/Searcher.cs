﻿using System;
using System.Collections.Generic;
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

		public SearchResult BFS()
		{
			NodeComparator comparator = (node1, node2) => node1.Depth.CompareTo(node2.Depth);
			return GenericSearch(_initialState, _goalState, "BFS", comparator, (state1, state2) => 1, null);
		}

		public SearchResult DFS(int depthLimit)
		{
			string name = "DFS (DepthLimit: " + depthLimit + ")";
			NodeComparator comparator = (node1, node2) => (-node1.Depth).CompareTo(-node2.Depth);
			return GenericSearch(_initialState, _goalState, name, comparator, (state1, state2) => 1, null, depthLimit);
		}

		public SearchResult UCS()
		{
			NodeComparator comparator = (node1, node2) => node1.GHat.CompareTo(node2.GHat);
			CostFunc cost = (state1, state2) => 1;
			return GenericSearch(_initialState, _goalState, "UCS", comparator, cost, null);
		}

		public SearchResult GenericSearch(StateBase initialState,
		                                  StateBase goalState,
										  string algName,
		                                  NodeComparator comparator,
										  CostFunc cost,
		                                  IHeuristic heuristic,
		                                  int? depthLimit = null)
		{

			Frontier frontier = new Frontier(comparator);
			Explored explored = new Explored();
			HeuristicCache cache = new HeuristicCache(goalState, heuristic);
			int nodesGenerated = 0;
			int nodesPrevGenerated = 0;

			Node initNode = new Node(initialState, cost, null, null, cache.Evaluate);
			frontier.Push(initNode);

			while (true)
			{
				if (frontier.Count == 0)
				{
					return new SearchResult(null, nodesGenerated, nodesPrevGenerated, 0, explored.Count, algName, heuristic);
				}

				Node node = frontier.Pop();
				explored.Push(node);

				// goal test
				if (node.State.Equals(goalState))
				{
					return new SearchResult(node, nodesGenerated, nodesPrevGenerated, frontier.Count, explored.Count, algName, heuristic);
				}

				if (depthLimit != null && node.Depth == depthLimit)
				{
					continue;
				}

				IEnumerable<Node> children = node.Successors(cost, cache.Evaluate);
				foreach(Node child in children)
				{
					nodesGenerated++;

					// If this state is found in the Frontier, replace the old state with the new state if its GHat is smaller
					Node foundInFrontier = frontier.Find(child.State);
					if (foundInFrontier != null)
					{
						nodesPrevGenerated++;
						if (foundInFrontier.GHat > child.GHat)
						{
							frontier.Replace(foundInFrontier, child);
						}
						
					}
					else
					{
						Node foundInExplored = explored.Find(child.State);

						// If this state is found in the Explored, replace the old state with the new state if its GHat is smaller
						if (foundInExplored != null)
						{
							nodesPrevGenerated++;
							if (foundInExplored.GHat > child.GHat)
							{
								explored.Remove(foundInExplored);
								frontier.Push(child);
							}	
						}
						else
						{
							// doesn't exist in frontier or explored, adding to frontier.
							frontier.Push(child);
						}
					}
				}
			}
		}
	}
}
