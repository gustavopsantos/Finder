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
            var eyeIcon = EditorGUIUtility.IconContent("animationvisibilitytoggleon", "Ping");
            var folderIcon = EditorGUIUtility.IconContent("d_FolderOpened Icon", "Reveal in Explorer");
            
            var style = new GUIStyle("Label")
            {
                richText = true
            };


            using (new GUILayout.HorizontalScope("box"))
            {
                _opened = EditorGUILayout.Foldout(_opened, $"({Occurrences.Count} {(Occurrences.Count > 1 ? "hits" : "hit")}) {_path}", true);

                if (GUILayout.Button(eyeIcon, GUILayout.Width(24), GUILayout.Height(16)))
                {
                    var asset = AssetDatabase.LoadMainAssetAtPath(_path);
                    Selection.activeObject = asset;
                    EditorGUIUtility.PingObject(asset);
                }

                if (GUILayout.Button(folderIcon, GUILayout.Width(24), GUILayout.Height(16)))
                {
                    EditorUtility.RevealInFinder(_path);
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
                            highlighted = highlighted.Insert(occurence.StartIndex, "<color=orange>");
                            highlighted = highlighted.Trim();
                            EditorGUILayout.LabelField($"<i>Line {occurence.LineIndex + 1}:</i> {highlighted}", style);
                        }
                    }
                }
            }
        }
    }
}