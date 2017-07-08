using System;
using System.Collections.Generic;

namespace Search
{
	public class HeuristicCache
	{
		private readonly IDictionary<StateBase, int> _cache;
		private readonly IHeuristic _heuristic;
		private readonly StateBase _goal;

		public HeuristicCache(StateBase goal, IHeuristic heuristic)
		{
			_goal = goal;
			_heuristic = heuristic;
			_cache = new Dictionary<StateBase, int>();
		}

		public int Evaluate(StateBase state)
		{
			if (_goal == null || state == null || _heuristic == null)
			{
				return 0;
			}

			if (_cache.ContainsKey(state))
			{
				return _cache[state];
			}

			int result = _heuristic.Evaluate(state, _goal);
			_cache[state] = result;
			return result;
		}
	}
}
