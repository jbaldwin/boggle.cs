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
