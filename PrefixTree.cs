using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boggle
{
	public class PrefixTree
	{
		public Dictionary<byte, PrefixNode> Books;

		public PrefixTree(string[] dictionary)
		{
			Books = new Dictionary<byte, PrefixNode>();

			for (byte i = (byte)'A'; i <= (byte)'Z' ; i++)
			{
				Books.Add((byte)i, new PrefixNode((byte)i));
			}

			foreach (var word in dictionary)
			{
				Insert(word);
			}
		}

		private void Insert(string word)
		{
			Insert(Books[(byte)word[0]], word);
		}

		private void Insert(PrefixNode parent, string word)
		{
			for (int i = 1; i < word.Length; i++)
			{
				var c = (byte)word[i];

				// add a child node since it does not exist
				if (!parent.Children.ContainsKey(c))
				{
					parent.Children.Add(c, new PrefixNode(c));
					parent = parent.Children[c];
				}
				// use existing child node since it already exists
				else
				{
					parent = parent.Children[c];
				}
			}

			if (word.Length > 2)
			{
				parent.IsWord = true;
			}
		}
	}

	public static class ArrayExtensions
	{
		public static T[] Tail<T>(this T[] data)
		{
			T[] result = new T[data.Length - 1];
			Array.Copy(data, 1, result, 0, data.Length - 1);
			return result;
		}
	}

	public class PrefixNode
	{
		public byte C;
		public Dictionary<byte, PrefixNode> Children;
		public bool IsWord;
		public bool IsFound;

		public PrefixNode(byte c)
		{
			C = c;
			Children = new Dictionary<byte, PrefixNode>();
			IsWord = false;
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
