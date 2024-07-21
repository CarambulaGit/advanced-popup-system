﻿using System.Threading;
using System.Threading.Tasks;
using AdvancedPS.Core.System;
using AdvancedPS.Core.Utils;
using UnityEngine;

namespace AdvancedPS.Core
{
    public class FadeDisplay : IDisplay
    {
        public async Task ShowMethod(RectTransform transform, DefaultSettings settings, CancellationToken cancellationToken = default)
        {
            FadeSettings settingsLocal = settings as FadeSettings; 
            CanvasGroup canvasGroup = GetCanvasGroup(transform);
            
            transform.localScale = Vector3.one;

            float initialAlpha = canvasGroup.alpha;
            float elapsedTime = 0;

            while (elapsedTime < settingsLocal.Duration)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;
                
                float t = elapsedTime / settingsLocal.Duration;
                float easedT = EasingFunctions.GetEasingValue(settingsLocal.Easing, t);
                
                canvasGroup.alpha = Mathf.LerpUnclamped(initialAlpha, settingsLocal.MaxValue, easedT);

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }   

            // Ensure the final alpha is set correctly
            SetCanvasGroupState(canvasGroup, settingsLocal, true);
            settingsLocal.OnAnimationEnd?.Invoke();
        }

        public async Task HideMethod(RectTransform transform, DefaultSettings settings, CancellationToken cancellationToken = default)
        {
            FadeSettings settingsLocal = settings as FadeSettings; 
            CanvasGroup canvasGroup = GetCanvasGroup(transform);

            float initialAlpha = canvasGroup.alpha;
            float elapsedTime = 0;

            while (elapsedTime < settingsLocal.Duration)
            {
                if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
                    return;
                
                float t = elapsedTime / settingsLocal.Duration;
                float easedT = EasingFunctions.GetEasingValue(settingsLocal.Easing, t);
                
                canvasGroup.alpha = Mathf.LerpUnclamped(initialAlpha, settingsLocal.MinValue, easedT);

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            // Ensure the final alpha is set correctly
            SetCanvasGroupState(canvasGroup, settingsLocal, false);
            settingsLocal.OnAnimationEnd?.Invoke();
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
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="state">The desired state (true for visible, false for hidden).</param>
        private void SetCanvasGroupState(CanvasGroup canvasGroup, FadeSettings settings, bool state)
        {
            canvasGroup.alpha = state ? settings.MaxValue : settings.MinValue;
            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;
        }
    }
}