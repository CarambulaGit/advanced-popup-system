using AdvancedPS.Core.Enum;
using UnityEditor;
using UnityEngine;

namespace AdvancedPS.Core
{
    [CustomEditor(typeof(IAdvancedPopup), true)]
    public class IAdvancedPopupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            SerializedProperty popupLayerProperty = serializedObject.FindProperty("PopupLayer");
            EditorGUILayout.PropertyField(popupLayerProperty, new GUIContent("Popup Layer"));

            if (GUILayout.Button("Edit Layers", new GUILayoutOption[]
                {
                    GUILayout.Width(100),
                    GUILayout.ExpandHeight(true)
                }))
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
    }
}