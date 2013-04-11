﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace boggle.Solvers
{
	public class DepthFirstSolver : ISolver
	{
		private static int[][] _neighbors = new int[][] {
			new int[] { -1, -1 },
			new int[] {  0, -1 },
			new int[] {  1, -1 },
			new int[] { -1,  0 },
			new int[] {  1,  0 },
			new int[] { -1,  1 },
			new int[] {  0,  1 },
			new int[] {  1,  1 }
		};

		private object _mergeResultsLock = new object();

		public DepthFirstSolver()
		{

		}

		public Result Solve(IBoard board, ILibrary library)
		{
			var result = new Result();
			result.Words = new HashSet<string>();
			var start = Environment.TickCount;

			var tasks = new List<Task>();

			for (int i = 0; i < board.Width; i++)
			{
				for (int j = 0; j < board.Height; j++)
				{
					// Capture i/j values (will fail without since i/j change too fast before the thread runs)
					var state = new ThreadState(i, j);

					tasks.Add(Task.Run(() =>
					{

						var path = new WordPath(board.Grid[state.X][state.Y], state.X, state.Y);
						var myWords = Worker(state.X, state.Y, path, 1, board, library.Books[board.Grid[state.X][state.Y]], new HashSet<string>());

						// Merge results (queue up instead and have the main thread do the work?
						// This seems like a bottle neck but since tasks are not reused it might not matter.
						lock (_mergeResultsLock)
						{
							result.Words.UnionWith(myWords);
						}
					
					}));
				}
			}

			Task.WaitAll(tasks.ToArray());

			var stop = Environment.TickCount;
			result.ElapsedMS = stop - start;

			return result;
		}

		private HashSet<string> Worker(int x, int y, WordPath path, int depth, IBoard board, ILetter parent, HashSet<string> words)
		{
			var c = board.Grid[x][y];

			for (int i = 0; i < 8; i++)
			{
				var nX = x + _neighbors[i][0];
				var nY = y + _neighbors[i][1];

				// check for out of bounds
				if (nX < 0 ||
					nX >= board.Width)
					continue;

				if (nY < 0 ||
					nY >= board.Height)
					continue;

				// do not re-use tiles, and do not check your own level
				if (path.AlreadyVisited(nX, nY, depth - 1))
					continue;

				var nC = board.Grid[nX][nY];
				path.Set(nC, nX, nY, depth);

				if (parent.Children.ContainsKey(nC))
				{
					var word = string.Concat(path.Letters.Take(depth + 1).Select(letter => (char)letter));
					//var word = System.Text.Encoding.UTF8.GetString(path.Letters.Take(depth + 1).ToArray());

					var child = parent.Children[nC];

					if (child.IsWord)
					{
						if (!child.IsFound)
						{
							words.Add(word);
							child.IsFound = true;
						}
					}

					// maximum recusion should be maximum word length (28~)
					Worker(nX, nY, path, depth + 1, board, child, words);
				}
			}

			return words;
		}
	}

	class ThreadState
	{
		public volatile int X;
		public volatile int Y;

		public ThreadState(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	struct Point
	{
		public int X;
		public int Y;

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	class WordPath
	{
		public byte[] Letters;
		public Point[] Parents;

		public WordPath(byte c, int x, int y)
		{
			Letters = new byte[32];
			Letters[0] = c;

			Parents = new Point[32];
			Parents[0].X = x;
			Parents[0].Y = y;
		}

		public void Set(byte c, int x, int y, int depth)
		{
			Letters[depth] = c;
			Parents[depth].X = x;
			Parents[depth].Y = y;
		}

		public bool AlreadyVisited(int x, int y, int depth)
		{
			for (int i = depth; i >= 0; i--)
			{
				if (Parents[i].X == x &&
					Parents[i].Y == y)
				{
					return true;
				}
			}

			return false;
		}
	}
}