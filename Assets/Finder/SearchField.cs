using UnityEditor;
using UnityEngine;

namespace Finder
{
    internal class SearchField
    {
        private readonly string _placeHolder;
        private readonly UnityEditor.IMGUI.Controls.SearchField _searchField;
        public string SearchString { get; private set; }

        public SearchField(string placeHolder)
        {
            _placeHolder = placeHolder;
            _searchField = new UnityEditor.IMGUI.Controls.SearchField();
        }

        public void Present()
        {
            SearchString = _searchField.OnGUI(SearchString);
            DrawPlaceHolder();
            GUILayout.Space(2);
        }

        private void DrawPlaceHolder()
        {
            GUILayout.Space(-18);

            using (new GUIColorScope(ShouldDrawPlaceholder() ? GUI.color : Color.clear))
            {
                var spacing = new string(' ', 5);
                GUILayout.Label($"{spacing}{_placeHolder}", GetPlaceholderStyle());
            }
        }

        private static GUIStyle GetPlaceholderStyle()
        {
            return new GUIStyle(EditorStyles.label)
            {
                fontSize = 10,
                fontStyle = FontStyle.Italic,
            };
        }

        private bool ShouldDrawPlaceholder()
        {
            return string.IsNullOrEmpty(SearchString) && !_searchField.HasFocus();
        }
    }
}
