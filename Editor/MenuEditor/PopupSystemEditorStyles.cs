using UnityEngine;

namespace AdvancedPS.Core.Editor.Styles
{
    public static class PopupSystemEditorStyles
    {
        public static readonly GUIStyle ToggleStyle;
        public static readonly GUIStyle SelectedTabStyle;
        public static readonly GUIStyle NormalTabStyle;
        public static readonly GUIStyle ScrollViewStyle;
        
        static PopupSystemEditorStyles()
        {
            ToggleStyle = new GUIStyle(GUI.skin.toggle)
            {
                fontSize = 13,
                contentOffset = new Vector2(-10, 0)
            };
            Texture2D selectedTexture = new Texture2D(1, 1);
            selectedTexture.SetPixel(0, 0, Color.white);
            selectedTexture.Apply();
            
            Texture2D normalTexture = new Texture2D(1, 1);
            normalTexture.SetPixel(0, 0, Color.gray);
            normalTexture.Apply();
            
            SelectedTabStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(4, 4, 4, 4),
                normal = { background = selectedTexture, textColor = Color.white },
                focused = { background = selectedTexture, textColor = Color.white },
                active = { background = selectedTexture, textColor = Color.white },
                hover = { background = selectedTexture, textColor = Color.white },
            };

            NormalTabStyle = new GUIStyle(GUI.skin.button)
            {
                border = new RectOffset(2, 2, 2, 2),
                normal = { background = normalTexture, textColor = Color.gray },
                focused = { background = normalTexture, textColor = Color.gray },
                active = { background = normalTexture, textColor = Color.gray },
                hover = { background = normalTexture, textColor = Color.gray },
            };
            
            ScrollViewStyle = new GUIStyle(GUI.skin.scrollView)
            {
                stretchHeight = true,
                stretchWidth = true,
            };
        }
    }
}