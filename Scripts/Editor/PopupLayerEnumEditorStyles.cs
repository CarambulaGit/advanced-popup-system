using UnityEngine;

namespace AdvancedPS.Core.Editor.Styles
{
    public static class PopupLayerEnumEditorStyles
    {
        public static readonly GUIStyle ToggleStyle;

        static PopupLayerEnumEditorStyles()
        {
            ToggleStyle = new GUIStyle(GUI.skin.toggle)
            {
                fontSize = 13,
                contentOffset = new Vector2(-10, 0)
            };
        }
    }
}