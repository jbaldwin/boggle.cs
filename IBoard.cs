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

		int Width { get; }
		int Height { get; }

		void Random();
		void Resize(int x, int y);
		void UseBoard(byte[][] board);

		/// <summary>
		/// Loads a serialized string board.
		/// The data is in the format of
		/// `width` `height` `data`
		/// 
		/// where `width` is the width of the board (int)
		/// where `height` is the height of the board (int)
		/// where `data` is the entire board data serialized as a single line.
		/// 
		/// This function will read the width and height and then chunk each row
		/// from the serialized data section.
		/// 
		/// example data:
		///		3 3 yoxrbaved
		/// becomes:
		///		width: 3
		///		height: 3
		///		board:
		///			yox
		///			rba
		///			ved
		/// </summary>
		/// <param name="data"></param>
		void UseSerialized(string data);

		/// <summary>
		/// Serializes the board.
		/// </summary>
		/// <returns></returns>
		string Serialize();
	}
}
