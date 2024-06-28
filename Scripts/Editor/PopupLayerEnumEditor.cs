using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdvancedPS.Core.Editor.Styles;
using AdvancedPS.Core.Enum;
using UnityEditor;
using UnityEngine;

namespace AdvancedPS.Core.Editor
{
    public class PopupLayerEnumEditor : EditorWindow
    {
        private string[] _enumNames;
        private string _newEnumName = string.Empty;
        private bool[] _enumNameChanged;
        private string enumFilePath;
        private bool autoSave;
        
        private const string AutoSaveKey = "AutoSaveEnabled";
        private const string ImagesPath = "Assets/advanced-popup-system/Scripts/Runtime/Images/";
        
        private Texture2D bannerTexture;
        private Texture2D iconTexture;
        private Texture2D blackTexture;
        
        [MenuItem("Tools/Advanced Popup System Layers")]
        public static void ShowWindow()
        {
            var window = GetWindow<PopupLayerEnumEditor>("Popup Layer Enum Editor");
            window.titleContent = new GUIContent("Popup Layer Enum Editor");
        }

        private void OnEnable()
        {
            enumFilePath = Path.Combine(Application.dataPath, "advanced-popup-system/Scripts/Runtime/Core/PopupLayerEnum.cs");
            LoadEnumNames();
            autoSave = PlayerPrefs.GetInt(AutoSaveKey, 1) == 1;
            
            bannerTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(ImagesPath + "AP_bundleBlack.png");
            iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(ImagesPath + "AP_LogoBlack.png");
            if (iconTexture != null)
            {
                titleContent = new GUIContent("Popup Layer Enum Editor", iconTexture);
                
                blackTexture = new Texture2D(1, 1);
                blackTexture.SetPixel(0, 0, Color.black);
                blackTexture.Apply();
            }
            
            this.minSize = new Vector2(256, 400);
        }

        private void OnGUI()
        {
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.white;
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), Color.black);
            
            if (bannerTexture != null)
            {
                const float bannerWidth = 256;
                const float bannerHeight = 128;
                Rect blackBackgroundRect = GUILayoutUtility.GetRect(position.width, bannerHeight);
                GUI.DrawTexture(blackBackgroundRect, blackTexture);

                Rect bannerRect = new Rect((position.width - bannerWidth) / 2, blackBackgroundRect.y, bannerWidth, bannerHeight);
                GUI.DrawTexture(bannerRect, bannerTexture);

                GUILayout.Space(blackBackgroundRect.height);
            }
            
            // Display current enums
            GUILayout.Space(10);
            
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
                        Debug.LogError("Invalid enum name.");
                    }
                }
                
                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    DeleteEnum(i);
                    if (autoSave)
                        SaveEnumChanges();
                    
                    GUILayout.EndHorizontal();
                    return;
                }
                GUILayout.EndHorizontal();
            }

            // Add new enum
            GUILayout.Space(10);
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
                    Debug.LogError("Invalid enum name.");
                }
            }
            
            // Auto-save and Save button (fixed at the bottom right)
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Auto-Save", GUILayout.ExpandWidth(false));
            bool newAutoSave = GUILayout.Toggle(autoSave, autoSave ? "[x]" : "[ ]", PopupLayerEnumEditorStyles.ToggleStyle);
            if (newAutoSave != autoSave)
            {
                autoSave = newAutoSave;
                PlayerPrefs.SetInt(AutoSaveKey, autoSave ? 1 : 0); // Save auto-save preference
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
            
            // Save changes
            if (_enumNameChanged != null && _enumNameChanged.Any(changed => changed) && autoSave)
            {
                SaveEnumChanges();
                Array.Clear(_enumNameChanged, 0, _enumNameChanged.Length);
            }
        }

        private void LoadEnumNames()
        {
            _enumNames = System.Enum.GetNames(typeof(PopupLayerEnum));
            _enumNameChanged = new bool[_enumNames.Length];
        }

        private void AddEnum(string enumName)
        {
            if (!string.IsNullOrEmpty(enumName) && !_enumNames.Contains(enumName))
            {
                Array.Resize(ref _enumNames, _enumNames.Length + 1);
                _enumNames[_enumNames.Length - 1] = enumName;
                Array.Resize(ref _enumNameChanged, _enumNames.Length);
            }
        }

        private void DeleteEnum(int index)
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

        private void SaveEnumChanges()
        {
            if (!File.Exists(enumFilePath))
            {
                Debug.LogError($"File not found: {enumFilePath}");
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
