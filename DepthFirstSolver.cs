using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boggle
{
	public class DepthFirstSolver : ISolver
	{
		private static int[][] neighbors = new int[][] {
			new int[] { -1, -1 },
			new int[] {  0, -1 },
			new int[] {  1, -1 },
			new int[] { -1,  0 },
			new int[] {  1,  0 },
			new int[] { -1,  1 },
			new int[] {  0,  1 },
			new int[] {  1,  1 }
		};

		private HashSet<string> _words;

		public DepthFirstSolver()
		{

		}

		public Result Run(IBoard board, ILibrary library)
		{
			_words = new HashSet<string>();

			var result = new Result();
			var start = Environment.TickCount;

			for (int i = 0; i < board.X; i++)
			{
				for (int j = 0; j < board.Y; j++)
				{
					var path = new WordPath(board.Grid[i][j], i, j);
					Run(i, j, path, 1, board, library.Book(board.Grid[i][j]));
				}
			}

			var stop = Environment.TickCount;
			result.ElapsedMS = stop - start;
			result.Words = _words;

			return result;
		}

		private void Run(int x, int y, WordPath path, int depth, IBoard board, PrefixNode parent)
		{
			var c = board.Grid[x][y];

			for (int i = 0; i < 8; i++)
			{
				var nX = x + neighbors[i][0];
				var nY = y + neighbors[i][1];

				// check for out of bounds
				if (nX < 0 ||
					nX >= board.X)
					continue;

				if (nY < 0 ||
					nY >= board.Y)
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
							_words.Add(word);
							child.IsFound = true;
						}
					}

					// maximum recusion should be maximum word length (28~)
					Run(nX, nY, path, depth + 1, board, child);
				}
			}
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
