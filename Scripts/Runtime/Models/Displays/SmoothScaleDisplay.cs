using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AdvancedPS.Core
{
    public class SmoothScaleDisplay : IAdvancedPopupDisplay
    {
        protected Vector3 ShowScale;
        protected Vector3 HideScale;
        protected float AnimationSpeed;
    
        public override void InitMethod()
        {
            AnimationSpeed = 1f; // Speed of zooming per second
            ShowScale = Vector3.one;
            HideScale = Vector3.zero;
        }

        public override async Task ShowMethod(Transform transform, CancellationToken cancellationToken = default)
        {
            SetCanvasGroupState(transform, true);

            // Initialize progress tracking variables
            float progress = 0;
            float targetProgress = Vector3.Distance(ShowScale, transform.localScale);

            // Animate scaling up to the show scale
            while (progress < targetProgress)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;

                // Calculate step for each frame
                Vector3 step = (ShowScale - transform.localScale) * AnimationSpeed * Time.deltaTime;
                transform.localScale += step;
                progress += Vector3.Distance(step, Vector3.zero);

                // Yield to the next frame
                await Task.Yield();
            }

            // Ensure the final scale is set correctly
            transform.localScale = ShowScale;
        }

        public override async Task HideMethod(Transform transform, CancellationToken cancellationToken = default)
        {
            // Initialize progress tracking variables
            float progress = 0;
            float targetProgress = Vector3.Distance(HideScale, transform.localScale);

            // Animate scaling down to the hide scale
            while (progress < targetProgress)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;

                // Calculate step for each frame
                Vector3 step = (HideScale - transform.localScale) * AnimationSpeed * Time.deltaTime;
                transform.localScale += step;
                progress += Vector3.Distance(step, Vector3.zero);

                // Yield to the next frame
                await Task.Yield();
            }

            // Ensure the final scale is set correctly
            transform.localScale = HideScale;

            // Set CanvasGroup state to hidden
            SetCanvasGroupState(transform, false);
        }

        /// <summary>
        /// Set the state of the CanvasGroup.
        /// </summary>
        /// <param name="transform">The transform of the popup.</param>
        /// <param name="state">The desired state (true for visible, false for hidden).</param>
        private static void SetCanvasGroupState(Transform transform, bool state)
        {
            CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup>();
            if (canvasGroup == null) return;
            
            canvasGroup.alpha = state ? 1 : 0;
            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;
        }
    }
}