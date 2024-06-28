using UnityEngine;

namespace AdvancedPS.Core.Editor.Styles
{
    public static class PopupLayerEnumEditorStyles
    {
        public static GUIStyle ToggleStyle;

        static PopupLayerEnumEditorStyles()
        {
            ToggleStyle = new GUIStyle(GUI.skin.toggle);
            ToggleStyle.normal.textColor = Color.white;
            ToggleStyle.onNormal.textColor = Color.white;
            ToggleStyle.onActive.textColor = Color.white;
            ToggleStyle.onFocused.textColor = Color.white;
            ToggleStyle.onHover.textColor = Color.white;
            ToggleStyle.active.textColor = Color.white;
            ToggleStyle.focused.textColor = Color.white;
            ToggleStyle.hover.textColor = Color.white;
            ToggleStyle.fontSize = 13;
            
            ToggleStyle.contentOffset = new Vector2(-10, 0);
        }
    }
}