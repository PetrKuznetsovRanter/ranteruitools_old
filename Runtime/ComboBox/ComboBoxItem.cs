using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RanterTools.Base;


namespace RanterTools.UI
{

    [RequireComponent(typeof(Button))]
    public class ComboBoxItem : MonoBehaviour
    {
        ComboBoxOptionString option;

        public ComboBoxOptionString Option
        {
            get { return option; }
            set
            {
                option = value; SetData(value); //if (string.IsNullOrEmpty(option.Country))
                                                //{ Button.interactable = false; }
            }
        }
        Button button;
        public Button Button
        {
            get { return button = button ?? GetComponent<Button>(); }
        }
        TextMeshProUGUI label;
        TextMeshProUGUI Label
        {
            get { return label = label ?? GetComponentInChildren<TextMeshProUGUI>(true); }
        }
        
        void SetData(ComboboxOption<string> option)
        {
            if (Label != null) Label.text = option.ToString().ToUpper();
        }
       
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {

        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {

        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {

        }
        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {

        }
    }
}