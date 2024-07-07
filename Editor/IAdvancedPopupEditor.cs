using System.Linq;
using AdvancedPS.Core.System;
using AdvancedPS.Core.Utils;
using AdvancedPS.Editor.Styles;
using UnityEditor;
using UnityEngine;

namespace AdvancedPS.Editor
{
    [CustomEditor(typeof(IAdvancedPopup), true)]
    public class IAdvancedPopupEditor : UnityEditor.Editor
    {
        private PopupSettings Settings;
        
        private static string imagesPath;
        private static Texture2D popupIcon;
        
        private void OnEnable()
        {
            if (Settings == null)
            {
                Settings = SettingsManager.Settings;
            }
            
            if (popupIcon == null)
            {
                imagesPath = FileSearcher.ImagesFolderPath;
                popupIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(imagesPath + "AP_LogoBlack32.png");
            }
        }
        
        public override void OnInspectorGUI()
        {
            if (Settings.CustomIconsEnabled)
                OnHeaderGUI();
            
            serializedObject.Update();
            
            EditorGUILayout.BeginHorizontal();
            SerializedProperty popupLayerProperty = serializedObject.FindProperty("PopupLayer");
            EditorGUILayout.PropertyField(popupLayerProperty, new GUIContent("Popup Layer"));

            if (GUILayout.Button("Edit Layers", new GUILayoutOption[] { GUILayout.Width(100), GUILayout.ExpandHeight(true) }))
            {
                PopupSystemEditor.ShowWindow();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(PopupSystemEditorStyles.backgroundStyle);
            DrawBoolPropertiesInGrid();
            EditorGUILayout.EndVertical();

            DrawDefaultInspectorExcept(new string[] { "PopupLayer", "m_Script", "DeepPopups" }.Concat(GetBoolPropertyNames()).ToArray());
            
            DrawDeepPopupsProperty();

            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawDefaultInspectorExcept(string[] propertyNamesToExclude)
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
        
        private void DrawDeepPopupsProperty()
        {
            SerializedProperty deepPopupsProperty = serializedObject.FindProperty("DeepPopups");
            if (deepPopupsProperty != null)
            {
                EditorGUILayout.PropertyField(deepPopupsProperty, true);
            }
        }
        
        private void DrawBoolPropertiesInGrid()
        {
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);

            int columnCount = 2;
            int currentColumn = 0;

            EditorGUILayout.BeginHorizontal();
            do
            {
                if (property.propertyType == SerializedPropertyType.Boolean)
                {
                    EditorGUILayout.PropertyField(property, true);

                    currentColumn++;
                    if (currentColumn >= columnCount)
                    {
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        currentColumn = 0;
                    }
                }
            }
            while (property.NextVisible(false));
            EditorGUILayout.EndHorizontal();
        }
        
        private string[] GetBoolPropertyNames()
        {
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);
            var boolPropertyNames = new System.Collections.Generic.List<string>();

            do
            {
                if (property.propertyType == SerializedPropertyType.Boolean)
                {
                    boolPropertyNames.Add(property.name);
                }
            }
            while (property.NextVisible(false));

            return boolPropertyNames.ToArray();
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