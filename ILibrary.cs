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
	/// Dictionary library interface.
	/// </summary>
	public interface ILibrary
	{
		/// <summary>
		/// Loads a new dictionary.
		/// </summary>
		/// <param name="dictionary">Each string in the array represents a word.</param>
		/// <param name="minWordSize">Minimum number of letters to be considered a word.
		/// Anything under this size will not be flagged as a word.</param>
		void Load(string[] dictionary, int minWordSize = 3);

		/// <summary>
		/// Each book contains a single letter (byte) [A-Z] with its children
		/// letters for all words under that book.
		/// </summary>
		IDictionary<byte, ILetter> Books { get; }

		/// <summary>
		/// Resets the entire library's books.
		/// </summary>
		void Reset();
	}

	/// <summary>
	/// Single letter in the word tree with all children letters.
	/// </summary>
	public interface ILetter
	{
		/// <summary>
		/// All children letters that form sub-words.
		/// </summary>
		IDictionary<byte, ILetter> Children { get; }

		/// <summary>
		/// True if this is the final letter in a word.
		/// This is required since similar words like 'apple' and 'apples'
		/// will use the same part of the tree but 'apple' is not a leaf
		/// since it will have 's' as a child letter.
		/// </summary>
		bool IsWord { get; set; }

		/// <summary>
		/// Optimization for setting if this word has been found already.
		/// Use 'Reset()' to set the Letter to its normal state, not found.
		/// </summary>
		bool IsFound { get; set; }

		/// <summary>
		/// The current Letter in the word of the tree.
		/// [A-Z]
		/// </summary>
		byte Letter { get; }

		/// <summary>
		/// Resets this Letter and all children Letters to be 'IsFound=false'.
		/// This is not required but is provided as an easy way to reset the
		/// entire word tree.
		/// </summary>
		void Reset();
	}
}
