using AdvancedPS.Core.System;
using UnityEngine;

namespace AdvancedPS.Core.Utils
{
    public class APLogger
    {
        private static readonly PopupSettings Settings;

        static APLogger()
        {
            Settings = SettingsManager.Settings;
        }
        
        public static void Log(string message)
        {
            if (Settings.LogType is "Log" or "Warning" or "Error")
            {
                Debug.Log(message);
            }
        }

        public static void LogWarning(string message)
        {
            if (Settings.LogType is "Warning" or "Error")
            {
                Debug.LogWarning(message);
            }
        }

        public static void LogError(string message)
        {
            if (Settings.LogType == "Error")
            {
                Debug.LogError(message);
            }
        }
    }
}