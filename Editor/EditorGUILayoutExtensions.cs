using UnityEditor;
using UnityEngine;
    
namespace AdvancedPS.Editor
{
    public static class EditorGUILayoutExtensions
    {
        public static void DrawHorizontalLine(Color color = default, float thickness = 1f, float padding = 5f, float margin = 0f)
        {
            if (color == default)
                color = new Color(1, 1, 1, 0.2f);
            
            thickness -= 0.1f;
            Rect rect = EditorGUILayout.GetControlRect(false, thickness + padding);
            rect.height = thickness;
            rect.y += padding / 2;
            rect.x += margin;
            rect.width -= margin * 2;
            EditorGUI.DrawRect(rect, color);
        }
        
        public static void DrawVerticalLine(Color color = default, float thickness = 1f, float padding = 0f, float margin = 0f)
        {
            if (color == default)
                color = new Color(1, 1, 1, 0.2f);
            
            Rect rect = EditorGUILayout.GetControlRect(false, GUILayout.ExpandHeight(true));
            rect.x += rect.width / 2f - thickness / 2f + padding / 2f;
            rect.y += margin;
            rect.width = thickness;
            rect.height -= margin * 2f;
            EditorGUI.DrawRect(rect, color);
        }
    }
}