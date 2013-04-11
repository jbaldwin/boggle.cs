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
