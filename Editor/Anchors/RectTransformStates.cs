using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RanterTools.UI.Editor
{
    class RectTransformStates : ScriptableObject
    {
        public List<RectTransformStatesKeyValuePair> states = new List<RectTransformStatesKeyValuePair>();
    }
    [System.Serializable]
    class RectTransformStatesKeyValuePair
    {
        public string Key;
        public RectTransformState Value;
    }
}