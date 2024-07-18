using System.IO;
using System.Text;
using UnityEditor;

namespace AdvancedPS.Core.System
{
    public static class APSCodeGenerator
    {
        public static void Execute()
        {
            var className = "AdvancedPopupSystem";
            var code = GenerateMethodsForDisplay(className);
            var path = $"Assets/advanced-popup-system/Runtime/Generated/{className}.generated.cs";

            File.WriteAllText(path, code);
            AssetDatabase.Refresh();
        }

        private static string GenerateMethodsForDisplay(string className)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using AdvancedPS.Core.System;");
            sb.AppendLine("using System.Threading;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using AdvancedPS.Core.Utils;");
            sb.AppendLine("using System;");

            sb.AppendLine("namespace AdvancedPS.Core");
            sb.AppendLine("{");
            sb.AppendLine($"    public static partial class {className}");
            sb.AppendLine("    {");

            GeneratePopupShowGenericMethods(sb);
            GeneratePopupHideGenericMethods(sb);
            GenerateLayerShowGenericMethods(sb);
            GenerateHideAllGenericMethods(sb);

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static void GeneratePopupShowGenericMethods(StringBuilder sb)
        {
            var dependencies = APS_Dependencies.DisplaySettingsDependency;
            foreach (var dependency in dependencies)
            {
                var displayType = dependency.Key.GetType().Name;
                var settingsType = dependency.Value.GetType().Name;

                sb.AppendLine(
                    $"        public static Operation PopupShow<T>(PopupLayerEnum layer, {settingsType} settings = null) where T : {displayType}, new()");
                sb.AppendLine("        {");
                sb.AppendLine("            return PopupShowGeneric<T>(layer, settings);");
                sb.AppendLine("        }");
            }
        }

        private static void GeneratePopupHideGenericMethods(StringBuilder sb)
        {
            var dependencies = APS_Dependencies.DisplaySettingsDependency;
            foreach (var dependency in dependencies)
            {
                var displayType = dependency.Key.GetType().Name;
                var settingsType = dependency.Value.GetType().Name;

                sb.AppendLine(
                    $"        public static Operation PopupHide<T>(PopupLayerEnum layer, {settingsType} settings = null) where T : {displayType}, new()");
                sb.AppendLine("        {");
                sb.AppendLine("            return PopupHideGeneric<T>(layer, settings);");
                sb.AppendLine("        }");
            }
        }

        private static void GenerateLayerShowGenericMethods(StringBuilder sb)
        {
            var dependencies = APS_Dependencies.DisplaySettingsDependency;
            foreach (var dependency in dependencies)
            {
                var displayType = dependency.Key.GetType().Name;
                var settingsType = dependency.Value.GetType().Name;

                sb.AppendLine(
                    $"        public static Operation LayerShow<T>(PopupLayerEnum layer, {settingsType} settings = null) where T : {displayType}, new()");
                sb.AppendLine("        {");
                sb.AppendLine("            return LayerShowGeneric<T>(layer, settings);");
                sb.AppendLine("        }");

                foreach (var dependencySecond in dependencies)
                {
                    var displayTypeSecond = dependencySecond.Key.GetType().Name;
                    var settingsTypeSecond = dependencySecond.Value.GetType().Name;

                    sb.AppendLine(
                        $"        public static Operation LayerShow<T,J>(PopupLayerEnum layer, {settingsType} showSettings = null, {settingsTypeSecond} hideSettings = null) where T : {displayType}, new() where J : {displayTypeSecond}, new()");
                    sb.AppendLine("        {");
                    sb.AppendLine("            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);");
                    sb.AppendLine("        }");
                }
            }
        }

        private static void GenerateHideAllGenericMethods(StringBuilder sb)
        {
            var dependencies = APS_Dependencies.DisplaySettingsDependency;
            foreach (var dependency in dependencies)
            {
                var displayType = dependency.Key.GetType().Name;
                var settingsType = dependency.Value.GetType().Name;

                sb.AppendLine(
                    $"        public static Operation HideAll<T>({settingsType} settings = null) where T : {displayType}, new()");
                sb.AppendLine("        {");
                sb.AppendLine("            return HideAllGeneric<T>(settings);");
                sb.AppendLine("        }");
            }
        }

    }
}