using System;

namespace Search
{
	public abstract class HeuristicBase<T>: IHeuristic where T: StateBase
	{
		public abstract string Name { get; }
		public abstract int Evaluate(T state, T goalState);

		public int Evaluate(StateBase state, StateBase goalState)
		{
			return Evaluate(state as T, goalState as T);
		}
	}
}
