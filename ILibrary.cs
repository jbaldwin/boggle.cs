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
