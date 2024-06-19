using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AdvancedPS.Core
{
    public class SmoothFadeDisplay : IAdvancedPopupDisplay
    {
        protected float MaxValue;
        protected float MinValue;
        protected float AnimationSpeed;
    
        public override void InitMethod()
        {
            MaxValue = 1f;
            MinValue = 0f;
            AnimationSpeed = 1f; // Speed changing of alpha per second
        }
    
        public override async Task ShowMethod(Transform transform, CancellationToken cancellationToken = default)
        {
            CanvasGroup canvasGroup = GetCanvasGroup(transform);
            if (canvasGroup == null)
                return;

            // Initialize progress tracking variables
            float progress = canvasGroup.alpha;
            float targetProgress = MaxValue;

            // Animate fading in
            while (progress < targetProgress)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;

                // Calculate step for each frame
                float step = AnimationSpeed * Time.deltaTime;
                canvasGroup.alpha = Mathf.Min(canvasGroup.alpha + step, MaxValue);
                progress = canvasGroup.alpha;

                // Yield to the next frame
                await Task.Yield();
            }

            // Ensure the final alpha is set correctly
            canvasGroup.alpha = MaxValue;
            SetCanvasGroupState(canvasGroup, true);
        }

        public override async Task HideMethod(Transform transform, CancellationToken cancellationToken = default)
        {
            CanvasGroup canvasGroup = GetCanvasGroup(transform);
            if (canvasGroup == null)
                return;

            // Initialize progress tracking variables
            float progress = canvasGroup.alpha;
            float targetProgress = MinValue;

            // Animate fading out
            while (progress > targetProgress)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;

                // Calculate step for each frame
                float step = AnimationSpeed * Time.deltaTime;
                canvasGroup.alpha = Mathf.Max(canvasGroup.alpha - step, MinValue);
                progress = canvasGroup.alpha;

                // Yield to the next frame
                await Task.Yield();
            }

            // Ensure the final alpha is set correctly
            canvasGroup.alpha = MinValue;
            SetCanvasGroupState(canvasGroup, false);
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
            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;
        }
    }
}