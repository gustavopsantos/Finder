using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Finder
{
    public class FindResult
    {
        private readonly string _path;
        private readonly string _search;
        public List<Occurrence> Occurrences { get; } = new();

        private bool _opened;

        public FindResult(string path)
        {
            _path = path;
        }

        public void Present()
        {
            var eyeIcon = EditorGUIUtility.IconContent("animationvisibilitytoggleon");
            var folderIcon = EditorGUIUtility.IconContent("d_FolderOpened Icon");
            
            var style = new GUIStyle("Label")
            {
                richText = true
            };


            using (new GUILayout.HorizontalScope())
            {
                _opened = EditorGUILayout.Foldout(_opened, $"({Occurrences.Count} hit) {_path}", true);

                if (GUILayout.Button(eyeIcon, GUILayout.Width(32), GUILayout.Height(16)))
                {
                    
                }

                if (GUILayout.Button(folderIcon, GUILayout.Width(32), GUILayout.Height(16)))
                {
                    
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Space(32);

                using (new GUILayout.VerticalScope())
                {
                    if (_opened)
                    {
                        foreach (var occurence in Occurrences)
                        {
                            var highlighted = occurence.LineContent;
                            highlighted = highlighted.Insert(occurence.FinalIndex, "</color>");
                            highlighted = highlighted.Insert(occurence.StartIndex, "<color=yellow>");
                            highlighted = highlighted.Trim();
                            EditorGUILayout.LabelField($"Line {occurence.LineIndex + 1}: {highlighted}", style);
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