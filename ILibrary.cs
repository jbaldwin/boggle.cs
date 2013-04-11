using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boggle
{
	public interface ILibrary
	{
		void Load(string path);
		PrefixNode Book(byte c);
		void Reset();
	}
}
