using System.IO;

namespace Finder
{
    public struct SearchResult
    {
        public string Search;
        public int FilesScanned;
        public SearchOption SearchMode;
        public string SearchDirectory;
        public string[] Patterns;
        public string[] FilesContainingSearch;
        public int Occurrences;
    }
}