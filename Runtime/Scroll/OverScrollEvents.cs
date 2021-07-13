using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
namespace RanterTools.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class OverScrollEvents : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Parameters
        [SerializeField]
        RectTransform parent;
        [SerializeField]
        [Range(0.01f, 1f)]
        [Tooltip("How long needed drag for events.")]
        float overDragValue = 0.1f;
        [SerializeField]
        [Tooltip("Scroll rect if used")]
        ScrollRect scrollRect;
        [SerializeField]
        [Tooltip("Used another scroll if true.")]
        bool scroll = false;
        [SerializeField]
        [Range(0.1f, 10f)]
        [Tooltip("Theresold for scroll drag end.")]
        float theresold = 0.1f;
        [SerializeField]
        [Tooltip("Axises that ")]
        bool axisX, axisY;
        #endregion Parameters
        #region Events
        public OverDragEvent OnBeginOverDrag = new OverDragEvent();
        public OverDragEvent OnOverDrag = new OverDragEvent();
        public OverDragEvent OnEndOverDrag = new OverDragEvent();
        public OverDragEvent OnReturnOverDrag = new OverDragEvent();
        #endregion Events
        #region State
        RectTransform RectTransform;
        bool dragged = false;
        public OverScrollState axisXState = 0;
        public OverScrollState axisYState = 0;
        #endregion State
        #region Methods
        public void OnBeginDrag(PointerEventData pointer)
        {
            dragged = true;
            if (scrollRect != null) scrollRect.OnBeginDrag(pointer);
        }
        public void OnDrag(PointerEventData pointer)
        {
            if (dragged)
            {
                if (scrollRect != null) scrollRect.OnDrag(pointer);
                else
                    OnScroll(pointer.delta);
            }
        }
        public void OnEndDrag(PointerEventData pointer)
        {
            if (scrollRect != null) scrollRect.OnEndDrag(pointer);
            dragged = false;
        }

        void OnScroll(Vector2 delta)
        {
            if (axisX)
            {
                if (!scroll)
                {
                    RectTransform.anchoredPosition += new Vector2(delta.x, 0);
                }
                OverScrollWorker(true, RectTransform.anchoredPosition.x, RectTransform.rect.width, parent.rect.width, ref axisXState);
            }
            if (axisY)
            {
                if (!scroll)
                {
                    RectTransform.anchoredPosition += new Vector2(0, delta.y);
                }
                OverScrollWorker(false, RectTransform.anchoredPosition.y, RectTransform.rect.height, parent.rect.height, ref axisYState);
            }
        }
        void OnScrollValueChange(Vector2 value)
        {
            if (value.x <= 0.01f)
            {
                OverScrollWorker(true, RectTransform.anchoredPosition.x, RectTransform.rect.width, parent.rect.width, ref axisXState);
            }
            if (value.y <= 0.01f)
            {
                OverScrollWorker(false, RectTransform.anchoredPosition.y, RectTransform.rect.height, parent.rect.height, ref axisYState);
            }

        }


        void OverScrollWorker(bool axis, float position, float contentSize, float parentSize, ref OverScrollState state)
        {
            var min = parentSize * overDragValue;
            float pos = 0;
            if (contentSize > parentSize) pos = contentSize - parentSize;
            if (position > -theresold && position < pos + theresold)
            {
                if (state == OverScrollState.NonOverScroll) return;
                if (state == OverScrollState.OverScrollEnded || state == OverScrollState.OverScrollBegun)
                {
                    state = OverScrollState.NonOverScroll;
                    OnReturnOverDrag?.Invoke(axis, 0);
                    return;
                }
            }
            else
            {
                float value = 0;
                //Up
                if (position < -theresold)
                {
                    value = Mathf.Clamp(position / min, -1, 0);
                }
                //Down
                else if (position > pos + theresold)
                {
                    value = Mathf.Clamp((position - pos) / min, 0, 1);
                }

                if (state == OverScrollState.NonOverScroll)
                {
                    state = OverScrollState.OverScrollBegun;
                    OnBeginOverDrag?.Invoke(axis, value);
                    return;
                }
                else if (state == OverScrollState.OverScrollBegun)
                {
                    if (value >= 1 || value <= -1)
                    {
                        state = OverScrollState.OverScrollEnded;
                        OnEndOverDrag?.Invoke(axis, value);
                        return;
                    }
                    else
                    {
                        OnOverDrag?.Invoke(axis, value);
                        return;
                    }
                }
                else if (state == OverScrollState.OverScrollEnded)
                {
                    OnOverDrag?.Invoke(axis, value);
                    return;
                }
            }
        }
        #endregion Methods
        #region Unity
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (parent == null)
            {
                parent = transform.parent as RectTransform;
            }
            if (scrollRect == null)
            {
                scrollRect = GetComponentInParent<ScrollRect>();
            }
            if (scrollRect == null) scroll = false;
            else
            {
                scrollRect.onValueChanged.AddListener(OnScroll);
                scroll = true;
                scrollRect.onValueChanged.AddListener(OnScrollValueChange);
            }
            RectTransform = transform as RectTransform;

        }
        #endregion Unity
    }

    [Serializable]
    public class OverDragEvent : UnityEvent<bool, float>
    {

    }

    public enum OverScrollState { NonOverScroll = 0, OverScrollBegun = 1, OverScrollEnded = 2 }

}
