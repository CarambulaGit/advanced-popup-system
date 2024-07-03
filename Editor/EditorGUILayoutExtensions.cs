using UnityEditor;
using UnityEngine;
    
namespace AdvancedPS.Core.Editor
{
    public static class EditorGUILayoutExtensions
    {
        public static void DrawHorizontalLine(Color color, float thickness = 1f, float padding = 20f, float margin = 10f)
        {
            thickness -= 0.1f;
            Rect rect = EditorGUILayout.GetControlRect(false, thickness + padding);
            rect.height = thickness;
            rect.y += padding / 2;
            rect.x += margin;
            rect.width -= margin * 2;
            EditorGUI.DrawRect(rect, color);
        }
    }
}