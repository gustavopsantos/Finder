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
        }

        private void FindAll()
        {
            
        }

        private void ReplaceInFiles()
        {
            
        }
    }
}