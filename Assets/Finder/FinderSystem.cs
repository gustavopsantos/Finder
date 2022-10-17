using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Finder
{
	public class FinderSystem
	{
		public static async Task<SearchResult> SearchInDirectoryAsync(string search, string directory, StringComparison comparison, SearchOption option, params string[] patterns)
		{
			using (var progress = new ProgressReportScope("Finder"))
			{
				return await Task.Run(async () =>
				{
					var files = DirectoryUtilities.GetFiles(directory, option, patterns);
					progress.SetTotalSteps(files.Length);
					var tasks = files.Chunk(32).Select(c => SearchInFilesAsync(search, c, comparison, progress));
					var result = (await Task.WhenAll(tasks)).SelectMany(x => x).ToArray();

					return new SearchResult
					{
						Search = search,
						FilesScanned = files.Length,
						SearchMode = option,
						SearchDirectory = directory,
						Patterns = patterns,
						FilesContainingSearch = result
					};
				});
			}
		}

		private static Task<List<string>> SearchInFilesAsync(string search, string[] files, StringComparison comparison, ProgressReportScope progress)
		{
			return Task.Run(() =>
			{
				var result = new List<string>();

				foreach (var file in files)
				{
					if (File.ReadAllText(file).Contains(search, comparison))
					{
						result.Add(file);
					}

					progress.Increment();
				}

				return result;
			});
		}
	}
}