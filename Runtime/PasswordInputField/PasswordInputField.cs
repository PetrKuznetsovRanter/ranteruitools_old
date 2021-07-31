using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

namespace RanterTools.UI
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class PasswordInputField : TMP_InputField
    {
        [SerializeField]
        Button switchButton;
        [SerializeField]
        RectTransform starPrefab;
        [SerializeField]
        float aspectRatioStar = 1;
        [SerializeField]
        RectTransform stars;
       
        List<RectTransform> starsContainer = new List<RectTransform>();
        bool visible = false;
        bool needUpdate = false;
       
        void Switch()
        {
            visible = !visible;
            if (visible)
            {
                Color c = textComponent.color;
                c.a = 1;
                textComponent.color = c;
                stars.gameObject.SetActive(false);
            }
            else
            {

                stars.gameObject.SetActive(true);
                Color c = textComponent.color;
                c.a = 0;
                textComponent.color = c;
                OnValueChanged(text);
            }
        }

        void OnValueChanged(string text)
        {
            needUpdate = true;

        }

        public void UpdateStars()
        {
            if (text.Length < starsContainer.Count)
            {
                while (text.Length != starsContainer.Count)
                {
                    Destroy(starsContainer[starsContainer.Count - 1].gameObject);
                    starsContainer.RemoveAt(starsContainer.Count - 1);
                }
            }
            else
            {
                while (text.Length != starsContainer.Count)
                {
                    var star = Instantiate(starPrefab, stars);
                    starsContainer.Add(star);
                }
            }

            var a = textComponent.GetRenderedValues(false);
            if (!visible && text.Length > 0)
            {
                stars.anchoredPosition = new Vector2((textComponent.transform as RectTransform).anchoredPosition.x, stars.anchoredPosition.y);
                stars.sizeDelta = new Vector2(a.x, stars.sizeDelta.y);
            }
            float w = a.x / starsContainer.Count, h = w;
            for (int i = 0; i < starsContainer.Count; i++)
            {
                starsContainer[i].anchoredPosition = new Vector2(i * w, starsContainer[i].anchoredPosition.y);
                if (aspectRatioStar > 0)
                    h = w / aspectRatioStar;
                else h = starsContainer[i].rect.height;
                starsContainer[i].sizeDelta = new Vector2(w, h);
                if (!starsContainer[i].gameObject.activeSelf) starsContainer[i].gameObject.SetActive(true);
            }
        }
      
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected override void Start()
        {
            base.Start();
            visible = true;
            Switch();
        }


        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            if (switchButton != null) switchButton.onClick.AddListener(Switch);
            onValueChanged.AddListener(OnValueChanged);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            if (switchButton != null) switchButton.onClick.RemoveListener(Switch);
            onValueChanged.RemoveListener(OnValueChanged);
        }


        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            textComponent.ForceMeshUpdate();
            var a = textComponent.GetRenderedValues(false);
            if (!visible && text.Length > 0)
            {
                stars.anchoredPosition = new Vector2((textComponent.transform as RectTransform).anchoredPosition.x, stars.anchoredPosition.y);
                stars.sizeDelta = new Vector2(a.x, stars.sizeDelta.y);
            }
            if (needUpdate)
            {
                UpdateStars();
                needUpdate = false;
            }
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected override void LateUpdate()
        {

            base.LateUpdate();

        }
    }
}