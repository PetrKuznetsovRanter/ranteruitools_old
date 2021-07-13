using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RanterTools.UI.Debug
{
    public class DebugPlayerPrefs : DebugTab
    {
        #region Events
        public static Action OnChangeVisible;
        #endregion Events

        #region Global State

        #endregion Global State

        #region Global Methods

        #endregion Global Methods

        #region Parameters
        [Header("PlayersPrefs")]
        [SerializeField]
        TMP_Dropdown prefsType;
        [SerializeField]
        TMP_InputField prefsKey;
        [SerializeField]
        TMP_InputField prefsValue;
        [SerializeField]
        Button setPrefs;
        [SerializeField]
        Button hasPrefs;
        [SerializeField]
        Button deletePrefs;
        [SerializeField]
        Button deleteAllPrefs;

        #endregion Parameters


        #region Methods



        public void SetPlayersPrefs()
        {
            switch (prefsType.value)
            {
                case 0:
                    PlayerPrefs.SetFloat(prefsKey.text, float.Parse(prefsValue.text));
                    break;

                case 1:
                    PlayerPrefs.SetInt(prefsKey.text, int.Parse(prefsValue.text));
                    break;
                case 2:
                    PlayerPrefs.SetString(prefsKey.text, prefsValue.text);
                    break;
                default:
                    PlayerPrefs.SetString(prefsKey.text, prefsValue.text);
                    break;
            }
            PlayerPrefs.Save();
        }

        public void DeletePlayersPrefs()
        {
            PlayerPrefs.DeleteKey(prefsKey.text);
        }
        public void DeleteAllPlayersPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        public void Has()
        {
            if (PlayerPrefs.HasKey(prefsKey.text))
            {
                switch (prefsType.value)
                {
                    case 0:
                        prefsValue.text = PlayerPrefs.GetFloat(prefsKey.text).ToString();
                        break;

                    case 1:
                        prefsValue.text = PlayerPrefs.GetInt(prefsKey.text).ToString();
                        break;
                    case 2:
                        prefsValue.text = PlayerPrefs.GetString(prefsKey.text);
                        break;
                    default:
                        prefsValue.text = PlayerPrefs.GetString(prefsKey.text);
                        break;
                }

            }
        }
        #endregion Methods

        #region Unity
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (prefsKey == null)
                prefsKey = GameObject.Find("Key").GetComponent<TMP_InputField>();
            if (prefsValue == null)
                prefsValue = GameObject.Find("Value").GetComponent<TMP_InputField>();
            if (prefsType == null)
                prefsType = GameObject.Find("Type").GetComponent<TMP_Dropdown>();
            if (setPrefs == null)
                setPrefs = GameObject.Find("Set").GetComponent<Button>();
            if (hasPrefs == null)
                hasPrefs = GameObject.Find("Has").GetComponent<Button>();
            if (deletePrefs == null)
                deletePrefs = GameObject.Find("Delete").GetComponent<Button>();
            if (deleteAllPrefs == null)
                deleteAllPrefs = GameObject.Find("DeleteAll").GetComponent<Button>();
        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            setPrefs.onClick.AddListener(SetPlayersPrefs);
            hasPrefs.onClick.AddListener(Has);
            deletePrefs.onClick.AddListener(DeletePlayersPrefs);
            deleteAllPrefs.onClick.AddListener(DeleteAllPlayersPrefs);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            setPrefs.onClick.RemoveListener(SetPlayersPrefs);
            hasPrefs.onClick.RemoveListener(Has);
            deletePrefs.onClick.RemoveListener(DeletePlayersPrefs);
            deleteAllPrefs.onClick.RemoveListener(DeleteAllPlayersPrefs);
        }

        #endregion Unity
    }

}