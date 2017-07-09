# FifteenPuzzle
AI Agent that solves the Fifteen Sliding Block Puzzle problem through multiple search techniques, namely,
* Breadth-First Search
* Depth-First Search
* Iterative Deepening Depth-First Search
* Uniform Cost Search
* Greedy Best First Search
* A* Search

# Usage
* Run "FifteenPuzzle.exe" on any Windows machine. By default, all the six search algorithms are run against a test suite of initial states and goal states, and the output is printed on to the screen.
* To solve a specific puzzle state, from the command line type "FifteenPuzzle.exe <init_state> <goal_state>"

Note: each algorithm runs on it's own thread, so the order of algorithm output may change with each trial.
	
# Notes on Implementation
* The business logic of the program rests within the "Search" folder that gets compiled into the "Search.dll".
"Searcher.cs" contains the main crux of the search algorithms. The "GenericSearch" function performs the all of 
the searches. Each specific search passes in a different "NodeComparator" that is used to keep the "Frontier" (implemented as a Priority Queue) sorted.
* A hash table of previously visited states is maintained and all the Heuristic calculations are cached to speed things up.
* In order to use the "GenericSearch" algorithm, the "Search.dll" requires an implementation of "StateBase" and "IHeuristic".
This is done in the "FifteenPuzzle" folder.
