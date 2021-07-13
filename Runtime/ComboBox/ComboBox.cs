using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace RanterTools.UI
{

    /// <summary>
    /// Class ComboBox
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("RanterTools/UI/ComboBox")]
    [DisallowMultipleComponent]
    public class ComboBox : MonoBehaviour
    {
        #region Global State
        public static event Action<ComboBox> OnOpenPanel;
        #endregion Global State

        #region Parameters
        /// <summary>
        /// AvailableOptions for combobox
        /// </summary>
        /// <typeparam name="ComboBoxOptionString">Standart class for combobox option.</typeparam>
        /// <returns></returns>
        public List<ComboBoxOptionString> AvailableOptions = new List<ComboBoxOptionString>();
        /// <summary>
        /// Main InputField for combobox
        /// </summary>
        [SerializeField]
        TMP_InputField mainInput;
        /// <summary>
        /// Scroll rect of items panel
        /// </summary>
        [SerializeField]
        ScrollRect scrollRect;
        /// <summary>
        /// Arrow button for show items panel
        /// </summary>
        [SerializeField]
        Button arrow;
        /// <summary>
        /// Template of option
        /// </summary>
        [SerializeField]
        ComboBoxItem template;
        /// <summary>
        /// Input field enabled
        /// </summary>
        [SerializeField]
        bool input = true;
        /// <summary>
        /// Reset input field text, after select option
        /// </summary>
        [SerializeField]
        bool resetAfterSelect = false;
        [SerializeField]
        bool closeIfAnotherOpened = false;
        /// <summary>
        /// On select option
        /// </summary>
        public SelectionChangedEvent OnSelectionChanged;
        #endregion Parameters
        #region Events
        [System.Serializable]
        public class SelectionChangedEvent : UnityEngine.Events.UnityEvent<string>
        {

        }

        #endregion Events

        #region State
        /// <summary>
        /// Selected option, if it is.
        /// </summary>
        public ComboBoxOptionString SelectedOption = null;

        /// <summary>
        /// Is panel active
        /// </summary>
        bool isPanelActive = false;
        /// <summary>
        /// Panel items
        /// </summary>
        List<ComboBoxOptionString> panelItems;
        /// <summary>
        /// Panel's object
        /// </summary>
        /// <typeparam name="int">Id of option</typeparam>
        /// <typeparam name="ComboBoxItem">ComboboxItem</typeparam>
        /// <returns></returns>
        Dictionary<int, ComboBoxItem> panelObjects = new Dictionary<int, ComboBoxItem>();
        /// <summary>
        /// Text for search
        /// </summary>
        string text = "";
        /// <summary>
        /// Panel
        /// </summary>
        RectTransform panel;
        /// <summary>
        /// Default panel size
        /// </summary>
        Vector2 defaultPanelSize;
        /// <summary>
        /// Scroll panel anchor max
        /// </summary>
        float scrollPanelAnchorMax;
        /// <summary>
        /// Scroll panel bottom offset
        /// </summary>
        float scrollPanelBottomOffset;

        #endregion State

        #region Methods

        /// <summary>
        /// Initialize for new available item
        /// </summary>
        /// <returns>Is success</returns>
        public bool Initialize()
        {
            bool success = true;
            try
            {
                mainInput.enabled = input;
                isPanelActive = false;
                panel.gameObject.SetActive(isPanelActive);
                panelItems = AvailableOptions;
                foreach (var o in panelObjects)
                {
                    DestroyImmediate(o.Value.gameObject);
                }
                panelObjects.Clear();
                foreach (var c in AvailableOptions)
                {
                    panelObjects[c.ID] = Instantiate<ComboBoxItem>(template, scrollRect.content);
                    panelObjects[c.ID].Option = c;
                    panelObjects[c.ID].gameObject.SetActive(true);
                    panelObjects[c.ID].gameObject.name = panelObjects[c.ID].Option.Name;
                    panelObjects[c.ID].Button.onClick.AddListener(delegate { OnItemClicked(c); });
                }
                RedrawPanel();
            }
            catch (System.NullReferenceException ex)
            {
                ToolsDebug.LogError("Something is setup incorrectly with the dropdownlist component causing a Null Refernece Exception");
                success = false;
            }
            return success;
        }

        /// <summary>
        /// Select item from the list.
        /// </summary>
        /// <param name="item"></param>
        public void Select(string item)
        {
            OnItemClicked(item);
        }


        /// <summary>
        /// What happens when an item in the list is selected
        /// </summary>
        /// <param name="item"></param>
        void OnItemClicked(string item)
        {
            SelectedOption = AvailableOptions.Find(s => s.Name == item);
            OnSelectionChanged.Invoke(item);
            if (!resetAfterSelect)
            {
                text = item;
                mainInput.text = item;
            }
            else
            {
                text = mainInput.text = "";
            }

            isPanelActive = false;
            panel.gameObject.SetActive(false);
        }



        /// <summary>
        /// Redraw panel
        /// </summary>
        void RedrawPanel()
        {
            RectTransform scrollRectTransform = (scrollRect.transform as RectTransform), rectTransform = (transform as RectTransform);
            float height = 0, contentHeight = 0, panelSize = 0;
            bool variants = false;
            foreach (var c in panelObjects)
            {
                if (text == "" || c.Value.Option.Name.ToLower().StartsWith(text.ToLower()))
                {
                    c.Value.gameObject.SetActive(true);
                    height += (c.Value.transform as RectTransform).rect.height;
                    variants = true;
                }
                else c.Value.gameObject.SetActive(false);
            }

            if (variants)
            {

                if (height < (scrollPanelBottomOffset - defaultPanelSize.x * rectTransform.rect.height))
                {
                    contentHeight = height + scrollPanelBottomOffset;
                    panel.anchorMin = new Vector2(panel.anchorMin.x, -(contentHeight / rectTransform.rect.height));
                    panelSize = (panel.anchorMax.y - panel.anchorMin.y) * rectTransform.rect.height;
                    panel.sizeDelta = panel.anchoredPosition = Vector2.zero;
                    scrollRectTransform.anchorMin = new Vector2(scrollRectTransform.anchorMin.x, scrollPanelBottomOffset / panelSize);
                    scrollRectTransform.anchorMax = new Vector2(scrollRectTransform.anchorMax.x, 1 - rectTransform.rect.height / panelSize);
                    scrollRectTransform.anchoredPosition = scrollRectTransform.sizeDelta = Vector2.zero;
                }
                else
                {
                    panel.anchorMin = new Vector2(panel.anchorMin.x, defaultPanelSize.x);
                    panelSize = (panel.anchorMax.y - panel.anchorMin.y) * rectTransform.rect.height;
                    panel.sizeDelta = panel.anchoredPosition = Vector2.zero;
                    scrollRectTransform.anchorMin = new Vector2(scrollRectTransform.anchorMin.x, scrollPanelBottomOffset / panelSize);
                    scrollRectTransform.anchorMax = new Vector2(scrollRectTransform.anchorMax.x, 1 - rectTransform.rect.height / panelSize);
                    scrollRectTransform.anchoredPosition = scrollRectTransform.sizeDelta = Vector2.zero;
                }
            }
            else
            {
                contentHeight = scrollPanelBottomOffset;
                panel.anchorMin = new Vector2(panel.anchorMin.x, (contentHeight / rectTransform.rect.height));
                panelSize = (panel.anchorMax.y - panel.anchorMin.y) * rectTransform.rect.height;
                panel.sizeDelta = panel.anchoredPosition = Vector2.zero;
            }
        }

        /// <summary>
        /// On input field value changed
        /// </summary>
        /// <param name="currText">New text</param>
        void OnValueChanged(string currText)
        {
            text = currText;
            if (text == "")
            {
                SelectedOption = null;
                if (arrow == null)
                {
                    isPanelActive = false;
                    panel.gameObject.SetActive(isPanelActive);
                }
                else
                {
                    RedrawPanel();
                }
            }
            else
            {
                isPanelActive = true;
                panel.gameObject.SetActive(isPanelActive);
                if (OnOpenPanel != null) OnOpenPanel(this);
                RedrawPanel();
            }
        }

        /// <summary>
        /// On option selected
        /// </summary>
        /// <param name="text">Option text</param>
        void OnFieldSelect(string text)
        {
            mainInput.text = "";
        }



        /// <summary>
        /// On Arrow tap handler
        /// </summary>
        void OnArrowTap()
        {
            isPanelActive = !isPanelActive;
            text = "";
            if (OnOpenPanel != null && isPanelActive) OnOpenPanel(this);
            RedrawPanel();
            panel.gameObject.SetActive(isPanelActive);
        }

        /// <summary>
        /// Callback for event when every combobox is opening.
        /// </summary>
        /// <param name="comboBox">Opening combobox</param>
        void OnOpenPanelHandler(ComboBox comboBox)
        {
            if (comboBox != this && closeIfAnotherOpened)
            {
                isPanelActive = false;
                panel.gameObject.SetActive(isPanelActive);
                RedrawPanel();
            }
        }
        #endregion Methods

        #region Unity
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        public void Awake()
        {
            OnOpenPanel += OnOpenPanelHandler;
            (template.transform as RectTransform).sizeDelta = new Vector2(scrollRect.content.rect.width, scrollRect.content.rect.width / template.GetComponent<AspectRatioFitter>().aspectRatio);
            panel = scrollRect.transform.parent as RectTransform;
            defaultPanelSize = new Vector2(panel.anchorMin.y, panel.anchorMax.y);
            scrollPanelAnchorMax = (scrollRect.transform as RectTransform).anchorMax.y;
            scrollPanelBottomOffset = (scrollRect.transform as RectTransform).anchorMin.y * panel.rect.height;
            //Debug.Log($"{gameObject.name} scrollPanelBottomOffset={scrollPanelBottomOffset} {panel.rect.height} {(scrollRect.transform.parent as RectTransform).anchorMin.y}");
            Initialize();
            mainInput.onValueChanged.AddListener(OnValueChanged);
            mainInput.onSelect.AddListener(OnFieldSelect);
            if (arrow != null) arrow.onClick.AddListener(OnArrowTap);
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            OnOpenPanel -= OnOpenPanelHandler;
            mainInput.onValueChanged.RemoveListener(OnValueChanged);
            mainInput.onSelect.RemoveListener(OnFieldSelect);
            if (arrow != null) arrow.onClick.RemoveListener(OnArrowTap);
        }
        #endregion Unity
    }

}
