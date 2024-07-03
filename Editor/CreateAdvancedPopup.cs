using AdvancedPS.Core.Popup;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedPS.Core.Editor
{
    public static class CreateAdvancedPopup
    {
        [MenuItem("GameObject/UI/Advanced Popup", false, 10)]
        private static void CreateNewAdvancedPopup()
        {
            GameObject newPopup = new GameObject("Advanced Popup");
            RectTransform rectTransform = newPopup.AddComponent<RectTransform>();
            newPopup.AddComponent<CanvasRenderer>();
            newPopup.AddComponent<Image>();
            AdvancedPopup popup = newPopup.AddComponent<AdvancedPopup>();
            popup.RootTransform = rectTransform;
            
            Transform parentTransform = Selection.activeTransform;
            
            Canvas canvas = parentTransform.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObject = new GameObject("Canvas");
                canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObject.AddComponent<CanvasScaler>();
                canvasObject.AddComponent<GraphicRaycaster>();

                newPopup.transform.SetParent(canvas.transform, false);
            }
            else
            {
                newPopup.transform.SetParent(parentTransform, false);
            }
            
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            
            Selection.activeGameObject = newPopup;
        }
    }
}