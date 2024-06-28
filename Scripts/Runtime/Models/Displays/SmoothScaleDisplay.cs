using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AdvancedPS.Core
{
    public class SmoothScaleDisplay : IAdvancedPopupDisplay
    {
        protected Vector3 ShowScale = Vector3.one;
        protected Vector3 HideScale = Vector3.zero;

        public override async Task ShowMethod(Transform transform, DisplaySettings settings, CancellationToken cancellationToken = default)
        {
            CanvasGroup canvasGroup = GetCanvasGroup(transform);
            if (canvasGroup == null)
                return;
            
            SetCanvasGroupState(canvasGroup, true);

            Vector3 initialScale = transform.localScale;
            float elapsedTime = 0;

            while (elapsedTime < settings.Duration)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;

                float t = elapsedTime / settings.Duration;
                transform.localScale = Vector3.Lerp(initialScale, ShowScale, t);

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            // Ensure the final scale is set correctly
            transform.localScale = ShowScale;
            settings.OnComplete?.Invoke();
        }

        public override async Task HideMethod(Transform transform, DisplaySettings settings, CancellationToken cancellationToken = default)
        {
            CanvasGroup canvasGroup = GetCanvasGroup(transform);
            if (canvasGroup == null)
                return;
            
            Vector3 initialScale = transform.localScale;
            float elapsedTime = 0;

            while (elapsedTime < settings.Duration)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;

                float t = elapsedTime / settings.Duration;
                transform.localScale = Vector3.Lerp(initialScale, HideScale, t);

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            // Ensure the final scale is set correctly
            transform.localScale = HideScale;

            // Set CanvasGroup state to hidden
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
        private static void SetCanvasGroupState(CanvasGroup canvasGroup, bool state)
        {
            canvasGroup.alpha = state ? 1 : 0;
            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;
        }
    }
}