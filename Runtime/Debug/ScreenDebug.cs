using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RanterTools.UI.Debug
{
    public class ScreenDebug : DebugTab
    {
        public static Action OnChangeVisible;
        
        [Header("Screen")]
        [SerializeField]
        TMP_Dropdown screenOrientation;
        [SerializeField]
        TMP_InputField screenSizeX;
        [SerializeField]
        TMP_InputField screenSizeY;
        [SerializeField]
        TMP_Dropdown fullScreenMode;
        [SerializeField]
        TMP_Dropdown refreshRate;
        [SerializeField]
        Button applyScreenSize;
        
        public void Apply()
        {
            Screen.orientation = (ScreenOrientation)(screenOrientation.value + 1);
            FullScreenMode fullScreenMode = (FullScreenMode)this.fullScreenMode.value;
            int refreshRate = 30;
            switch (this.refreshRate.value)
            {
                case 0:
                    refreshRate = 30;
                    break;
                case 1:
                    refreshRate = 60;
                    break;
                case 2:
                    refreshRate = 90;
                    break;
                case 3:
                    refreshRate = 120;
                    break;
                default:
                    refreshRate = 30;
                    break;
            }
            Screen.SetResolution(int.Parse(screenSizeX.text), int.Parse(screenSizeY.text), fullScreenMode, refreshRate);
        }
       
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (screenOrientation == null)
                screenOrientation = GameObject.Find("ScreenOrientation").GetComponent<TMP_Dropdown>();
            if (screenSizeX == null)
                screenSizeX = GameObject.Find("ScreenSizeX").GetComponent<TMP_InputField>();
            if (screenSizeY == null)
                screenSizeY = GameObject.Find("ScreenSizeY").GetComponent<TMP_InputField>();
            if (fullScreenMode == null)
                fullScreenMode = GameObject.Find("FullScreenMode").GetComponent<TMP_Dropdown>();
            if (refreshRate == null)
                refreshRate = GameObject.Find("RefrashRate").GetComponent<TMP_Dropdown>();
            if (applyScreenSize == null)
                applyScreenSize = GameObject.Find("ApplyScreen").GetComponent<Button>();


        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            if (screenOrientation != null) screenOrientation.value = ((int)Screen.orientation) - 1;
            if (screenSizeX != null) screenSizeX.text = Screen.currentResolution.width.ToString();
            if (screenSizeY != null) screenSizeY.text = Screen.currentResolution.height.ToString();
            if (fullScreenMode != null)
            {
                fullScreenMode.value = (int)Screen.fullScreenMode;
            }
            if (refreshRate != null)
            {
                switch (Screen.currentResolution.refreshRate)
                {
                    case 30:
                        refreshRate.value = 0;
                        break;
                    case 60:
                        refreshRate.value = 1;
                        break;
                    case 90:
                        refreshRate.value = 2;
                        break;
                    case 120:
                        refreshRate.value = 3;
                        break;
                    default:
                        refreshRate.value = 0;
                        break;
                }
            }
            applyScreenSize.onClick.AddListener(Apply);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            applyScreenSize.onClick.RemoveListener(Apply);
        }
    }

}