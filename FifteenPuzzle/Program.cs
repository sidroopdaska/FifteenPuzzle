using System;
using System.Collections.Generic;
using System.Threading;
using Search;

namespace FifteenPuzzle
{	
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				// No arguments mean run all test cases
				if(args.Length == 0)
				{
					RunAllTestCases();
				}
				// 2 arguments means <init_state> <goal_state>
				else if (args.Length == 2)
				{
					RunSingleSearch(args[0], args[1]);
				}

				//AskForInput();
			}
			catch(Exception e)
			{
				Console.WriteLine("Something seriously went wrong!");
				Console.WriteLine(e.Message);
			}
		}

		// Runs easy/med/hard tests against the two goal states
		private static void RunAllTestCases()
		{
			Console.WriteLine("**********************");
			Console.WriteLine("RUNNING ALL TEST CASES");
			Console.WriteLine("**********************");

			const string goalState1 = "((0 1 2 3) (4 5 6 7) (8 9 10 11) (12 13 14 15) (0 0))";
			const string easyInitState = "((1 2 3 0) (4 5 6 7) (8 9 10 11) (12 13 14 15) (0 3))";
			//const string medInitState = "((1 2 6 3) (4 5 10 7) (0 9 14 11) (8 12 13 15) (2 0))";
			//const string hardInitState = "((1 2 3 7) (4 5 6 15) (8 9 11 0) (12 13 14 10) (2 3))";

			Console.WriteLine("\nGoal State 1: " + goalState1);
			Console.WriteLine("EASY Init State: " + easyInitState + "\n");
			RunSingleSearch(easyInitState, goalState1);

		}

		private static void RunSingleSearch(string initStateString, string goalStateString)
		{
			PuzzleState initState = new PuzzleState(initStateString);
			PuzzleState goalState = new PuzzleState(goalStateString);
	
			RunSingleSearch(initState, goalState);
		}

		private static void RunSingleSearch(PuzzleState initState, PuzzleState goalState)
		{
			Searcher searcher = new Searcher(initState, goalState);
			CostFunc cost = (state1, state2) => 1;

			List<Thread> searchThreads = new List<Thread>()
			{
				new Thread(() => Console.WriteLine(searcher.BFS().ToString()))
			};

			searchThreads.ForEach(x => x.Start());

			searchThreads.ForEach(x => x.Join());
		}

		private static void AskForInput()
		{
			
		}
	}
}
