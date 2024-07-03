using System.Threading;
using System.Threading.Tasks;
using AdvancedPS.Core.System;
using AdvancedPS.Core.Utils;
using UnityEngine;

namespace AdvancedPS.Core
{
    public class SlideDisplay : IAdvancedPopupDisplay
    {
        public async Task ShowMethod(RectTransform transform, IDefaultSettings settings, CancellationToken cancellationToken = default)
        {
            if (settings is not SlideSettings settingsLocal)
            {
                APLogger.LogError($"Wrong type of display settings, should be {nameof(SlideSettings)}. Used cached one.");
                return;
            }
            
            CanvasGroup canvasGroup = GetCanvasGroup(transform);
            if (canvasGroup == null)
            {
                APLogger.LogError("AdvancedPopupDisplay not found CanvasGroup.");
                return;
            }
            
            SetCanvasGroupState(canvasGroup, true);

            Vector3 startPos = GetStartPosition(transform, settingsLocal);
            Vector3 targetPos = settingsLocal.TargetPosition == Vector3.zero ? GetCanvasCenter(transform) : settingsLocal.TargetPosition;

            float elapsedTime = 0;

            while (elapsedTime < settings.Duration)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;

                float t = elapsedTime / settings.Duration;
                transform.localPosition = Vector3.Lerp(startPos, targetPos, t);

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            // Ensure the final position is set correctly
            transform.localPosition = targetPos;
            settings.OnAnimationEnd?.Invoke();
        }

        public async Task HideMethod(RectTransform transform, IDefaultSettings settings, CancellationToken cancellationToken = default)
        {
            if (settings is not SlideSettings settingsLocal)
            {
                APLogger.LogError($"Wrong type of display settings, should be {nameof(SlideSettings)}. Used cached one.");
                return;
            }
            
            CanvasGroup canvasGroup = GetCanvasGroup(transform);
            if (canvasGroup == null)
            {
                APLogger.LogError("AdvancedPopupDisplay not found CanvasGroup.");
                return;
            }
            
            Vector3 startPos = transform.localPosition;
            Vector3 endPos = GetStartPosition(transform, settingsLocal);

            float elapsedTime = 0;

            while (elapsedTime < settings.Duration)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;

                float t = elapsedTime / settings.Duration;
                transform.localPosition = Vector3.Lerp(startPos, endPos, t);

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            // Ensure the final position is set correctly
            transform.localPosition = endPos;

            // Set CanvasGroup state to hidden
            SetCanvasGroupState(canvasGroup, false);
            settings.OnAnimationEnd?.Invoke();
        }
        
        /// <summary>
        /// Get the CanvasGroup component from the transform.
        /// </summary>
        /// <param name="transform">The transform of the popup.</param>
        /// <returns>The CanvasGroup component if it exists, null otherwise.</returns>
        private static CanvasGroup GetCanvasGroup(Component transform)
        {
            CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                APLogger.LogWarning($"CanvasGroup component missing on {transform.name}");
            }
            return canvasGroup;
        }

        /// <summary>
        /// Set the state of the CanvasGroup.
        /// </summary>
        /// <param name="canvasGroup">The CanvasGroup component of the popup.</param>
        /// <param name="state">The desired state (true for visible, false for hidden).</param>
        private static void SetCanvasGroupState(CanvasGroup canvasGroup, bool state)
        {
            canvasGroup.alpha = state ? 1 : 0;
            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;
        }
        
        private static Vector3 GetStartPosition(RectTransform transform, SlideSettings settings)
        {
            Rect rect = transform.rect;
            Vector3 startPos = settings.SlideEnum switch
            {
                SlideEnum.Up => new Vector3(0, rect.height, 0),
                SlideEnum.Down => new Vector3(0, -rect.height, 0),
                SlideEnum.Left => new Vector3(-rect.width, 0, 0),
                SlideEnum.Right => new Vector3(rect.width, 0, 0),
                _ => Vector3.zero,
            };
            return startPos;
        }

        private static Vector3 GetCanvasCenter(RectTransform transform)
        {
            Canvas canvas = transform.GetComponentInParent<Canvas>();
            if (canvas == null || canvas.renderMode == RenderMode.WorldSpace) 
                return Vector3.zero;
            
            RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
            var rect = canvasRectTransform.rect;
            return new Vector3(rect.width / 2, rect.height / 2, 0);
        }
    }
}