using System.Text.RegularExpressions;

namespace Finder
{
    public static class StringExtensions
    {
        public static int CountOccurrences(this string input, string substr, RegexOptions options)
        {
            return Regex.Matches(input, substr, options).Count;
        }
    }
}