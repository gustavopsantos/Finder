using System.Collections.Generic;
using System.IO;

namespace Finder
{
	public static class DirectoryUtilities
	{
		public static string[] GetFiles(string path, SearchOption option, params string[] patterns)
		{
			var result = new List<string>();

			foreach (var pattern in patterns)
			{
				result.AddRange(Directory.GetFiles(path, pattern, option));
			}

			return result.ToArray();
		}
	}
}