using UnityEditor;
using UnityEngine;

namespace AdvancedPS.Core.Editor
{
    [InitializeOnLoad]
    public static class CustomInspectorIcon
    {
        private static Texture2D popupIcon;

        static CustomInspectorIcon()
        {
            popupIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/advanced-popup-system/Scripts/Runtime/Images/AP_LogoBlack32.png");
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj is GameObject go && go.GetComponent<IAdvancedPopup>() != null)
            {
                Rect rect = new Rect(selectionRect.x + selectionRect.width - 16, selectionRect.y, 16, 16);
                GUI.Label(rect, new GUIContent(popupIcon));
            }
        }
    }
}
