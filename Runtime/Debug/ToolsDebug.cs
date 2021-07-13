using System.Collections;
using System.Collections.Generic;
using UE = UnityEngine;


namespace RanterTools.UI
{


    public class ToolsDebug
    {
        public static void Log(object log)
        {
#if RANTER_TOOLS_DEBUG_NETWORKING
            UE.Debug.Log(log);
#endif
        }
        public static void Log(object log, UE.Object context)
        {
#if RANTER_TOOLS_DEBUG_NETWORKING
            UE.Debug.Log(log, context);
#endif
        }

        public static void LogError(object log)
        {
#if RANTER_TOOLS_DEBUG_NETWORKING
            UE.Debug.LogError(log);
#endif
        }
        public static void LogError(object log, UE.Object context)
        {
#if RANTER_TOOLS_DEBUG_NETWORKING
            UE.Debug.LogError(log, context);
#endif
        }

        public static void LogWarning(object log)
        {
#if RANTER_TOOLS_DEBUG_NETWORKING
            UE.Debug.Log(log);
#endif
        }
        public static void LogWarning(object log, UE.Object context)
        {
#if RANTER_TOOLS_DEBUG_NETWORKING
            UE.Debug.Log(log, context);
#endif
        }
    }
}