using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            GUILayout.Label($"Searched through {_searchedFiles} files, found {_findResults.Values.Sum(fr => fr.Occurrences.Count)} hits in {_findResults.Count} files");
            
            foreach (var findResult in _findResults.Values)
            {
                findResult.Present();
            }
        }

        private int _searchedFiles;
        private readonly ConcurrentDictionary<string, FindResult> _findResults = new();

        private void FindAll()
        {
            _findResults.Clear();
            
            var files = GetFiles(
                _directoryField.Value,
                _includeSubFoldersField.Value ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly,
                _filtersField.Value.Split(',')
            );

            _searchedFiles = files.Length;

            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);

                for (var i = 0; i < lines.Length; i++)
                {
                    var lineOccurrences = FindOccurrences(lines[i], _findWhatField.Value);

                    foreach (var lineOccurrence in lineOccurrences)
                    {
                        var fr = _findResults.GetOrAdd(file, f => new FindResult(f, _findWhatField.Value));
                        fr.Occurrences.Add( new Occurrence(file,lines[i],i, lineOccurrence, lineOccurrence + _findWhatField.Value.Length));
                    }
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
                ? @"\b" + Regex.Escape(pattern) + @"\b"
                : Regex.Escape(pattern);
            
            var regex = new Regex(regexPattern , _matchCaseField.Value ? RegexOptions.None : RegexOptions.IgnoreCase);
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