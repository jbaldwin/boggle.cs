/** 
 * Boggle Solver
 * Copyright (C) - 2013 Josh Baldwin - https://github.com/jbaldwin/boggle
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace boggle.Solvers
{
	public class DepthFirstSolver : ISolver
	{
		/// <summary>
		/// All 8 neighbor combinations.
		/// </summary>
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

		/// <summary>
		/// Solves the boggle board!
		/// </summary>
		/// <param name="board">Board to solve.</param>
		/// <param name="library">Dictionary of words.</param>
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

					// note: I'm sure this is a lot of memory spawning a bajillion tasks but it was easy.
					tasks.Add(Task.Run(() =>
					{

						var path = new WordPath(board.Grid[state.X][state.Y], state.X, state.Y);
						var myWords = Worker(
							state.X, 
							state.Y, 
							path, 
							1, 
							board, 
							library.Books[board.Grid[state.X][state.Y]], 
							new HashSet<string>());

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

		/// <summary>
		/// Worker thread for a single square in the boggle grid.
		/// </summary>
		/// <param name="x">X grid position.</param>
		/// <param name="y">Y grid position.</param>
		/// <param name="path">Word Path object.</param>
		/// <param name="depth">Current number of squares deep, maximum is 32.</param>
		/// <param name="board">The boggle board.</param>
		/// <param name="parent">The parent node, when first called this is the first letter in the word's book from the dictionary.</param>
		/// <param name="words">The words this worker has found on the boggle board.</param>
		/// <returns></returns>
		private HashSet<string> Worker(
			int x, 
			int y, 
			WordPath 
			path, int 
			depth, 
			IBoard board, 
			ILetter parent, 
			HashSet<string> words)
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

					// maximum recursion should be maximum word length (28~)
					Worker(nX, nY, path, depth + 1, board, child, words);
				}
			}

			return words;
		}
	}

	/// <summary>
	/// This class is to capture the X and Y grid coordinates for a worker thread.
	/// </summary>
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

	/// <summary>
	/// Each worker thread is given a word path.  A word path is the letters from each square on the grid to make a word.
	/// Since boggle does not let you re-use letters this word path remembers all the parent squares and their letters
	/// in two 32 sized arrays, each element cooresponding to the depth of the word path.
	/// The word path only goes to a depth of 32 since the maximum word size in the enable1 dictionary is 28 (or 27 can't remember).
	/// If you have longer words... up the depth count.
	/// Note: this does not check for overflow on the MAX_DEPTH when performing actions so verify your dictionary's longest word or you could get a runtime error.
	/// </summary>
	class WordPath
	{
		public static readonly int MAX_DEPTH = 32;

		public byte[] Letters;
		public Point[] Parents;

		public WordPath(byte c, int x, int y)
		{
			Letters = new byte[MAX_DEPTH];
			Letters[0] = c;

			Parents = new Point[MAX_DEPTH];
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
