using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RanterTools.UI.Debug
{
    public class DebugWindow : MonoBehaviour
    {
        public static Action<string, IDebugTab, bool> OnRegisterTab;
        public static Action<string> OnShow;
       
        [SerializeField]
        Button close;
        [SerializeField]
        string Name;
        [SerializeField]
        Transform tabsContainer;
        [SerializeField]
        Transform framesContainer;
       
        List<IDebugTab> Tabs { get; set; } = new List<IDebugTab>();
        
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
    }
}