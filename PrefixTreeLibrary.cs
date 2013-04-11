using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace boggle
{
	public class PrefixTreeLibrary : ILibrary
	{
		private PrefixTree _tree;

		public PrefixTreeLibrary(string path)
		{
			Load(path);
		}

		public void Load(string path)
		{
			var words = File.ReadAllLines(path);
			words = words.Select((word) => word.ToUpper()).ToArray();
			_tree = new PrefixTree(words);
		}

		public PrefixNode Book(byte c)
		{
			return _tree.Books[c];
		}

		public void Reset()
		{
			foreach (var book in _tree.Books)
			{
				book.Value.Reset();
			}
		}
	}
}
