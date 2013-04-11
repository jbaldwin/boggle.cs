using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boggle
{
	public interface IBoard
	{
		byte[][] Grid { get; }

		int X { get; }
		int Y { get; }

		void Random();
		void Resize(int x, int y);
		void UseBoard(byte[][] board);
		void UseSerialized(string data);
	}
}
