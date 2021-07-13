using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RanterTools.UI.Debug
{
    /// <summary>
    /// Tab interface
    /// </summary>
    public interface IDebugTab
    {
        /// <summary>
        /// Event for click on tab button or another event for this.
        /// </summary>
        event Action<IDebugTab> OnTabClick;
        /// <summary>
        /// Initialize tab.Create all objects.
        /// </summary>
        /// <param name="tabsContainer">Container for tabs.</param>
        /// <param name="frameContainer">Container for frame.</param>
        void Init(Transform tabsContainer, Transform frameContainer);
        /// <summary>
        /// Show tab.
        /// </summary>
        void Show();
        /// <summary>
        /// Hide tab.
        /// </summary>
        void Hide();
        /// <summary>
        /// Close tab. Normal mean delete object.
        /// </summary>
        void Close();
    }
}