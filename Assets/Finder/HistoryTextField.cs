using UnityEditor;
using UnityEngine;

namespace Finder
{
    public class HistoryTextField
    {
        private readonly string _label;
        private readonly string _storageId;

        private string _content;

        public HistoryTextField(string label, string storageId)
        {
            _label = label;
            _storageId = storageId;
            _content = EditorPrefs.GetString(_storageId, string.Empty);
        }

        public void Present()
        {
            using (new GUILayout.HorizontalScope())
            {
                _content = EditorGUILayout.TextField(_label, _content);

                if (GUILayout.Button(GUIContent.none, EditorStyles.popup, GUILayout.Width(20)))
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent(@"C:\git\ProjectOne"), false, () => { });
                    menu.AddItem(new GUIContent(@"C:\git\ProjectTwo"), false, () => { });
                    menu.AddItem(new GUIContent(@"C:\git\ProjectThree"), false, () => { });
                    menu.ShowAsContext();
                }
            }
        }
    }
}