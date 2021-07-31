using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RanterTools.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        public static List<SafeArea> All { get; set; } = new List<SafeArea>();
        
        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get { return rectTransform = rectTransform ?? GetComponent<RectTransform>(); }
        }
        
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            All.Add(this);
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            All.Remove(this);
        }
    }
}