using System;
using AdvancedPS.Core.System;
using AdvancedPS.Core.Utils;
using AdvancedPS.Editor.Styles;
using UnityEditor;
using UnityEngine;

namespace AdvancedPS.Editor
{
    public static class PopupSettingsEditor
    {
        private static PopupSettings Settings;
        
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
            Settings.CustomIconsEnabled = customIconsEnabled;
            Settings.LogType = logTypes[logTypeIndex];
            
            SettingsManager.SaveSettings();
        }

        private static void LoadSettings()
        {
            Settings = SettingsManager.LoadSettings();
            
            customIconsEnabled = Settings.CustomIconsEnabled;
            logTypeIndex = Array.IndexOf(logTypes, Settings.LogType);
        }
    }
}