using System;

namespace Search
{
	public interface IHeuristic
	{
		string Name { get; }
		int Evaluate(StateBase state, StateBase goal);
	}
}
