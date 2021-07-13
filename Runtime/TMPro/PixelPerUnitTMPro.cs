using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace RanterTools.UI
{

    [ExecuteInEditMode]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PixelPerUnitTMPro : MonoBehaviour
    {

        #region Global State
#if UNITY_EDITOR
        static List<PixelPerUnitTMPro> All = new List<PixelPerUnitTMPro>();
#endif
        #endregion Global State

        #region Global Methods
#if UNITY_EDITOR
        [MenuItem("RanterTools/UI/DPI/ForceUpdate")]
        static void UpdateAll()
        {
            foreach (var p in All)
            {
                p.UpdateParameters();
            }
        }
#endif
        #endregion Global Methods


        #region Parameters
        [SerializeField]
        float fontSize = -1;
        #endregion Parameters

        #region State
        bool cacheTried = false;

        TextMeshProUGUI TextMeshProUGUI { get; set; }
        TMP_InputField InputField { get; set; }
#if UNITY_EDITOR
        float width;
        float oldFont;
#endif
        float font;
        #endregion State

        #region Methods
        void UpdateParameters()
        {
            font = fontSize * Mathf.Min(Screen.width, Screen.height) / 1080.0f;
            if (InputField != null)
            {
                InputField.SetGlobalPointSize(font);
            }
            TextMeshProUGUI.fontSize = font;
        }

        void InitFont()
        {
            if (fontSize == -1)
            {
                if (InputField != null)
                {
                    if (InputField.textComponent == TextMeshProUGUI)
                    {
                        InputField.textComponent = null;
                    }
                    TextMeshProUGUI.enableAutoSizing = true;
                    TextMeshProUGUI.ForceMeshUpdate();
                }
                fontSize = TextMeshProUGUI.fontSize;
                if (InputField != null)
                {
                    if (InputField.textComponent == null)
                    {
                        InputField.textComponent = TextMeshProUGUI;
                    }
                    InputField.SetGlobalPointSize(font);
                }
            }
            TextMeshProUGUI.enableAutoSizing = false;
        }
        #endregion Methods

        #region Unity
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
            InputField = GetComponentInParent<TMP_InputField>();
#if UNITY_EDITOR
            All.Add(this);
#endif
#if UNITY_EDITOR
            InitFont();
            if (width != Mathf.Min(Screen.width, Screen.height) || oldFont != fontSize)
            {
                width = Mathf.Min(Screen.width, Screen.height);
                oldFont = font;
#endif
                UpdateParameters();
#if UNITY_EDITOR
            }
#endif
        }
#if UNITY_EDITOR
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            if (TextMeshProUGUI == null) TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
            if (InputField == null) InputField = GetComponentInParent<TMP_InputField>();
            InitFont();
            if (width != Mathf.Min(Screen.width, Screen.height) || oldFont != fontSize)
            {
                width = Mathf.Min(Screen.width, Screen.height);
                oldFont = font;
                UpdateParameters();
            }
        }
#endif

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
#if UNITY_EDITOR
            All.Remove(this);
#endif
        }
#if UNITY_EDITOR
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (width != Mathf.Min(Screen.width, Screen.height) || oldFont != fontSize)
            {
                UpdateParameters();
                width = Mathf.Min(Screen.width, Screen.height);
                oldFont = font;
            }
        }
#endif
        #endregion Unity
    }
}