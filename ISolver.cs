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
	/// <summary>
	/// Interface for boggle solvers.
	/// </summary>
	public interface ISolver
	{
		/// <summary>
		/// Solves a boggle board with the defined dictionary.
		/// </summary>
		/// <param name="board">The boggle board to solve.</param>
		/// <param name="library">The dictionary library of words.</param>
		/// <returns>The words found in the boggle board and the execution time in ms.</returns>
		Result Solve(IBoard board, ILibrary library);
	}

	/// <summary>
	/// Result information from execution.
	/// </summary>
	public class Result
	{
		/// <summary>
		/// Entire set of words found on the Boggle board.
		/// Note that duplicates are not counted or included.
		/// </summary>
		public ISet<string> Words { get; set; }

		/// <summary>
		/// The elapsed execution time to solve the Boggle board in milliseconds.
		/// </summary>
		public int ElapsedMS { get; set; }
	}
}
