using System;
using System.Collections.Generic;
using Search;

namespace FifteenPuzzle
{
	public struct PuzzleSpace
	{
		public int Row { get; private set; }
		public int Col { get; private set; }

		public PuzzleSpace(int row, int col)
		{
			Row = row;
			Col = col;
		}
	}

	public class PuzzleState : StateBase
	{
		private Dictionary<int, PuzzleSpace> _valueLookup;

		public int[,] Board { get; private set; }

		public PuzzleState(int[,] board, Dictionary<int, PuzzleSpace> valueLookup)
		{
			Board = board;
			_valueLookup = valueLookup;
		}

		public PuzzleState(string stateAsString)
		{
			// example stateAsString:
			// ((1 5 3 7) (4 9 2 11) (8 13 10 14) (12 15 0 6) (3 2))

			Board = new int[4,4];
			_valueLookup = new Dictionary<int, PuzzleSpace>();

			string[] parts = stateAsString.Split(new[] { ") (" }, StringSplitOptions.None);

			for (int i = 0; i < 4; i++)
			{
				string part = parts[i].TrimStart(new[] { '(', ' ' }).TrimEnd(new[] { ')', ' ' });
				string[] nums = part.Split();

				for (int j = 0; j < 4; j++)
				{
					int num = int.Parse(nums[j]);
					Board[i, j] = num;
					_valueLookup[num] = new PuzzleSpace(i, j);

				}
			}
		}

		#region equality

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals(obj as PuzzleState);
		}

		public override bool Equals(StateBase other)
		{
			return other.GetHashCode().Equals(GetHashCode());
		}

		#endregion

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;

				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						hash = hash * 397 + Board[i, j] ^ 23 + (i + j) ^ 19;
					}
				}

				return hash;		
			}
		}

		public PuzzleSpace GetSpace(int num)
		{
			return _valueLookup[num];
		}

		public PuzzleState Clone()
		{
			Dictionary<int, PuzzleSpace> valueLookup = new Dictionary<int, PuzzleSpace>(16);
			int[,] newBoard = new int[4, 4];
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					int num = Board[i, j];
					newBoard[i, j] = num;
					valueLookup[num] = new PuzzleSpace(i, j);
				}
			}

			return new PuzzleState(newBoard, valueLookup);
		}

		#region Actions

		public PuzzleState Up()
		{
			
			PuzzleSpace oldVoidLocation = GetSpace(0);

			if (oldVoidLocation.Row == 0)
			{
				return null;
			}

			PuzzleState newState = Clone();
			PuzzleSpace newVoidLocation = new PuzzleSpace(oldVoidLocation.Row - 1, oldVoidLocation.Col);

			int valToBeSwapped = newState.Board[newVoidLocation.Row, newVoidLocation.Col];
			newState.Board[newVoidLocation.Row, newVoidLocation.Col] = 0;
			newState.Board[oldVoidLocation.Row, oldVoidLocation.Col] = valToBeSwapped;
			newState._valueLookup[0] = newVoidLocation;
			newState._valueLookup[valToBeSwapped] = oldVoidLocation;
			return newState;
		}

		public PuzzleState Down()
		{
			PuzzleSpace oldVoidLocation = GetSpace(0);

			if (oldVoidLocation.Row == 3)
			{
				return null;
			}

			PuzzleState newState = Clone();
			PuzzleSpace newVoidLocation = new PuzzleSpace(oldVoidLocation.Row + 1, oldVoidLocation.Col);

			int valToBeSwapped = newState.Board[newVoidLocation.Row, newVoidLocation.Col];
			newState.Board[newVoidLocation.Row, newVoidLocation.Col] = 0;
			newState.Board[oldVoidLocation.Row, oldVoidLocation.Col] = valToBeSwapped;
			newState._valueLookup[0] = newVoidLocation;
			newState._valueLookup[valToBeSwapped] = oldVoidLocation;
			return newState;

		}

		public PuzzleState Left()
		{
			PuzzleSpace oldVoidLocation = GetSpace(0);

			if (oldVoidLocation.Col == 0)
			{
				return null;
			}

			PuzzleState newState = Clone();

			PuzzleSpace newVoidLocation = new PuzzleSpace(oldVoidLocation.Row, oldVoidLocation.Col - 1);

			int valToBeSwapped = newState.Board[newVoidLocation.Row, newVoidLocation.Col];
			newState.Board[newVoidLocation.Row, newVoidLocation.Col] = 0;
			newState.Board[oldVoidLocation.Row, oldVoidLocation.Col] = valToBeSwapped;
			newState._valueLookup[0] = newVoidLocation;
			newState._valueLookup[valToBeSwapped] = oldVoidLocation;
			return newState;

		}

		public PuzzleState Right()
		{
			PuzzleSpace oldVoidLocation = GetSpace(0);

			if (oldVoidLocation.Col == 3)
			{
				return null;
			}

			PuzzleState newState = Clone();
			PuzzleSpace newVoidLocation = new PuzzleSpace(oldVoidLocation.Row, oldVoidLocation.Col + 1);

			int valToBeSwapped = newState.Board[newVoidLocation.Row, newVoidLocation.Col];
			newState.Board[newVoidLocation.Row, newVoidLocation.Col] = 0;
			newState.Board[oldVoidLocation.Row, oldVoidLocation.Col] = valToBeSwapped;
			newState._valueLookup[0] = newVoidLocation;
			newState._valueLookup[valToBeSwapped] = oldVoidLocation;
			return newState;

		}

		#endregion

		public override IDictionary<string, StateBase> Successors()
		{
			Dictionary<string, StateBase> dict = new Dictionary<string, StateBase>();
			var u = Up();
			var d = Down();
			var l = Left();
			var r = Right();

			if (u != null)
			{
				dict["U"] = u;
			}

			if (d != null)
			{
				dict["D"] = d;
			}

			if (l != null)
			{
				dict["L"] = l;
			}

			if (r != null)
			{
				dict["R"] = r;
			}

			return dict;
		}
	}
}
