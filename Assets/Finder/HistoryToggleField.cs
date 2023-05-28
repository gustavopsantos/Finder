using UnityEditor;

namespace Finder
{
    public class HistoryToggleField
    {
        private readonly string _label;
        private readonly string _storageId;
        
        public bool Value { get; private set; }

        public HistoryToggleField(string label, string storageId)
        {
            _label = label;
            _storageId = storageId;
            Value = EditorPrefs.GetBool(storageId, false);
        }

        public void Present()
        {
            Value = EditorGUILayout.Toggle(_label, Value);
        }
    }
}