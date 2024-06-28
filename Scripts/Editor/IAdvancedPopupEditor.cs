using UnityEditor;
using UnityEngine;

namespace AdvancedPS.Core.Editor
{
    [CustomEditor(typeof(IAdvancedPopup), true)]
    public class IAdvancedPopupEditor : UnityEditor.Editor
    {
        private static Texture2D popupIcon;
        
        private void OnEnable()
        {
            if (popupIcon == null)
                popupIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/advanced-popup-system/Scripts/Runtime/Images/AP_LogoBlack32.png");
        }
        
        public override void OnInspectorGUI()
        {
            OnHeaderGUI();
            
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            SerializedProperty popupLayerProperty = serializedObject.FindProperty("PopupLayer");
            EditorGUILayout.PropertyField(popupLayerProperty, new GUIContent("Popup Layer"));

            if (GUILayout.Button("Edit Layers", new GUILayoutOption[] { GUILayout.Width(100), GUILayout.ExpandHeight(true) }))
            {
                PopupLayerEnumEditor.ShowWindow();
            }
            EditorGUILayout.EndHorizontal();

            DrawDefaultInspectorExcept("PopupLayer", "m_Script");

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDefaultInspectorExcept(params string[] propertyNamesToExclude)
        {
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);
            do
            {
                if (System.Array.IndexOf(propertyNamesToExclude, property.name) < 0)
                {
                    EditorGUILayout.PropertyField(property, true);
                }
            }
            while (property.NextVisible(false));
        }
 
        protected override void OnHeaderGUI()
        {
            if (popupIcon != null)
            {
                var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                rect.y -= 24;
                rect.x = 18;
                rect.xMax = 36;
                
                EditorGUI.DrawPreviewTexture(rect, popupIcon);
            }
            else
            {
                base.OnHeaderGUI();
            }
        }
    }
}