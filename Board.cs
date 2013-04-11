using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boggle
{
	public class Board : IBoard
	{
		private Random rand = new Random();

		private byte[][] _grid;

		public byte[][] Grid
		{
			get
			{
				return _grid;
			}
		}

		public int X { get; private set; }
		public int Y { get; private set; }

		public Board(int x, int y)
		{
			Resize(x, y);
			Random();
		}

		public Board(byte[][] board)
		{
			UseBoard(board);
		}

		public Board(string data)
		{
			UseSerialized(data);
		}

		public void Random()
		{
			for (int i = 0; i < _grid.Length; i++)
			{
				for (int j = 0; j < _grid[i].Length; j++)
				{
					_grid[i][j] = (byte)rand.Next('A', 'Z');
				}
			}
		}

		public void Resize(int x, int y)
		{
			X = x;
			Y = y;
			_grid = new byte[x][];
			for (var i = 0; i < x; i++)
			{
				_grid[i] = new byte[y];
			}
		}

		public void UseBoard(byte[][] board)
		{
			_grid = board;
			X = _grid.Length;
			Y = _grid[0].Length;

			for (int i = 0; i < _grid.Length; i++)
			{
				for (int j = 0; j < _grid[i].Length; j++)
				{
					if (_grid[i][j] >= 97 && _grid[i][j] <= 122)
					{
						_grid[i][j] -= 32;
					}
				}
			}
		}

		public void UseSerialized(string data)
		{
			var tokens = data.Split(' ');
			X = int.Parse(tokens[0]);
			Y = int.Parse(tokens[1]);
			var rows = new List<string>();

			_grid = new byte[X][];
			for (int i = 0; i < X; i++)
				_grid[i] = new byte[Y];

			for (int i = 0; i < X; i++)
			{
				var line = tokens[2].Substring(i * X, Y);
				line = line.ToUpper();
				for (int j = 0; j < Y; j++)
				{
					_grid[i][j] = (byte)line[j];
				}
			}
		}
	}
}
