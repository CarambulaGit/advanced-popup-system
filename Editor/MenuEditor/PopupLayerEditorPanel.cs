using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdvancedPS.Core.Editor.Styles;
using AdvancedPS.Core.Enum;
using AdvancedPS.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace AdvancedPS.Core.Editor
{
    public class PopupLayerEditorPanel
    {
        private static string[] _enumNames;
        private static string _newEnumName = string.Empty;
        private static bool[] _enumNameChanged;
        private static string enumFilePath;
        private static bool autoSave;
        private static Vector2 scrollPosition;

        private const string AutoSaveKey = "AutoSaveEnabled";

        public static void Initialize()
        {
            enumFilePath = FileSearcher.LayersEnumFilePath;
            LoadEnumNames();
            autoSave = PlayerPrefs.GetInt(AutoSaveKey, 1) == 1;
        }

        public static void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, PopupSystemEditorStyles.ScrollViewStyle);
            GUILayout.BeginVertical();

            Array.Resize(ref _enumNameChanged, _enumNames.Length);
            for (int i = 0; i < _enumNames.Length; i++)
            {
                GUILayout.BeginHorizontal();
                string newEnumName = EditorGUILayout.DelayedTextField(_enumNames[i]);
                if (newEnumName != _enumNames[i])
                {
                    newEnumName = ValidateAndFormatEnumName(newEnumName);
                    if (!string.IsNullOrEmpty(newEnumName))
                    {
                        _enumNames[i] = newEnumName;
                        _enumNameChanged[i] = true;
                    }
                    else
                    {
                        APLogger.LogWarning("Invalid enum name.");
                    }
                }

                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    DeleteEnum(i);
                    if (autoSave)
                        SaveEnumChanges();

                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    EditorGUILayout.EndScrollView();
                    return;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

            EditorGUILayoutExtensions.DrawHorizontalLine(new Color(1, 1, 1, 0.2f), 1, 20, 0);
            
            GUILayout.Label("Add New Enum", EditorStyles.boldLabel);
            _newEnumName = GUILayout.TextField(_newEnumName);
            if (GUILayout.Button("Add"))
            {
                _newEnumName = ValidateAndFormatEnumName(_newEnumName);
                if (!string.IsNullOrEmpty(_newEnumName))
                {
                    AddEnum(_newEnumName);
                    _newEnumName = string.Empty;
                    if (autoSave)
                        SaveEnumChanges();
                }
                else
                {
                    APLogger.LogWarning("Invalid enum name.");
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayoutExtensions.DrawHorizontalLine(new Color(1, 1, 1, 0.2f), 1, 5, 0);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            GUILayout.Label("Auto-Save", GUILayout.ExpandWidth(false));
            bool newAutoSave = GUILayout.Toggle(autoSave, autoSave ? "[x]" : "[ ]", PopupSystemEditorStyles.ToggleStyle);
            if (newAutoSave != autoSave)
            {
                autoSave = newAutoSave;
                PlayerPrefs.SetInt(AutoSaveKey, autoSave ? 1 : 0);
                PlayerPrefs.Save();

                if (autoSave)
                    SaveEnumChanges();
            }

            GUI.enabled = !autoSave;
            if (GUILayout.Button("Save", GUILayout.Width(80)))
            {
                SaveEnumChanges();
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            if (_enumNameChanged != null && _enumNameChanged.Any(changed => changed) && autoSave)
            {
                SaveEnumChanges();
                Array.Clear(_enumNameChanged, 0, _enumNameChanged.Length);
            }
        }

        private static void LoadEnumNames()
        {
            _enumNames = System.Enum.GetNames(typeof(PopupLayerEnum));
            _enumNameChanged = new bool[_enumNames.Length];
        }

        private static void AddEnum(string enumName)
        {
            if (!string.IsNullOrEmpty(enumName) && !_enumNames.Contains(enumName))
            {
                Array.Resize(ref _enumNames, _enumNames.Length + 1);
                _enumNames[_enumNames.Length - 1] = enumName;
                Array.Resize(ref _enumNameChanged, _enumNames.Length);
            }
        }

        private static void DeleteEnum(int index)
        {
            _enumNames = _enumNames.Where((_, i) => i != index).ToArray();
            _enumNameChanged = _enumNameChanged.Where((_, i) => i != index).ToArray();
        }

        private static string ValidateAndFormatEnumName(string enumName)
        {
            if (string.IsNullOrEmpty(enumName))
                return string.Empty;

            enumName = Regex.Replace(enumName, @"\s+", "_");
            enumName = Regex.Replace(enumName, "_+", "_");
            enumName = enumName.ToUpper();

            return !Regex.IsMatch(enumName, @"^[A-Z0-9_]+$") ? string.Empty : enumName;
        }

        private static void SaveEnumChanges()
        {
            if (!File.Exists(enumFilePath))
            {
                APLogger.LogError($"File not found: {enumFilePath}");
                return;
            }

            StringBuilder enumFileContent = new StringBuilder();

            enumFileContent.AppendLine("namespace AdvancedPS.Core.Enum");
            enumFileContent.AppendLine("{");
            enumFileContent.AppendLine("    [System.Flags]");
            enumFileContent.AppendLine("    public enum PopupLayerEnum");
            enumFileContent.AppendLine("    {");

            for (int i = 0; i < _enumNames.Length; i++)
            {
                enumFileContent.AppendLine($"        {_enumNames[i]} = 1 << {i},");
            }

            enumFileContent.AppendLine("    }");
            enumFileContent.AppendLine("}");

            File.WriteAllText(enumFilePath, enumFileContent.ToString());
            AssetDatabase.Refresh();
        }
    }
}