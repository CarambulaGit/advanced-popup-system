using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using AdvancedPS.Core.System;
using AdvancedPS.Core.Utils;
using AdvancedPS.Editor.Styles;
using UnityEditor;
using UnityEngine;

namespace AdvancedPS.Editor
{
    public class PopupDisplaysEditorPanel : EditorWindow
    {
        private static string[] _displayNames;
        private static bool[] _displayNameChanged;
        private static string displayFilePath;
        private static Vector2 scrollPosition;

        public static void Initialize()
        {
            displayFilePath = FileSearcher.DisplaysDependenciesFilePath;
            LoadEnumNames();
        }
        
        public static void OnGUI()
        {
            GUILayout.BeginVertical();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, PopupSystemEditorStyles.ScrollViewStyle);

            Array.Resize(ref _displayNameChanged, _displayNames.Length);
            for (int i = 0; i < _displayNames.Length; i++)
            {
                GUILayout.BeginHorizontal();
                
                string newEnumName = EditorGUILayout.DelayedTextField(_displayNames[i]);
                if (newEnumName != _displayNames[i])
                {
                    newEnumName = ValidateAndFormatDisplayName(newEnumName);
                    if (newEnumName != null)
                    {
                        _displayNames[i] = newEnumName;
                        _displayNameChanged[i] = true;
                    }
                    else
                    {
                        APLogger.LogWarning("Invalid display name.");
                    }
                }
                
                if (string.IsNullOrEmpty(newEnumName))
                {
                    if (GUILayout.Button("Done", GUILayout.Width(60)))
                    {
                        GUI.FocusControl(null);
                    }
                }
                else
                {
                    if (GUILayout.Button("Delete", GUILayout.Width(60)))
                    {
                        DeleteDisplay(i);
                    }
                }
                
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            if (_displayNames.All(s => !string.IsNullOrEmpty(s)))
            {
                if (GUILayout.Button("+", PopupSystemEditorStyles.BoldButtonStyle,GUILayout.Height(15)))
                { 
                    AddDisplay(string.Empty);
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayoutExtensions.DrawHorizontalLine();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUI.enabled = _displayNameChanged.Any(changed => changed);
            if (GUILayout.Button("Save", GUILayout.Width(80)))
            {
                SaveDisplayChanges();
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();
        }

        private static void LoadEnumNames()
        {
            _displayNames = Assembly.GetAssembly(typeof(IDisplay)).GetTypes()
                .Where(t => typeof(IDisplay).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t => t.Name).ToArray();
            _displayNameChanged = new bool[_displayNames.Length];
        }

        private static void AddDisplay(string enumName)
        {
            if (enumName != null && !_displayNames.Contains(enumName))
            {
                Array.Resize(ref _displayNames, _displayNames.Length + 1);
                _displayNames[_displayNames.Length - 1] = enumName;
                Array.Resize(ref _displayNameChanged, _displayNames.Length);
            }
        }

        private static void DeleteDisplay(int index)
        {
            _displayNames = _displayNames.Where((_, i) => i != index).ToArray();
            _displayNameChanged = _displayNameChanged.Where((_, i) => i != index).ToArray();
        }

        private static string ValidateAndFormatDisplayName(string enumName)
        {
            if (enumName == null)
                return string.Empty;

            enumName = Regex.Replace(enumName, @"\s+", "_");
            enumName = Regex.Replace(enumName, "_+", "_");
            enumName = enumName.ToUpper();

            return !Regex.IsMatch(enumName, @"^[A-Z0-9_]+$") ? null : enumName;
        }

        private static string RemoveDisplaySuffix(string input)
        {
            string[] suffixes = { "display", "Display", "displays", "Displays", "settings", "Settings", "Setting", "setting"};
            return suffixes.Aggregate(input, (current, suffix) => Regex.Replace(current, suffix, "", RegexOptions.IgnoreCase));
        }
        
        private static void SaveDisplayChanges()
        {
            if (!File.Exists(displayFilePath))
            {
                APLogger.LogError($"File not found: {displayFilePath}");
                return;
            }

            StringBuilder enumFileContent = new StringBuilder();

            enumFileContent.AppendLine("using System.Collections.Generic;");
            enumFileContent.AppendLine("namespace AdvancedPS.Core.System");
            enumFileContent.AppendLine("{");
            enumFileContent.AppendLine("    public static class APS_Dependencies");
            enumFileContent.AppendLine("    {");
            enumFileContent.AppendLine("        public static readonly Dictionary<IDisplay, IDefaultSettings> DisplaySettingsDependency =");
            enumFileContent.AppendLine("            new Dictionary<IDisplay, IDefaultSettings>()");
            enumFileContent.AppendLine("            {");

            foreach (var display in _displayNames)
            {
                if (string.IsNullOrEmpty(display))
                    continue;

                string displayName = RemoveDisplaySuffix(display);
                string displayClass = displayName + "Display";
                string settingsClass = displayName + "Settings";
        
                if (GetTypeByName(displayClass) == null)
                {
                    CreateDisplayAndSettingsFiles(displayName);
                }
                
                enumFileContent.AppendLine($"           {{ new {displayClass}(), new {settingsClass}() }},");
            }
            
            enumFileContent.AppendLine("            };");
            enumFileContent.AppendLine("    }");
            enumFileContent.AppendLine("}");

            File.WriteAllText(displayFilePath, enumFileContent.ToString());
            AssetDatabase.Refresh();
        }
        
        private static void CreateDisplayAndSettingsFiles(string displayName)
        {
            string displayFolderPath = Path.Combine(FileSearcher.DisplaysFolderPath, displayName + "Display");
            if (!Directory.Exists(displayFolderPath))
            {
                Directory.CreateDirectory(displayFolderPath);
            }

            string displayPath = Path.Combine(displayFolderPath, displayName + "Display.generated.cs");
            string settingsPath = Path.Combine(displayFolderPath, displayName + "Settings.generated.cs");

            File.WriteAllText(displayPath, GenerateDisplayClass(displayName + "Display", displayName + "Settings"));
            File.WriteAllText(settingsPath, GenerateSettingsClass(displayName + "Settings"));

            AssetDatabase.Refresh();
        }

        private static string GenerateDisplayClass(string className, string settingsName)
        {
            return $@"
using System.Threading;
using System.Threading.Tasks;
using AdvancedPS.Core.System;
using AdvancedPS.Core.Utils;
using UnityEngine;

namespace AdvancedPS.Core
{{
    public class {className} : IDisplay
    {{
        public async Task ShowMethod(RectTransform transform, IDefaultSettings settings, CancellationToken cancellationToken = default)
        {{
            {settingsName} settingsLocal = settings as {settingsName};
            CanvasGroup canvasGroup = GetCanvasGroup(transform);
        }}
        
        public async Task HideMethod(RectTransform transform, IDefaultSettings settings, CancellationToken cancellationToken = default)
        {{
            {settingsName} settingsLocal = settings as {settingsName};
            CanvasGroup canvasGroup = GetCanvasGroup(transform);
        }}
    }}
}}
";
        }

        private static string GenerateSettingsClass(string className)
        {
            return $@"
using AdvancedPS.Core.System;

namespace AdvancedPS.Core
{{
    public class {className} : IDefaultSettings
    {{
        public {className}()
        {{
        }}
    }}
}}
";
        }

        private static Type GetTypeByName(string typeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(type => type.Name == typeName);
        }
    }
}