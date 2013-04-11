using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace boggle
{
	class Boggle
	{
		static void Main(string[] args)
		{
			if (args.Length < 3)
			{
				var name = System.AppDomain.CurrentDomain.FriendlyName;
				Console.WriteLine("{0} <dictionary> <input> <output>", name);
				Console.WriteLine("\t<dictionary> is a line delimted file containing all valid words");
				Console.WriteLine("\t<input> is a file containing 'width height characterarray'\n\t\tCan be multiple files separated by ';'.");
				Console.WriteLine("\t<output> is the file to save the results to.");
				Console.ReadLine();
				Environment.Exit(0);
			}

			var dict = args[0];
			var inputs = args[1].Split(';');
			var output = args[2];

			var library = new PrefixTreeLibrary(dict);
			var solver = new DepthFirstSolver();

			foreach (var input in inputs)
			{
				var board = new Board(File.ReadAllText(input));

				var result = solver.Run(board, library);
				var words = result.Words.ToList();
				words.Sort();

				Console.WriteLine("{0}", input);
				Console.WriteLine("Time (ms): {0}", result.ElapsedMS);
				Console.WriteLine("Words Found: {0}\n", result.Words.Count);

				//using (StreamWriter sw = new StreamWriter(output))
				//{
				//	foreach (var word in words)
				//	{
				//		sw.WriteLine(word.ToLower());
				//	}
				//}

				library.Reset();
			}

			Console.ReadLine();
		}
	}
}
