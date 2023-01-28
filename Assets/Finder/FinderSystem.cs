using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace Finder
{
    public class FinderSystem
    {
        public static async Task<SearchResult> SearchInDirectoryAsync(string search, string directory, StringComparison comparison, SearchOption option, params string[] patterns)
        {
            using (var progress = new ProgressReportScope("Finder"))
            {
                return await Task.Run(() =>
                {
                    var files = DirectoryUtilities.GetFiles(directory, option, patterns);
                    progress.SetTotalSteps(files.Length);

                    var result = new ConcurrentBag<string>();

                    void FindInFile(string filePath)
                    {
                        if (File.ReadAllText(filePath).Contains(search, comparison))
                        {
                            result.Add(filePath);
                        }

                        progress.Increment();
                    }

                    Parallel.ForEach(files, FindInFile);

                    return new SearchResult
                    {
                        Search = search,
                        FilesScanned = files.Length,
                        SearchMode = option,
                        SearchDirectory = directory,
                        Patterns = patterns,
                        FilesContainingSearch = result.ToArray()
                    };
                });
            }
        }
    }
}