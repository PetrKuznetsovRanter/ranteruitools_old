using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RanterTools.UI.Editor
{

    /// <summary>
    /// Context menu for combobox
    /// </summary>
    public class ComboBoxMenu
    {
        #region Global Methods
        [MenuItem("GameObject/UI/ComboBox (TMP)", false, 10)]
        public static void CreateComboBox()
        {
            var combobox = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/ComboBox (TMP)"));
            GameObject parent = null;
            if (Selection.activeObject != null) parent = Selection.activeObject as GameObject;
            if (parent == null) combobox.transform.parent = null;
            else combobox.transform.parent = parent.transform;
            combobox.name = "ComboBox (TMP)";
            (combobox.transform as RectTransform).anchoredPosition = Vector2.zero;
            Selection.activeObject = combobox;
        }
        #endregion Global Methods
    }

}