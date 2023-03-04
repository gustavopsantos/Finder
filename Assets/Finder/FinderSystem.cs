using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Finder
{
    public class FinderSystem
    {
        public static async Task<SearchResult> SearchInDirectoryAsync(
            string search,
            string directory,
            RegexOptions comparison,
            SearchOption option,
            string[] patterns,
            CancellationToken cancellationToken)
        {
            using (var progress = new ProgressReportScope("Finder"))
            {
                return await Task.Run(() =>
                {
                    var files = DirectoryUtilities.GetFiles(directory, option, patterns);
                    progress.SetTotalSteps(files.Length);

                    var usages = 0;
                    var result = new ConcurrentBag<string>();

                    void FindInFile(string filePath)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        
                        var occurrences = File.ReadAllText(filePath).CountOccurrences(search, comparison);
                        usages += occurrences;

                        if (occurrences >= 1)
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
                        FilesContainingSearch = result.ToArray(),
                        Occurrences = usages,
                    };
                }, cancellationToken);
            }
        }
    }
}