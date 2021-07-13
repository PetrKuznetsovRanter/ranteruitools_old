using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;
namespace RanterTools.UI.Editor
{
    [DefaultExecutionOrder(10000)]
    public static class AnchorCalculator
    {
        #region Events

        #endregion Events

        #region Global State
        static Dictionary<string, RectTransformState> states = new Dictionary<string, RectTransformState>();
        #endregion Global State

        #region Global Methods
        /// <summary>
        /// Convert current size for currect screen to anchors.
        /// </summary>
        [MenuItem("CONTEXT/RectTransform/Apply anchors")]
        public static void ApplyAnchors(MenuCommand command)
        {
            if (EditorApplication.isPlaying) return;
            RectTransform rectTransform = (RectTransform)command.context;
            RectTransform parentRectTransform = rectTransform.parent as RectTransform;
            string path = RectTransformScenePath(rectTransform);
            if (!states.ContainsKey(path))
            {
                var state = new RectTransformState();
                state.defaultPosition = rectTransform.anchoredPosition;
                state.defaultSize = rectTransform.sizeDelta;
                state.defaultPivot = rectTransform.pivot;
                state.defaultAnchorMin = rectTransform.anchorMin;
                state.defaultAnchorMax = rectTransform.anchorMax;
                states[path] = state;
            }
            Vector2 position, pivotOffset = Vector2.zero;
            if (rectTransform.anchorMin == rectTransform.anchorMax)
            {
                pivotOffset = rectTransform.pivot * rectTransform.rect.size;
            }
            position = parentRectTransform.rect.size * rectTransform.anchorMin + rectTransform.anchoredPosition - pivotOffset;
            Vector2 min = new Vector2(position.x / parentRectTransform.rect.width,
                                      position.y / parentRectTransform.rect.height);
            Vector2 max = new Vector2((position.x + rectTransform.rect.width) / parentRectTransform.rect.width,
                                      (position.y + rectTransform.rect.height) / parentRectTransform.rect.height);
            rectTransform.anchorMin = min;
            rectTransform.anchorMax = max;
            rectTransform.sizeDelta = rectTransform.anchoredPosition = Vector2.zero;
            SaveStates();
        }


        /// <summary>
        /// Revert previous state of rect transform.
        /// </summary>
        [MenuItem("CONTEXT/RectTransform/Revert anchors")]
        public static void RevertAnchors(MenuCommand command)
        {
            if (EditorApplication.isPlaying) return;
            RectTransform rectTransform = (RectTransform)command.context;
            string path = RectTransformScenePath(rectTransform);
            if (!states.ContainsKey(path))
            {
                ToolsDebug.Log($"RectTransformExtension can't revert anchors.");
            }
            else
            {
                var state = states[path];
                rectTransform.pivot = state.defaultPivot;
                rectTransform.anchorMin = state.defaultAnchorMin;
                rectTransform.anchorMax = state.defaultAnchorMax;
                rectTransform.sizeDelta = state.defaultSize;
                rectTransform.anchoredPosition = state.defaultPosition;
                states.Remove(path);
            }
            SaveStates();
        }


        static string RectTransformScenePath(Transform transform)
        {

            string path = $"{transform.gameObject.name}";
            Transform t = transform.parent;
            while (t != null)
            {
                path = path.Insert(0, $"{t.gameObject.name}\\");
                t = t.parent;
            }
            path = path.Insert(0, $"{transform.gameObject.scene.name}\\");
            return path;
        }

        static void SaveStates()
        {
            var statesObjects = Resources.FindObjectsOfTypeAll<RectTransformStates>().ToList();
            RectTransformStates statesAsset;
            if (statesObjects.Count == 0)
            {
                statesAsset = CreateScriptableObjectForStateOrLoad();
            }
            else if (statesObjects.Count == 1)
            {
                statesAsset = statesObjects[0];
            }
            else
            {
                statesAsset = statesObjects[0];
                for (int i = statesObjects.Count - 1; i > 0; i--)
                {
                    statesObjects.RemoveAt(i);
                }
            }
            statesAsset.states = states.ToList().ConvertAll((s) => new RectTransformStatesKeyValuePair() { Key = s.Key, Value = s.Value });
        }

        [InitializeOnLoadMethod]
        static void LoadStates()
        {
            var statesObjects = Resources.FindObjectsOfTypeAll<RectTransformStates>().ToList();
            RectTransformStates statesAsset;
            if (statesObjects.Count == 0)
            {
                statesAsset = CreateScriptableObjectForStateOrLoad();
            }
            else if (statesObjects.Count == 1)
            {
                statesAsset = statesObjects[0];
            }
            else
            {
                statesAsset = statesObjects[0];
                for (int i = statesObjects.Count - 1; i > 0; i--)
                {
                    statesObjects.RemoveAt(i);
                }
            }
            states = statesAsset.states.ToDictionary((s) => s.Key, (s) => s.Value);
        }



        static RectTransformStates CreateScriptableObjectForStateOrLoad()
        {

            var path = Path.Combine(GetUIToolsDirectory(), "Editor", "Resources", "AnchorsHistory.asset");
            RectTransformStates statesAsset;
            if (File.Exists(path))
            {
                statesAsset = Resources.Load<RectTransformStates>("AnchorsHistory");
            }
            else
            {
                Directory.CreateDirectory(Path.Combine(GetUIToolsDirectory(), "Editor", "Resources"));
                statesAsset = ScriptableObject.CreateInstance<RectTransformStates>();
                AssetDatabase.CreateAsset(statesAsset, path);
            }
            AssetDatabase.SaveAssets();
            return statesAsset;
        }


        static string GetUIToolsDirectory(bool inAssets = true)
        {
            DirectoryInfo assets = new DirectoryInfo(Application.dataPath), uiPath = null;
            foreach (var p in assets.GetDirectories("*", SearchOption.AllDirectories))
            {
                if (p.Name == "uitools")
                {
                    uiPath = p;
                    break;
                }
            }
            if (uiPath == null) throw new System.Exception("Can't find uitool folder.");
            return uiPath.FullName.Replace(assets.FullName, "Assets");
        }
        #endregion Global Methods
    }


    [System.Serializable]
    class RectTransformState
    {
        public Vector2 defaultPosition;
        public Vector2 defaultSize;
        public Vector2 defaultAnchorMin, defaultAnchorMax;
        public Vector2 defaultPivot;
    }
}