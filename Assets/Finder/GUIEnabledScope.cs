using System;
using UnityEngine;

namespace Finder
{
    public class GUIEnabledScope : IDisposable
    {
        private readonly bool _previousState;

        public GUIEnabledScope(bool value)
        {
            _previousState = GUI.enabled;
            GUI.enabled = value;
        }

        public void Dispose()
        {
            GUI.enabled = _previousState;
        }
    }
}