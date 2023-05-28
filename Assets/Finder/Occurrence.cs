namespace Finder
{
    public class Occurrence
    {
        public string FilePath { get; }
        public string LineContent { get; }
        public int LineIndex { get; }
        public int StartIndex { get; }
        public int FinalIndex { get; }

        public Occurrence(string filePath, string lineContent, int lineIndex, int startIndex, int finalIndex)
        {
            FilePath = filePath;
            LineContent = lineContent;
            LineIndex = lineIndex;
            StartIndex = startIndex;
            FinalIndex = finalIndex;
        }
    }
}