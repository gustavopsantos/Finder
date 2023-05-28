using System;
using UnityEngine;

namespace Finder
{
    public class Button
    {
        private readonly string _label;
        private readonly Action _callback;

        public Button(string label, Action callback)
        {
            _label = label;
            _callback = callback;
        }

        public void Present()
        {
            if (GUILayout.Button(_label))
            {
                _callback.Invoke();
            }
        }
    }
}