using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace RanterTools.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Image))]
    public class PixelPerUnitFromDPI : MonoBehaviour
    {
#if UNITY_EDITOR
        static List<PixelPerUnitFromDPI> All = new List<PixelPerUnitFromDPI>();
#endif
        
#if UNITY_EDITOR
        [MenuItem("RanterTools/UI/DPI/ForceUpdate")]
        static void UpdateAll()
        {
            foreach (var p in All)
            {
                p.UpdateParameters();
            }
        }
#endif
      
        [SerializeField]
        float PixelPerUnityMultiplier;
        
        Image image;
        Image Image
        {
            get { return image = image ?? GetComponent<Image>(); }
        }
#if UNITY_EDITOR
        float width;
        float oldPixelPerUnityMultiplier;
#endif
        float multiplier;
        
        void UpdateParameters()
        {
            multiplier = PixelPerUnityMultiplier * 1080.0f / Mathf.Min(Screen.width, Screen.height);
            if (Image.type == Image.Type.Sliced)
            {
                Image.pixelsPerUnitMultiplier = multiplier;
            }


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
            if (PixelPerUnityMultiplier == -1)
                PixelPerUnityMultiplier = Image.pixelsPerUnitMultiplier;
#if UNITY_EDITOR
            All.Add(this);
#endif
#if UNITY_EDITOR
            if (width != Mathf.Min(Screen.width, Screen.height) || oldPixelPerUnityMultiplier != PixelPerUnityMultiplier)
            {
                width = Mathf.Min(Screen.width, Screen.height);
                oldPixelPerUnityMultiplier = PixelPerUnityMultiplier;
#endif
                UpdateParameters();
#if UNITY_EDITOR
            }
#endif
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
#if UNITY_EDITOR
            All.Remove(this);
#endif

        }
        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {

        }

#if UNITY_EDITOR
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (width != Mathf.Min(Screen.width, Screen.height) || oldPixelPerUnityMultiplier != PixelPerUnityMultiplier)
            {
                UpdateParameters();
                width = Mathf.Min(Screen.width, Screen.height);
                oldPixelPerUnityMultiplier = PixelPerUnityMultiplier;
            }
        }
#endif
    }

}
