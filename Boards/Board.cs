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

namespace boggle.Boards
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

		public int Width { get; private set; }
		public int Height { get; private set; }

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
			Width = x;
			Height = y;
			_grid = new byte[x][];
			for (var i = 0; i < x; i++)
			{
				_grid[i] = new byte[y];
			}
		}

		public void UseBoard(byte[][] board)
		{
			_grid = board;
			Width = _grid.Length;
			Height = _grid[0].Length;

			for (int i = 0; i < _grid.Length; i++)
			{
				for (int j = 0; j < _grid[i].Length; j++)
				{
					if (_grid[i][j] >= (byte)'a' && _grid[i][j] <= (byte)'z')
					{
						_grid[i][j] -= 32;
					}
				}
			}
		}

		public void UseSerialized(string data)
		{
			var tokens = data.Split(' ');
			Width = int.Parse(tokens[0]);
			Height = int.Parse(tokens[1]);
			var rows = new List<string>();

			_grid = new byte[Width][];
			for (int i = 0; i < Width; i++)
				_grid[i] = new byte[Height];

			for (int i = 0; i < Width; i++)
			{
				var line = tokens[2].Substring(i * Width, Height);
				line = line.ToUpper();
				for (int j = 0; j < Height; j++)
				{
					_grid[i][j] = (byte)line[j];
				}
			}
		}

		public string Serialize()
		{
			var data = new StringBuilder(Width.ToString() + " " + Height.ToString() + " ");
			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					data.Append((char)Grid[i][j]);
				}
			}
			return data.ToString();
		}
	}
}
