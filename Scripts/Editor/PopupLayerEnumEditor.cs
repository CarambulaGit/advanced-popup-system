using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AdvancedPS.Core.Enum
{
    public class PopupLayerEnumEditor : EditorWindow
    {
        private string[] _enumNames;
        private string _newEnumName = string.Empty;
        private bool[] _enumNameChanged;
        private string enumFilePath;

        [MenuItem("Tools/Advanced Popup System Layers")]
        public static void ShowWindow()
        {
            GetWindow<PopupLayerEnumEditor>("Popup Layer Enum Editor");
        }

        private void OnEnable()
        {
            enumFilePath = Path.Combine(Application.dataPath, "advanced-popup-system/Scripts/Runtime/Core/PopupLayerEnum.cs");
            LoadEnumNames();
        }

        private void OnGUI()
        {
            GUILayout.Label("Advanced Popup System Layers", EditorStyles.boldLabel);
            GUILayout.Space(10);

            // Display current enums
            for (int i = 0; i < _enumNames.Length; i++)
            {
                GUILayout.BeginHorizontal();
                _enumNames[i] = GUILayout.TextField(_enumNames[i]);
                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    DeleteEnum(i);
                    SaveEnumChanges();
                    return;
                }
                GUILayout.EndHorizontal();

                if (i >= _enumNameChanged.Length)
                {
                    Array.Resize(ref _enumNameChanged, _enumNames.Length);
                }
                _enumNameChanged[i] = GUI.changed;
            }

            // Add new enum
            GUILayout.Space(10);
            GUILayout.Label("Add New Enum", EditorStyles.boldLabel);
            _newEnumName = GUILayout.TextField(_newEnumName);
            if (GUILayout.Button("Add"))
            {
                AddEnum(_newEnumName);
                _newEnumName = string.Empty;
                SaveEnumChanges();
            }

            // Save changes
            GUILayout.Space(20);
            if (_enumNameChanged != null && _enumNameChanged.Any(changed => changed))
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
                Array.Resize(ref _enumNameChanged, _enumNames.Length); // Убедиться, что размер массива изменен
            }
        }

        private void DeleteEnum(int index)
        {
            _enumNames = _enumNames.Where((_, i) => i != index).ToArray();
            _enumNameChanged = _enumNameChanged.Where((_, i) => i != index).ToArray();
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
