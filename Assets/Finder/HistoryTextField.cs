using UnityEditor;
using UnityEngine;

namespace Finder
{
    public class HistoryTextField
    {
        private readonly string _label;
        private readonly string _storageId;

        public string Value { get; private set; }

        public HistoryTextField(string label, string storageId)
        {
            _label = label;
            _storageId = storageId;
            Value = EditorPrefs.GetString(_storageId, string.Empty);  
        }

        public void Present()
        {
            using (new GUILayout.HorizontalScope())
            {
                EditorGUI.BeginChangeCheck();
                Value = EditorGUILayout.TextField(_label, Value);
                if (EditorGUI.EndChangeCheck())
                {
                    EditorPrefs.SetString(_storageId, Value);
                }

                if (GUILayout.Button(GUIContent.none, EditorStyles.popup, GUILayout.Width(20)))
                {
                    var menu = new GenericMenu();
                    AddMenuItem(menu, "class");
                    AddMenuItem(menu, "*.cs");
                    AddMenuItem(menu, "Assets");
                    menu.ShowAsContext();
                }
            }
        }

        private void AddMenuItem(GenericMenu menu, string item)
        {
            menu.AddItem(new GUIContent(item), false, () =>
            {
                Value = item;
                EditorPrefs.SetString(_storageId, Value);
            });
        }
    }
}