using UnityEditor;
using UnityEngine;

namespace Finder
{
    public class FinderWindow : EditorWindow
    {
        [MenuItem("Finder/Open")]
        private static void Open()
        {
            var window = GetWindow<FinderWindow>();
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Finder", EditorStyles.centeredGreyMiniLabel);
        }
    }
}