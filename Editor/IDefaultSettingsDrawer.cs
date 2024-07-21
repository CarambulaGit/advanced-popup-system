using AdvancedPS.Core.System;
using UnityEditor;
using UnityEngine;

namespace AdvancedPS.Editor
{
    [CustomPropertyDrawer(typeof(DefaultSettings))]
    public class IDefaultSettingsDrawer : PropertyDrawer
    {
        static string[] propNames = new[]
        {
            "Duration",
            "Easing",
            "OnAnimationEnd",
        };
        
         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float y = position.y;

            SerializedProperty[] props = new SerializedProperty[propNames.Length];
            for (int i = 0; i < props.Length; i++)
            {
                props[i] = property.FindPropertyRelative(propNames[i]);
                if (props[i] == null)
                {
                    Debug.LogError($"Property '{propNames[i]}' not found on '{property.displayName}'");
                    EditorGUI.EndProperty();
                    return;
                }

                GUIContent labelContent = new GUIContent(props[i].displayName);
                
                Rect labelRect = new Rect(position.x, y, position.width, lineHeight);
                EditorGUI.LabelField(labelRect, labelContent);
                y += lineHeight + spacing;

                float fieldHeight = EditorGUI.GetPropertyHeight(props[i], true);
                Rect fieldRect = new Rect(position.x, y, position.width, fieldHeight);

                EditorGUI.PropertyField(fieldRect, props[i], GUIContent.none);
                y += fieldHeight + spacing;
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = 0f;
            SerializedProperty[] props = new SerializedProperty[propNames.Length];

            for (int i = 0; i < props.Length; i++)
            {
                props[i] = property.FindPropertyRelative(propNames[i]);
                if (props[i] != null)
                {
                    GUIContent labelContent = new GUIContent(props[i].displayName);
                    Vector2 labelSize = EditorStyles.label.CalcSize(labelContent);
                    bool isLabelTooWide = labelSize.x > 50f;

                    float fieldHeight = EditorGUI.GetPropertyHeight(props[i], true);
                    totalHeight += fieldHeight + EditorGUIUtility.standardVerticalSpacing;
                    if (isLabelTooWide)
                    {
                        totalHeight += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                }
            }

            return totalHeight;
        }
    }
}