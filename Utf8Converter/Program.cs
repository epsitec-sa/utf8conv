//	Copyright © 2016, EPSITEC SA, CH-1400 Yverdon-les-Bains, Switzerland
//	Author: Pierre ARNAUD, Maintainer: Pierre ARNAUD

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utf8Converter
{
	public class Program
	{
		public static void Main(string[] args)
		{
			string filter = args.Length > 1 ? args[1] : "*.cs";
			string root   = args.Length > 2 ? args[2] : ".";

			foreach (var path in System.IO.Directory.GetFiles (root, filter, System.IO.SearchOption.AllDirectories))
			{
				if (Program.ProcessFile (path))
				{
					System.Console.WriteLine ("{0}: converted", path);
				}
			}
		}


		public static bool ProcessFile(string path)
		{
			var input  = System.IO.File.ReadAllBytes (path);
			var output = System.Text.Encoding.UTF8.GetBytes (UnicodeSampler.GetUTF8String (input));

			if (input.SequenceEqual (output))
			{
				return false;
			}
			else
			{
				System.IO.File.WriteAllBytes (path, output);
				return true;
			}
		}

	}
}
