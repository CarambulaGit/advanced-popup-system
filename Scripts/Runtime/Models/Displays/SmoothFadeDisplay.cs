using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AdvancedPS.Core
{
    public class SmoothFadeDisplay : IAdvancedPopupDisplay
    {
        protected float MaxValue = 1f;
        protected float MinValue = 0f;
    
        public override async Task ShowMethod(Transform transform, DisplaySettings settings, CancellationToken cancellationToken = default)
        {
            CanvasGroup canvasGroup = GetCanvasGroup(transform);
            if (canvasGroup == null)
                return;

            float initialAlpha = canvasGroup.alpha;
            float elapsedTime = 0;

            while (elapsedTime < settings.Duration)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;

                float t = elapsedTime / settings.Duration;
                canvasGroup.alpha = Mathf.Lerp(initialAlpha, MaxValue, t);

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }   

            // Ensure the final alpha is set correctly
            SetCanvasGroupState(canvasGroup, true);
            settings.OnComplete?.Invoke();
        }

        public override async Task HideMethod(Transform transform, DisplaySettings settings, CancellationToken cancellationToken = default)
        {
            CanvasGroup canvasGroup = GetCanvasGroup(transform);
            if (canvasGroup == null)
                return;

            float initialAlpha = canvasGroup.alpha;
            float elapsedTime = 0;

            while (elapsedTime < settings.Duration)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;

                float t = elapsedTime / settings.Duration;
                canvasGroup.alpha = Mathf.Lerp(initialAlpha, MinValue, t);

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            // Ensure the final alpha is set correctly
            SetCanvasGroupState(canvasGroup, false);
            settings.OnComplete?.Invoke();
        }

        /// <summary>
        /// Get the CanvasGroup component from the transform.
        /// </summary>
        /// <param name="transform">The transform of the popup.</param>
        /// <returns>The CanvasGroup component if it exists, null otherwise.</returns>
        private static CanvasGroup GetCanvasGroup(Transform transform)
        {
            CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogWarning($"CanvasGroup component missing on {transform.name}");
            }
            return canvasGroup;
        }

        /// <summary>
        /// Set the state of the CanvasGroup.
        /// </summary>
        /// <param name="canvasGroup">The CanvasGroup component of the popup.</param>
        /// <param name="state">The desired state (true for visible, false for hidden).</param>
        private void SetCanvasGroupState(CanvasGroup canvasGroup, bool state)
        {
            canvasGroup.alpha = state ? MaxValue : MinValue;
            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;
        }
    }
}