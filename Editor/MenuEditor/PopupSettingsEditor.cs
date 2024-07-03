using System;
using AdvancedPS.Core.Editor.Styles;
using AdvancedPS.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace AdvancedPS.Core.Editor
{
    public static class PopupSettingsEditor
    {
        private static bool customIconsEnabled;
        private static int logTypeIndex;
        private static readonly string[] logTypes = { "Error", "Warning", "Info" };
        
        public static void Initialize()
        {
            LoadSettings();
        }

        public static void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Custom icons:", GUILayout.ExpandWidth(false));
            bool newCustomIconsEnabled = GUILayout.Toggle(customIconsEnabled, customIconsEnabled ? "[x]" : "[ ]", PopupSystemEditorStyles.ToggleStyle);
            if (newCustomIconsEnabled != customIconsEnabled)
            {
                customIconsEnabled = newCustomIconsEnabled;
                SaveSettings();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Log Type:", GUILayout.ExpandWidth(false));
            int newLogTypeIndex = EditorGUILayout.Popup(logTypeIndex, logTypes);
            if (newLogTypeIndex != logTypeIndex)
            {
                logTypeIndex = newLogTypeIndex;
                SaveSettings();
            }
            GUILayout.EndHorizontal();
        }

        private static void SaveSettings()
        {
            SettingsManager.SaveSettings(new PopupSettings
            {
                CustomIconsEnabled = customIconsEnabled,
                LogType = logTypes[logTypeIndex]
            });
            
            APLogger.RefreshSettings();
        }

        private static void LoadSettings()
        {
            var settings = SettingsManager.LoadSettings();
            
            customIconsEnabled = settings.CustomIconsEnabled;
            logTypeIndex = Array.IndexOf(logTypes, settings.LogType);
        }
    }
}