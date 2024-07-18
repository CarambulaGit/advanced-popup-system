using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
        private static Texture2D popupBunner;

        private bool isShowSettings;
        private const string IsShowSettingsKey = "APS_AutoSaveEnabled";
        
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

            if (popupBunner == null)
            {
                popupBunner = AssetDatabase.LoadAssetAtPath<Texture2D>(imagesPath + "AP_Banner.png");
            }

            if (!PlayerPrefs.HasKey(IsShowSettingsKey))
            {
                PlayerPrefs.SetInt(IsShowSettingsKey, 1);
                isShowSettings = true;
            }
            else
            {
                isShowSettings = PlayerPrefs.GetInt(IsShowSettingsKey) == 1;
            }
        }
        
        public override void OnInspectorGUI()
        {
            if (Settings.CustomIconsEnabled)
                OnHeaderGUI();
            
            serializedObject.Update();
            
            EditorGUILayout.BeginVertical(PopupSystemEditorStyles.backgroundStyle);
            EditorGUILayout.BeginHorizontal();
            SerializedProperty popupLayerProperty = serializedObject.FindProperty("PopupLayer");
            EditorGUILayout.PropertyField(popupLayerProperty, new GUIContent("Popup Layer"));

            if (GUILayout.Button("Edit Layers", new GUILayoutOption[] { GUILayout.Width(100), GUILayout.ExpandHeight(true) }))
            {
                PopupSystemEditor.ShowWindow();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(isShowSettings ? "Hide settings" : "Show settings", GUILayout.Width(100)))
            {
                isShowSettings = !isShowSettings;
                PlayerPrefs.SetInt(IsShowSettingsKey, isShowSettings ? 1 : 0);
            }
            if (GUILayout.Button(new GUIContent("Preview", 
                        EditorGUIUtility.IconContent("console.warnicon.sml").image, 
                        "-Experimental-\nPreview show & hide animation in editor."), PopupSystemEditorStyles.ExperimentalButtonStyle))
            {
                ExperimentalShowHide();
            }
            EditorGUILayout.EndHorizontal();
            if (isShowSettings)
            {
                DrawPopupSettings();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("General Settings");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                EditorGUILayoutExtensions.DrawHorizontalLine();
            }
            else
            {
                EditorGUILayoutExtensions.DrawHorizontalLine();
            }
            DrawBoolPropertiesInGrid();
            EditorGUILayout.EndVertical();

            DrawDefaultInspectorExcept(new string[] { "PopupLayer", "m_Script", "DeepPopups" }.Concat(GetBoolPropertyNames()).ToArray());
            
            DrawDeepPopupsProperty();

            serializedObject.ApplyModifiedProperties();
        }

        private async void ExperimentalShowHide()
        {
            var targetType = target.GetType();
            var methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            var showMethod = methods.FirstOrDefault(m => m.Name == "ShowAsync" && !m.IsGenericMethod);
            var hideMethod = methods.FirstOrDefault(m => m.Name == "HideAsync" && !m.IsGenericMethod);
            
            if (showMethod == null || hideMethod == null)
            {
                Debug.LogError("Not found methods 'ShowAsync' or/and 'HideAsync', preview was safely aborted.");
                return;
            }
            
            var popup = (IAdvancedPopup)target;
            bool wasActive = popup.gameObject.activeSelf;
            bool wasVisible = popup.GetComponent<CanvasGroup>().alpha != 0;
            
            if (!wasActive)
                popup.gameObject.SetActive(true);

            if (wasVisible)
            {
                await (Task)hideMethod.Invoke(target, new object[] { default, null, false });
                await (Task)showMethod.Invoke(target, new object[] { default, null, false });
            }
            else
            {
                await (Task)showMethod.Invoke(target, new object[] { default, null, false });
                await (Task)hideMethod.Invoke(target, new object[] { default, null, false });
            }
            
            if (!wasActive)
                popup.gameObject.SetActive(false);
        }

        private void DrawPopupSettings()
        {
            EditorGUILayoutExtensions.DrawHorizontalLine();
            GUILayout.BeginHorizontal();
            
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Show Settings");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorGUILayoutExtensions.DrawHorizontalLine();
            
            GUILayout.Space(200);
            GUILayout.EndVertical();
            
            EditorGUILayoutExtensions.DrawVerticalLine();
            
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Hide Settings");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorGUILayoutExtensions.DrawHorizontalLine();
            
            GUILayout.Space(200);
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();
            EditorGUILayoutExtensions.DrawHorizontalLine();
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

            do
            {
                if (property.propertyType == SerializedPropertyType.Boolean)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(property.displayName, GUILayout.Width(150));
                    property.boolValue = EditorGUILayout.Toggle(property.boolValue);
                    EditorGUILayout.EndHorizontal();
                }
            }
            while (property.NextVisible(false));
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
                float availableWidth = Screen.width;
                float aspectRatio = (float)popupBunner.width / popupBunner.height;
                float bannerHeight = availableWidth / aspectRatio - 75;
                
                var rectBanner = EditorGUILayout.GetControlRect(false, bannerHeight, GUILayout.ExpandWidth(true));
                rectBanner.y -= 5;
                rectBanner.height += 30;
                
                GUI.DrawTexture(rectBanner, popupBunner, ScaleMode.ScaleToFit);
                
                var rectIcon = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                rectIcon.y -= 26 + bannerHeight;
                rectIcon.x = 18;
                rectIcon.xMax = 36;
                EditorGUI.DrawPreviewTexture(rectIcon, popupIcon);
            }
            else
            {
                base.OnHeaderGUI();
            }
        }
    }
}