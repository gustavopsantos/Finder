using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Finder
{
    public class FinderWindow : EditorWindow
    {
        private HistoryTextField _findWhatField;
        private HistoryTextField _replaceWithField;
        private HistoryTextField _filtersField;
        private HistoryTextField _directoryField;

        private HistoryToggleField _includeSubFoldersField;
        private HistoryToggleField _matchCaseField;
        private HistoryToggleField _matchWholeWordOnlyField;

        private Button _findAllButton;
        private Button _replaceInFilesButton;

        [MenuItem("Finder/Open %f")]
        private static void Open()
        {
            var window = GetWindow<FinderWindow>(true, "Finder", true);
            window.minSize = new Vector2(480, 190);
            window.Show();
        }

        private void OnEnable()
        {
            _findWhatField = new HistoryTextField("Find what:", "finder-find-what");
            _replaceWithField = new HistoryTextField("Replace with:", "finder-replace-with");
            _filtersField = new HistoryTextField("Filters:", "finder-filters");
            _directoryField = new HistoryTextField("Directory:", "finder-directory");
            
            _includeSubFoldersField = new HistoryToggleField("Include sub-folders", "finder-include-sub-folders");
            _matchCaseField = new HistoryToggleField("Match case", "finder-match-case");
            _matchWholeWordOnlyField = new HistoryToggleField("Match whole word only", "finder-match-whole-word-only");

            _findAllButton = new Button("Find All", FindAll);
            _replaceInFilesButton = new Button("Replace in Files", ReplaceInFiles);
        }

        private void OnGUI()
        {
            _findWhatField.Present();
            _replaceWithField.Present();
            _filtersField.Present();
            _directoryField.Present();
            
            _includeSubFoldersField.Present();
            _matchCaseField.Present();
            _matchWholeWordOnlyField.Present();

            _findAllButton.Present();
            _replaceInFilesButton.Present();

            foreach (var tuple in _findResult)
            {
                GUILayout.Label($"[{tuple.Item2.Length}] {tuple.Item1}");
            }
        }

        private readonly List<(string, int[])> _findResult = new List<(string, int[])>();

        private void FindAll()
        {
            var files = GetFiles(
                _directoryField.Value,
                _includeSubFoldersField.Value ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly,
                _filtersField.Value.Split(',')
            );

            foreach (var file in files)
            {
                var fileContents = File.ReadAllText(file);
                var occurrences = FindOccurrences(fileContents, _findWhatField.Value);

                if (occurrences.Length > 0)
                {
                    var tuple = (file, occurrences);
                    _findResult.Add(tuple);
                }
            }
        }

        private void ReplaceInFiles()
        {
            
        }
        
                    
        public static string[] GetFiles(string directory, SearchOption option, params string[] patterns)
        {
            var result = new List<string>();

            foreach (var pattern in patterns)
            {
                result.AddRange(Directory.GetFiles(directory, pattern, option));
            }

            return result.ToArray();
        }
    
        private int[] FindOccurrences(string input, string pattern)
        {
            var regexPattern = _matchWholeWordOnlyField.Value
                ? Regex.Escape(pattern)
                : @"\b" + Regex.Escape(pattern) + @"\b";
            
            var regex = new Regex(regexPattern , RegexOptions.IgnoreCase);
            var matches = regex.Matches(input);
            var indices = new int[matches.Count];
        
            for (var i = 0; i < matches.Count; i++)
            {
                indices[i] = matches[i].Index;
            }
        
            return indices;
        }
    }
}