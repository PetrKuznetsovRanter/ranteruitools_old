using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RanterTools.UI.Debug
{
    public class DebugTab : MonoBehaviour, IDebugTab
    {
        /// <summary>
        /// Event for click on tab button or another event for this.
        /// </summary>
        public event Action<IDebugTab> OnTabClick;
        
        [Header("Tab")]
        [SerializeField]
        bool activeDefault = false;
        [SerializeField]
        string DebugWindowName = "RanterToolsDebug";
        [SerializeField]
        Color ActiveColor = new Color(0, 0, 0, 0.5f);
        [SerializeField]
        Color InactiveColor = new Color(0, 0, 0, 0.2f);
        [SerializeField]
        GameObject frame;
       
        Image TabBackground;
        Button TabButton;
        
        /// <summary>
        /// Initialize tab.virtual Create all objects.
        /// </summary>
        /// <param name="tabsContainer">Container for tabs.</param>
        /// <param name="frameContainer">Container for frame.</param>
        public virtual void Init(Transform tabsContainer, Transform frameContainer)
        {
            transform.SetParent(tabsContainer);
            frame.transform.SetParent(frameContainer);
            Hide();
        }
        /// <summary>
        /// Show tab.
        /// </summary>
        public virtual void Show()
        {
            TabBackground.color = ActiveColor;
            frame.SetActive(true);
        }
        /// <summary>
        /// Hide tab.
        /// </summary>
        public virtual void Hide()
        {
            TabBackground.color = InactiveColor;
            frame.SetActive(false);
        }
        /// <summary>
        /// Close tab. Normal mean delete object.
        /// </summary>
        public virtual void Close()
        {
            DestroyImmediate(frame);
            DestroyImmediate(gameObject);
        }
        
        void TabButtonClick()
        {
            if (OnTabClick != null) OnTabClick(this);
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            frame = GameObject.Find(gameObject.name.Replace("Tab", "DebugFrame"));
            TabBackground = GetComponent<Image>();
            TabButton = GetComponent<Button>();
            TabButton.onClick.AddListener(TabButtonClick);
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            DebugWindow.OnRegisterTab(DebugWindowName, this, activeDefault);
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            TabButton.onClick.RemoveListener(TabButtonClick);
        }
    }
}