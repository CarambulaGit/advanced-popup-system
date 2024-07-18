using System.IO;
using System.Text;
using UnityEditor;

namespace AdvancedPS.Core.System
{
    public static class APSCodeGenerator
    {
        private const string ClassName = "AdvancedPopupSystem";
        
        public static void Execute(string[] displayNames)
        {
            var code = GenerateMethodsForDisplay(ClassName, displayNames);
            var path = $"Assets/advanced-popup-system/Runtime/Generated/{ClassName}.generated.cs";

            File.WriteAllText(path, code);
            AssetDatabase.Refresh();
        }

        private static string GenerateMethodsForDisplay(string className, string[] displayNames)
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

            GeneratePopupShowGenericMethods(sb, displayNames);
            GeneratePopupHideGenericMethods(sb, displayNames);
            GenerateLayerShowGenericMethods(sb, displayNames);
            GenerateHideAllGenericMethods(sb, displayNames);

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static void GeneratePopupShowGenericMethods(StringBuilder sb, string[] displayNames)
        {
            foreach (string display in displayNames)
            {
                string displayType = display + "Display";
                string settingsType = display + "Settings";

                sb.AppendLine(
                    $"        public static Operation PopupShow<T>(PopupLayerEnum layer, {settingsType} settings = null) where T : {displayType}, new()");
                sb.AppendLine("        {");
                sb.AppendLine("            return PopupShowGeneric<T>(layer, settings);");
                sb.AppendLine("        }");
            }
        }

        private static void GeneratePopupHideGenericMethods(StringBuilder sb, string[] displayNames)
        {
            foreach (string display in displayNames)
            {
                string displayType = display + "Display";
                string settingsType = display + "Settings";

                sb.AppendLine(
                    $"        public static Operation PopupHide<T>(PopupLayerEnum layer, {settingsType} settings = null) where T : {displayType}, new()");
                sb.AppendLine("        {");
                sb.AppendLine("            return PopupHideGeneric<T>(layer, settings);");
                sb.AppendLine("        }");
            }
        }

        private static void GenerateLayerShowGenericMethods(StringBuilder sb, string[] displayNames)
        {
            foreach (string display in displayNames)
            {
                string displayType = display + "Display";
                string settingsType = display + "Settings";

                sb.AppendLine(
                    $"        public static Operation LayerShow<T>(PopupLayerEnum layer, {settingsType} settings = null) where T : {displayType}, new()");
                sb.AppendLine("        {");
                sb.AppendLine("            return LayerShowGeneric<T>(layer, settings);");
                sb.AppendLine("        }");

                foreach (string dependencySecond in displayNames)
                {
                    string displayTypeSecond = dependencySecond + "Display";
                    string settingsTypeSecond = dependencySecond + "Settings";

                    sb.AppendLine(
                        $"        public static Operation LayerShow<T,J>(PopupLayerEnum layer, {settingsType} showSettings = null, {settingsTypeSecond} hideSettings = null) where T : {displayType}, new() where J : {displayTypeSecond}, new()");
                    sb.AppendLine("        {");
                    sb.AppendLine("            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);");
                    sb.AppendLine("        }");
                }
            }
        }

        private static void GenerateHideAllGenericMethods(StringBuilder sb, string[] displayNames)
        {
            foreach (string display in displayNames)
            {
                string displayType = display + "Display";
                string settingsType = display + "Settings";

                sb.AppendLine(
                    $"        public static Operation HideAll<T>({settingsType} settings = null) where T : {displayType}, new()");
                sb.AppendLine("        {");
                sb.AppendLine("            return HideAllGeneric<T>(settings);");
                sb.AppendLine("        }");
            }
        }

    }
}