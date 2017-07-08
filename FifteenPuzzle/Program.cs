using System;

namespace FifteenPuzzle
{	public class Program
	{
		public static void Main(string[] args)
		{
			try {
				if(args.Length == 0)
				{
					RunAllTestCases();
				} else if (args.Length == 2)
				{
					RunSingleSearch();
				}

				//AskForInput();
			}
			catch
			{
				Console.WriteLine("Something seriously went wrong!");
			}
		}

		private static void RunAllTestCases()
		{
			
		}

		private static void RunSingleSearch() {

		}
	}
}
