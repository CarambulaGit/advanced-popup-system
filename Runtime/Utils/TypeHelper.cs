using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdvancedPS.Core.Utils
{
    public static class TypeHelper
    {
        public static Type GetDisplaySettings(string displayName)
        {
            string settingsClassName = RemoveDisplaySuffix(displayName) + "Settings";
            return Type.GetType(settingsClassName);
        }
        
        public static Type GetDisplay(string displayName)
        {
            string settingsClassName = RemoveDisplaySuffix(displayName) + "Display";
            return Type.GetType(settingsClassName);
        }
        
        public static Type GetTypeByFullName(string typeFullName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(type => type.FullName == typeFullName);
        }
        public static Type GetTypeByName(string typeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(type => type.Name == typeName);
        }
        
        public static string RemoveDisplaySuffix(string input)
        {
            string[] suffixes =
                { "display", "Display", "displays", "Displays", "settings", "Settings", "Setting", "setting" };
            return suffixes.Aggregate(input,
                (current, suffix) => Regex.Replace(current, suffix, "", RegexOptions.IgnoreCase));
        }
    }
}