using System;
using System.IO;
using AdvancedPS.Core.System;
using Newtonsoft.Json;
using UnityEngine;

namespace AdvancedPS.Core.Utils
{
    public class APLogger
    {
        private static PopupSettings _settings;

        static APLogger()
        {
            LoadSettings();
        }
        
        public static void RefreshSettings()
        {
            _settings = SettingsManager.Settings;
        }
        
        public static void Log(string message)
        {
            if (_settings.LogType is "Log" or "Warning" or "Error")
            {
                Debug.Log(message);
            }
        }

        public static void LogWarning(string message)
        {
            if (_settings.LogType is "Warning" or "Error")
            {
                Debug.LogWarning(message);
            }
        }

        public static void LogError(string message)
        {
            if (_settings.LogType == "Error")
            {
                Debug.LogError(message);
            }
        }

        private static void LoadSettings()
        {
            try
            {
                string path = FileSearcher.SettingsFilePath;
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    _settings = JsonConvert.DeserializeObject<PopupSettings>(json);
                }
                else
                {
                    _settings = new PopupSettings
                    {
                        CustomIconsEnabled = true,
                        LogType = "Error"
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to load settings: " + ex.Message);
                _settings = new PopupSettings
                {
                    CustomIconsEnabled = true,
                    LogType = "Error"
                };
            }
        }
    }
}