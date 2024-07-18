using AdvancedPS.Core.Utils;
using AdvancedPS.Editor.Styles;
using UnityEditor;
using UnityEngine;

namespace AdvancedPS.Editor
{
    public class PopupSystemEditor : EditorWindow
    {
        private enum Tab
        {
            Layers,
            Settings
        }
        private string imagesPath;
        
        private Tab currentTab = Tab.Layers;
        
        private Texture2D bannerTexture;
        private Texture2D iconTexture;
        private Texture2D blackTexture;
        
        [MenuItem("Tools/Advanced Popup System Menu")]
        public static void ShowWindow()
        {
            var window = GetWindow<PopupSystemEditor>("Popup System Editor");
            window.titleContent = new GUIContent("Popup System Editor");
        }

        private void OnEnable()
        {
            minSize = new Vector2(300, 450);
            
            imagesPath = FileSearcher.ImagesFolderPath;
            bannerTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(imagesPath + "AP_bundleBlack.png");
            iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(imagesPath + "AP_LogoBlack.png");
            if (iconTexture != null)
            {
                blackTexture = new Texture2D(1, 1);
                blackTexture.SetPixel(0, 0, Color.black);
                blackTexture.Apply();
            }
            
            // Initialize and load necessary resources
            PopupLayerEditorPanel.Initialize();
            PopupSettingsEditor.Initialize();
        }

        private void OnGUI()
        {
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.white;
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), Color.black);

            DrawTabs();
            
            if (bannerTexture != null)
            {
                const float bannerWidth = 256;
                const float bannerHeight = 128;
                Rect blackBackgroundRect = GUILayoutUtility.GetRect(position.width, bannerHeight);
                GUI.DrawTexture(blackBackgroundRect, blackTexture);

                Rect bannerRect = new Rect((position.width - bannerWidth) / 2, blackBackgroundRect.y, bannerWidth, bannerHeight);
                GUI.DrawTexture(bannerRect, bannerTexture);

                GUILayout.Space(blackBackgroundRect.height);
            }

            EditorGUILayoutExtensions.DrawHorizontalLine(padding: 20);

            switch (currentTab)
            {
                case Tab.Layers:
                    PopupLayerEditorPanel.OnGUI();
                    break;
                case Tab.Settings:
                    PopupSettingsEditor.OnGUI();
                    break;
            }
        }

        private void DrawTabs()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Layers", currentTab == Tab.Layers ? PopupSystemEditorStyles.SelectedTabStyle : PopupSystemEditorStyles.NormalTabStyle))
            {
                currentTab = Tab.Layers;
            }
            if (GUILayout.Button("Settings", currentTab == Tab.Settings ? PopupSystemEditorStyles.SelectedTabStyle : PopupSystemEditorStyles.NormalTabStyle))
            {
                currentTab = Tab.Settings;
            }
            GUILayout.EndHorizontal();
        }
    }
}
