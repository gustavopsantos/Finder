using System;
using UnityEngine;

namespace Finder
{
    public class GUIColorScope : IDisposable
    {
        private readonly Color _previousState;

        public GUIColorScope(Color color)
        {
            _previousState = GUI.color;
            GUI.color = color;
        }

        public void Dispose()
        {
            GUI.color = _previousState;
        }
    }
}