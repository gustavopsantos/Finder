using System;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Finder
{
    public class FinderWindow : EditorWindow
    {
        private SearchField _searchField;
        private Vector2 _scrollPosition;
        private string _patterns = "*.meta,*.unity,*.anim,*.prefab";
        private SearchResult _result;
        private StringComparison _stringComparison = StringComparison.Ordinal;

        [MenuItem("Finder/Open")]
        private static void Open()
        {
            var window = GetWindow<FinderWindow>(true, "Finder", true);
            window.minSize = new Vector2(480, 190);
            window.Show();
        }

        private void OnEnable()
        {
            _searchField = new SearchField("What to search");
        }

        private void OnGUI()
        {
            using (new GUILayout.VerticalScope("box"))
            {
                _searchField.Present();
                _patterns = EditorGUILayout.TextField("Patterns", _patterns);
                _stringComparison = (StringComparison)EditorGUILayout.EnumPopup("Comparison Method", _stringComparison);
                EditorGUILayout.HelpBox(
                    "Muliple patterns are comma separated. Usage example: *.meta,*.unity,*.anim,*.prefab",
                    MessageType.Info);

                using (new GUIEnabledScope(!string.IsNullOrEmpty(_searchField.SearchString)))
                {
                    if (GUILayout.Button("Search"))
                    {
                        SearchAsync();
                    }
                }
            }

            PresentResultSection();
        }

        private void PresentResultSection()
        {
            if (_result.FilesContainingSearch == null)
            {
                return;
            }
            
            using (new GUILayout.VerticalScope("box"))
            {
                GUILayout.Label($"{_result.FilesContainingSearch.Length} results from {_result.FilesScanned.ToString("N0")} files scanned", EditorStyles.centeredGreyMiniLabel);

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                foreach (var path in _result.FilesContainingSearch)
                {
                    if (GUILayout.Button(path))
                    {
                        var asset = AssetDatabase.LoadMainAssetAtPath(path);
                        Selection.activeObject = asset;
                        EditorGUIUtility.PingObject(asset);
                    }
                }

                EditorGUILayout.EndScrollView();
            }
        }

        private CancellationTokenSource _cancellationTokenSource;

        private async void SearchAsync()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            
            string ToUnityProjectRelativePath(string absolutePath)
            {
                return absolutePath.Substring(Application.dataPath.Length - "Assets".Length);
            }

            _result = await FinderSystem.SearchInDirectoryAsync(
                _searchField.SearchString,
                Application.dataPath,
                _stringComparison,
                SearchOption.AllDirectories,
                _patterns.Split(','),
                _cancellationTokenSource.Token);

            _result.FilesContainingSearch = _result.FilesContainingSearch.Select(ToUnityProjectRelativePath).ToArray();
            
            Repaint();
        }
    }
}