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
using System.IO;

namespace boggle.Libraries
{
	public class WordTreeLibrary : ILibrary
	{
		public IDictionary<byte, ILetter> Books { get; private set; }

		/// <summary>
		/// Creates a new word tree library.  This implementation is a single letter per node with the root
		/// being each starting character [A-Z].  Each root is defined as a 'Book'.
		/// </summary>
		/// <param name="dictionary">Each string entry in the array is a word in the dictionary.</param>
		/// <param name="minWordSize">Minimum word size to flag a letter as a word.</param>
		public WordTreeLibrary(string[] dictionary, int minWordSize = 3)
		{
			Books = new Dictionary<byte, ILetter>();

			for (byte i = (byte)'A'; i <= (byte)'Z'; i++)
				Books.Add((byte)i, new Node((byte)i));

			foreach (var word in dictionary)
				Insert(word, minWordSize);
		}

		public void Load(string[] dictionary, int minWordSize = 3)
		{
			foreach (var word in dictionary)
				Insert(word, minWordSize);
		}

		public void Reset()
		{
			foreach (var book in Books)
				book.Value.Reset();
		}

		private void Insert(string word, int minWordSize)
		{
			Insert(Books[(byte)word[0]], word, minWordSize);
		}

		private void Insert(ILetter node, string word, int minWordSize)
		{
			for (int i = 1; i < word.Length; i++)
			{
				var c = (byte)word[i];

				// add a child node since it does not exist
				if (!node.Children.ContainsKey(c))
				{
					node.Children.Add(c, new Node(c));
					node = node.Children[c];
				}
				// use existing child node since it already exists
				else
				{
					node = node.Children[c];
				}
			}

			// flag the final node as a word if it is long enough
			if (word.Length >= minWordSize)
			{
				node.IsWord = true;
			}
		}
	}

	class Node : ILetter
	{
		public IDictionary<byte, ILetter> Children { get; private set; }
		public bool IsWord { get; set; }
		public bool IsFound { get; set; }
		public byte Letter { get; private set; }

		/// <summary>
		/// Node in the word tree.
		/// </summary>
		/// <param name="letter">Expects uppercase [A-Z] ASCII characters</param>
		/// <param name="isWord">True if this node is a word.</param>
		public Node(byte letter, bool isWord = false)
		{
			Letter = letter;
			Children = new Dictionary<byte, ILetter>();
			IsWord = isWord;
			IsFound = false;
		}

		public void Reset()
		{
			IsFound = false;
			foreach (var child in Children)
				child.Value.Reset();
		}
	}
}
