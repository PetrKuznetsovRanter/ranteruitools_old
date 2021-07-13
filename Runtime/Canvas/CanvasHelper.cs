using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace RanterTools.UI
{

    [RequireComponent(typeof(Canvas))]
    public class CanvasHelper : MonoBehaviour
    {
        #region Global Event
        public static UnityEvent onOrientationChange = new UnityEvent();
        public static UnityEvent onResolutionChange = new UnityEvent();
        #endregion Global Event
        #region Global State
        public static bool isLandscape { get; private set; }
        static List<CanvasHelper> helpers = new List<CanvasHelper>();
        static bool screenChangeVarsInitialized = false;
        static ScreenOrientation lastOrientation = ScreenOrientation.Portrait;
        static Vector2 lastResolution = Vector2.zero;
        static Rect lastSafeArea = Rect.zero;
        #endregion Global State
        #region Global Methods

        public static void ForceUpdate()
        {
            OrientationChanged();
            ResolutionChanged(true);
            SafeAreaChanged(true);
        }

        static void OrientationChanged()
        {
            //Debug.Log("Orientation changed from " + lastOrientation + " to " + Screen.orientation + " at " + Time.time);

            lastOrientation = Screen.orientation;
            lastResolution.x = Screen.width;
            lastResolution.y = Screen.height;

            isLandscape = lastOrientation == ScreenOrientation.LandscapeLeft || lastOrientation == ScreenOrientation.LandscapeRight || lastOrientation == ScreenOrientation.Landscape;
            onOrientationChange.Invoke();

        }

        static void ResolutionChanged(bool force = false)
        {
            if (lastResolution.x == Screen.width && lastResolution.y == Screen.height && !force)
                return;

            //Debug.Log("Resolution changed from " + lastResolution + " to (" + Screen.width + ", " + Screen.height + ") at " + Time.time);

            lastResolution.x = Screen.width;
            lastResolution.y = Screen.height;

            isLandscape = Screen.width > Screen.height;
            onResolutionChange.Invoke();
        }

        static void SafeAreaChanged(bool force = false)
        {
            if (lastSafeArea == Screen.safeArea && !force)
                return;

            //Debug.Log("Safe Area changed from " + lastSafeArea + " to " + Screen.safeArea.size + " at " + Time.time);

            lastSafeArea = Screen.safeArea;

            for (int i = 0; i < helpers.Count; i++)
            {
                helpers[i].ApplySafeArea();
            }
        }

        public static Vector2 GetCanvasSize()
        {
            return helpers[0].rectTransform.sizeDelta;
        }

        public static Vector2 GetSafeAreaSize()
        {
            foreach (var s in SafeArea.All)
            {
                if (s.RectTransform != null)
                {
                    return s.RectTransform.sizeDelta;
                }
            }

            return GetCanvasSize();
        }
        #endregion Global  Methods
        #region State
        Canvas canvas;
        RectTransform rectTransform;
        #endregion State


        #region Unity


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (!helpers.Contains(this))
                helpers.Add(this);

            canvas = GetComponent<Canvas>();
            rectTransform = GetComponent<RectTransform>();

            if (!screenChangeVarsInitialized)
            {
                lastOrientation = Screen.orientation;
                lastResolution.x = Screen.width;
                lastResolution.y = Screen.height;
                lastSafeArea = Screen.safeArea;

                screenChangeVarsInitialized = true;
            }
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            ApplySafeArea();
        }


        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (helpers[0] != this)
                return;

            if (Screen.orientation != lastOrientation)
                OrientationChanged();

            if (Screen.safeArea != lastSafeArea)
                SafeAreaChanged();

            if (Screen.width != lastResolution.x || Screen.height != lastResolution.y)
                ResolutionChanged();
        }

        void ApplySafeArea()
        {
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= canvas.pixelRect.width;
            anchorMin.y /= canvas.pixelRect.height;
            anchorMax.x /= canvas.pixelRect.width;
            anchorMax.y /= canvas.pixelRect.height;
            foreach (var s in SafeArea.All)
            {
                s.RectTransform.anchorMin = anchorMin;
                s.RectTransform.anchorMax = anchorMax;
            }
        }


        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            if (helpers != null && helpers.Contains(this))
                helpers.Remove(this);
        }

        #endregion Unity
    }


}