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

namespace boggle
{
	class Boggle
	{
		static void Usage(string[] args)
		{
			var name = System.AppDomain.CurrentDomain.FriendlyName;
			Console.WriteLine("{0} <dictionary> <input> <output>", name);
			Console.WriteLine("\t<dictionary> is a line delimted file containing all valid words");
			Console.WriteLine("\t<input> is a file containing 'width height characterarray'\n\t\tCan be multiple files separated by ';'.");
			Console.WriteLine("\t<output> is the file to save the results to.");
			Console.ReadLine();
			Environment.Exit(0);
		}

		static void Main(string[] args)
		{
			if (args.Length < 3)
			{
				Usage(args);
			}

			var dictPath = args[0];
			var inputs = args[1].Split(';');
			var output = args[2];

			string[] dict = null;

			try
			{
				dict = File.ReadAllLines(dictPath);
				dict = dict.Select((word) => word.ToUpper()).ToArray();
			}
			catch (FileNotFoundException e)
			{
				// Fail hard since the dictionary file could not be found.
				Console.WriteLine("Dictionary file '{0}' was not found, does the file exist?", dictPath);
				Console.ReadLine();
				Environment.Exit(e.HResult);
			}

			var library = new Libraries.WordTreeLibrary(dict);
			var solver = new Solvers.DepthFirstSolver();

			var writer = new StreamWriter(output);

			foreach (var input in inputs)
			{
				var board = new Boards.Board(File.ReadAllText(input));

				var result = solver.Solve(board, library);
				var words = result.Words.ToList();
				words.Sort();

				Console.WriteLine("{0}", input);
				Console.WriteLine("Time (ms): {0}", result.ElapsedMS);
				Console.WriteLine("Words Found: {0}\n", result.Words.Count);

				writer.WriteLine("{0} | Time (ms): {1} | Words Found: {2}\n", input, result.ElapsedMS, result.Words.Count);

				foreach (var word in words)
				{
					writer.WriteLine(word.ToLower());
				}

				library.Reset();
			}

			writer.Close();

			Console.WriteLine("Press any key to exit...");
			Console.ReadLine();
		}
	}
}
