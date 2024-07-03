using System.Threading;
using System.Threading.Tasks;
using AdvancedPS.Core.System;
using AdvancedPS.Core.Utils;
using UnityEngine;

namespace AdvancedPS.Core
{
    public class ScaleDisplay : IAdvancedPopupDisplay
    {
        public async Task ShowMethod(RectTransform transform, IDefaultSettings settings, CancellationToken cancellationToken = default)
        {
            if (settings is not ScaleSettings settingsLocal)
            {
                APLogger.LogError($"Wrong type of display settings, should be {nameof(ScaleSettings)}. Used cached one.");
                return;
            }
            
            CanvasGroup canvasGroup = GetCanvasGroup(transform);
            if (canvasGroup == null)
            {
                APLogger.LogError("AdvancedPopupDisplay not found CanvasGroup.");
                return;
            }
            
            SetCanvasGroupState(canvasGroup, true);

            Vector3 initialScale = transform.localScale;
            float elapsedTime = 0;

            while (elapsedTime < settings.Duration)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;

                float t = elapsedTime / settings.Duration;
                transform.localScale = Vector3.Lerp(initialScale, settingsLocal.ShowScale, t);

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            // Ensure the final scale is set correctly
            transform.localScale = settingsLocal.ShowScale;
            settings.OnAnimationEnd?.Invoke();
        }

        public async Task HideMethod(RectTransform transform, IDefaultSettings settings, CancellationToken cancellationToken = default)
        {
            if (settings is not ScaleSettings settingsLocal)
            {
                APLogger.LogError($"Wrong type of display settings, should be {nameof(ScaleSettings)}. Used cached one.");
                return;
            }
            
            CanvasGroup canvasGroup = GetCanvasGroup(transform);
            if (canvasGroup == null)
            {
                APLogger.LogError("AdvancedPopupDisplay not found CanvasGroup.");
                return;
            }
            
            Vector3 initialScale = transform.localScale;
            float elapsedTime = 0;

            while (elapsedTime < settings.Duration)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;

                float t = elapsedTime / settings.Duration;
                transform.localScale = Vector3.Lerp(initialScale, settingsLocal.HideScale, t);

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            // Ensure the final scale is set correctly
            transform.localScale = settingsLocal.HideScale;

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
    }
}