using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RanterTools.UI.Debug
{
    public class StandartDebugWindowShower : MonoBehaviour
    {
        #region Parameters
        [SerializeField]
        string windowName;
        [SerializeField]
        float tapTheresold = 0.2f;
        [SerializeField]
        KeyCode debugKey;
        #endregion Parameters
        #region State
        float timer;
        #endregion State
        #region Unity
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= tapTheresold && (Input.touchCount == 3 || Input.GetKeyDown(debugKey)))
            {
                if (DebugWindow.OnShow != null) DebugWindow.OnShow(windowName);
            }
        }
        #endregion Unity
    }

}