using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Finder
{
    public class FindResult
    {
        private readonly string _path;
        private readonly string _search;
        public int[] Occurrences { get; private set; }

        private bool _opened;

        public FindResult(string path, string search, int[] occurrences)
        {
            _path = path;
            _search = search;
            Occurrences = occurrences;
        }

        public void Present()
        {
            var style = new GUIStyle("Label")
            {
                richText = true
            };

            _opened = EditorGUILayout.Foldout(_opened, $"({Occurrences.Length} hit) {_path}");

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Space(32);

                using (new GUILayout.VerticalScope())
                {
                    if (_opened)
                    {
                        foreach (var occurrence in Occurrences)
                        {
                            var contents = File.ReadAllText(_path);
                            var startIndex = Mathf.Clamp(occurrence - 10, 0, int.MaxValue);
                            var finalIndex = Mathf.Clamp(occurrence + _search.Length + 10, int.MinValue, contents.Length);
                            
                            var lines = File.ReadAllLines(_path);
                            Convert(_path, occurrence, out var lineIndex, out var localIndex);
                            var line = lines[lineIndex];

                            line = line.Insert(localIndex + _search.Length, "</color>");
                            line = line.Insert(localIndex, "<color=yellow>");
                            EditorGUILayout.LabelField($"Line {lineIndex}: {line}", style);
                        }
                    }
                }
            }
        }

        public static void Convert(string filePath, int characterIndex, out int lineIndex, out int localIndex)
        {
            using (var reader = new StreamReader(filePath))
            {
                lineIndex = 0;
                var charCount = 0;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    int lineLength = line.Length + Environment.NewLine.Length; // Account for line breaks

                    if (charCount + lineLength > characterIndex)
                    { 
                        localIndex = characterIndex - charCount;
                        return;
                    }

                    charCount += lineLength;
                    lineIndex++;
                }
            }

            throw new Exception();
        }
    }
}