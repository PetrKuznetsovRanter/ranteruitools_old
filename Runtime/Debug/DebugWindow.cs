using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RanterTools.UI.Debug
{
    public class DebugWindow : MonoBehaviour
    {
        #region Global Events
        public static Action<string, IDebugTab, bool> OnRegisterTab;
        public static Action<string> OnShow;
        #endregion Global Events

        #region Parameters
        [SerializeField]
        Button close;
        [SerializeField]
        string Name;
        [SerializeField]
        Transform tabsContainer;
        [SerializeField]
        Transform framesContainer;
        #endregion Parameters
        #region State
        List<IDebugTab> Tabs { get; set; } = new List<IDebugTab>();
        #endregion State
        #region Methods
        void RegisterTab(IDebugTab tab, bool active)
        {
            Tabs.Add(tab);
            tab.Init(tabsContainer, framesContainer);
            if (active) tab.Show();
            tab.OnTabClick += ClickOnTab;
        }

        void ClickOnTab(IDebugTab tab)
        {
            foreach (var t in Tabs)
            {
                if (tab != t) t.Hide();
                else t.Show();
            }
        }

        void RegisterTabHandler(string name, IDebugTab tab, bool active = false)
        {
            if (name == Name) RegisterTab(tab, active);
        }

        void Show(string name)
        {
            if (name == Name) gameObject.SetActive(true);
        }
        void Hide()
        {
            gameObject.SetActive(false);
        }
        #endregion Methods

        #region Unity
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (string.IsNullOrEmpty(Name))
            {
                Name = gameObject.name;
            }
            OnRegisterTab += RegisterTabHandler;
            OnShow += Show;
            close.onClick.AddListener(Hide);
            gameObject.SetActive(false);
        }


        #endregion Unity
    }
}