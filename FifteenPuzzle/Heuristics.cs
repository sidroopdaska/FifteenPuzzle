using System;
using System.Collections.Generic;
using System.Linq;
using Search;

namespace FifteenPuzzle
{
	public class ManhattanDistanceHeuristic : HeuristicBase<PuzzleState>
	{
		public override string Name { get { return "Manhattan"; } }

		public override int Evaluate(PuzzleState state, PuzzleState goalState)
		{
			var manhattanDistanceOff = 0;

			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					int val = state.Board[i, j];

					if (val == 0)
					{
						continue;
					}

					var correctPlace = goalState.GetSpace(val);
					manhattanDistanceOff += ManhattanDistance(i, j, correctPlace.Row, correctPlace.Col);
				}
			}

			return manhattanDistanceOff;
		}

		public static int ManhattanDistance(int fromRow, int fromCol, int toRow, int toCol)
		{
			return Math.Abs(fromRow - toRow) + Math.Abs(fromCol - toCol);
		}
	}

	public class ManhanttanDistanceWithLinearConflictHeuristic: HeuristicBase<PuzzleState>
	{
		public override string Name { get { return "Manhattan + Linear Conflict"; }}

		public override int Evaluate(PuzzleState state, PuzzleState goalState)
		{
			int manhanttanDist = 0;

			var possibleRowConflicts = new Dictionary<int, List<Tuple<int, int>>>();
			var possibleColConflicts = new Dictionary<int, List<Tuple<int, int>>>();

			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					int val = state.Board[i, j];
					if (val == 0)
					{
						continue;
					}

					var correctPos = goalState.GetSpace(val);
					manhanttanDist += ManhattanDistanceHeuristic.ManhattanDistance(i, j, correctPos.Row, correctPos.Col);

					// check for linear conflicts in the horizontal + vertical direction

					if (i == correctPos.Row && j != correctPos.Col)
					{
						if (!possibleRowConflicts.ContainsKey(i))
						{
							possibleRowConflicts[i] = new List<Tuple<int, int>>();
						}

						possibleRowConflicts[i].Add(Tuple.Create(j, correctPos.Col));
					}

					if (j == correctPos.Col && i != correctPos.Row)
					{
						if (!possibleColConflicts.ContainsKey(j))
						{
							possibleColConflicts[j] = new List<Tuple<int, int>>();
						}

						possibleColConflicts[j].Add(Tuple.Create(i, correctPos.Row));
					}

				}
			}

			// Calculate the # of linear conflicts

			int linearConflicts = 0;

			foreach (List<Tuple<int, int>> possibleConflicts in possibleRowConflicts.Values.Concat(possibleColConflicts.Values))
			{
				Tuple<int, int> conflict = possibleConflicts.First();

				linearConflicts += possibleConflicts.Skip(1).Count(x => (x.Item1 > conflict.Item1 && x.Item2 < conflict.Item2) ||
																  (x.Item1 < conflict.Item1 && x.Item2 > conflict.Item2));
			}

			return manhanttanDist + 2 * linearConflicts;

		}
	}
}
