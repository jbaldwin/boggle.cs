using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boggle
{
	public interface ISolver
	{
		Result Run(IBoard board, ILibrary library);
	}

	public class Result
	{
		public ISet<string> Words { get; set; }
		public int ElapsedMS { get; set; }
	}
}
