using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedPS.Core.Popup
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AdvancedPopup : IAdvancedPopup
    {
        /// <summary>
        /// For public events.
        /// </summary>
        public Action OnShowing;
        
        /// <summary>
        /// For public events.
        /// </summary>
        public Action OnHided;
        
        /// <summary>
        /// Leave it empty if don't need.
        /// </summary>
        [Tooltip("Leave it empty if don't need.")]
        public Button closeButton;
        
        [HideInInspector] public CanvasGroup canvasGroup;

        private bool _awaitingHide;
        private bool _subscribed;

        /// <summary>
        /// Initialize the popup.
        /// To define the default window animation - invoke SetCachedDisplay method here. 
        /// </summary>
        public override void Init()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            base.Init();
        }

        /// <summary>
        /// Handler for close button press event.
        /// </summary>
        public virtual void OnCloseButtonPress()
        {
            Hide(default, true);
        }

        /// <summary>
        /// Subscribe for local events.
        /// </summary>
        protected virtual void Subscribe()
        {
            if (closeButton) closeButton.onClick.AddListener(OnCloseButtonPress);
            _subscribed = true;
            _awaitingHide = false;
            OnShowing?.Invoke();
        }

        /// <summary>
        /// Unsubscribe for local events.
        /// </summary>
        protected virtual void Unsubscribe()
        {
            if (closeButton) closeButton.onClick.RemoveListener(OnCloseButtonPress);
            _subscribed = false;
            _awaitingHide = false;
            OnHided?.Invoke();
        }

        /// <summary>
        /// Show the popup.
        /// </summary>
        public override void Show(DisplaySettings settings = default, bool deepShow = false, CancellationToken cancellationToken = default)
        {
            _ = ShowAsync(settings, deepShow, cancellationToken);
        }
        public override async Task ShowAsync(DisplaySettings settings = default, bool deepShow = false, CancellationToken cancellationToken = default)
        {
            if (_subscribed) return;
            
            // Set new source if not have yet
            if (cancellationToken == default)
                cancellationToken = UpdateCancellationTokenSource();
            // Dispose old token
            else if (cancellationToken != Source?.Token)
                UpdateCancellationTokenSource();
                
            Subscribe();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = base.CachedDisplay;
            DisplaySettings cachedSettings = GetSettings(popupDisplay, settings);
            tasks.Add(popupDisplay.ShowMethod(RootTransform, cachedSettings, cancellationToken));

            if (deepShow)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.ShowAsync(settings, true, cancellationToken));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Show the popup with a specific display type.
        /// </summary>
        public override void Show<T>(DisplaySettings settings = default, bool deepShow = false, CancellationToken cancellationToken = default)
        {
            _ = ShowAsync<T>(settings, deepShow, cancellationToken);
        }
        public override async Task ShowAsync<T>(DisplaySettings settings = default, bool deepShow = false, CancellationToken cancellationToken = default)
        {
            if (_subscribed) return;
            
            // Set new source if not have yet
            if (cancellationToken == default)
                cancellationToken = UpdateCancellationTokenSource();
            // Dispose old token
            else if (cancellationToken != Source?.Token)
                UpdateCancellationTokenSource();
                
            Subscribe();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = AdvancedPopupSystem.GetDisplay<T>();
            DisplaySettings cachedSettings = GetSettings(popupDisplay, settings);
            tasks.Add(popupDisplay.ShowMethod(RootTransform, cachedSettings, cancellationToken));

            if (deepShow)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.ShowAsync<T>(settings, true, cancellationToken));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Show the popup with specific display types for the popup and deep popups.
        /// </summary>
        public override void Show<T,J>(DisplaySettings settings = default, bool deepShow = false, CancellationToken cancellationToken = default)
        {
            _ = ShowAsync<T,J>(settings, deepShow, cancellationToken);
        }
        public override async Task ShowAsync<T, J>(DisplaySettings settings = default, bool deepShow = false, CancellationToken cancellationToken = default)
        {
            if (_subscribed) return;
            
            // Set new source if not have yet
            if (cancellationToken == default)
                cancellationToken = UpdateCancellationTokenSource();
            // Dispose old token
            else if (cancellationToken != Source?.Token)
                UpdateCancellationTokenSource();
                
            Subscribe();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = AdvancedPopupSystem.GetDisplay<T>();
            DisplaySettings cachedSettings = GetSettings(popupDisplay, settings);
            tasks.Add(popupDisplay.ShowMethod(RootTransform, cachedSettings, cancellationToken));

            if (deepShow)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.ShowAsync<J>(settings, true, cancellationToken));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide the popup.
        /// </summary>
        public override void Hide(DisplaySettings settings = default, bool deepHide = false, CancellationToken cancellationToken = default)
        {
            _ = HideAsync(settings, deepHide, cancellationToken);
        }
        public override async Task HideAsync(DisplaySettings settings = default, bool deepHide = false, CancellationToken cancellationToken = default)
        {
            if (_awaitingHide || !_subscribed) return;
            _awaitingHide = true;
            
            // Set new source if not have yet
            if (cancellationToken == default)
                cancellationToken = UpdateCancellationTokenSource();
            // Dispose old token
            else if (cancellationToken != Source?.Token)
                UpdateCancellationTokenSource();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = base.CachedDisplay;
            DisplaySettings cachedSettings = GetSettings(popupDisplay, settings);
            tasks.Add(popupDisplay.HideMethod(RootTransform, cachedSettings, cancellationToken));

            if (deepHide)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.HideAsync(settings, true, cancellationToken));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);

            if (_subscribed)
                Unsubscribe();
        }

        /// <summary>
        /// Hide the popup with a specific display type.
        /// </summary>
        public override void Hide<T>(DisplaySettings settings = default, bool deepHide = false, CancellationToken cancellationToken = default)
        {
            _ = HideAsync<T>(settings, deepHide, cancellationToken);
        }
        public override async Task HideAsync<T>(DisplaySettings settings = default, bool deepHide = false, CancellationToken cancellationToken = default)
        {
            if (_awaitingHide || !_subscribed) return;
            _awaitingHide = true;
            
            // Set new source if not have yet
            if (cancellationToken == default)
                cancellationToken = UpdateCancellationTokenSource();
            // Dispose old token
            else if (cancellationToken != Source?.Token)
                UpdateCancellationTokenSource();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = AdvancedPopupSystem.GetDisplay<T>();
            DisplaySettings cachedSettings = GetSettings(popupDisplay, settings);
            tasks.Add(popupDisplay.HideMethod(RootTransform, cachedSettings, cancellationToken));

            if (deepHide)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.HideAsync<T>(settings, true, cancellationToken));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);

            if (_subscribed)
                Unsubscribe();
        }

        /// <summary>
        /// Hide the popup with specific display types for the popup and deep popups.
        /// </summary>
        public override void Hide<T,J>(DisplaySettings settings = default, bool deepHide = false, CancellationToken cancellationToken = default)
        {
            _ = HideAsync<T,J>(settings, deepHide, cancellationToken);
        }
        public override async Task HideAsync<T, J>(DisplaySettings settings = default, bool deepHide = false, CancellationToken cancellationToken = default)
        {
            if (_awaitingHide || !_subscribed) return;
            _awaitingHide = true;
            
            // Set new source if not have yet
            if (cancellationToken == default)
                cancellationToken = UpdateCancellationTokenSource();
            // Dispose old token
            else if (cancellationToken != Source?.Token)
                UpdateCancellationTokenSource();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = AdvancedPopupSystem.GetDisplay<T>();
            DisplaySettings cachedSettings = GetSettings(popupDisplay, settings);
            tasks.Add(popupDisplay.HideMethod(RootTransform, cachedSettings, cancellationToken));

            if (deepHide)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.HideAsync<J>(settings, true, cancellationToken));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);

            if (_subscribed)
                Unsubscribe();
        }
    }
}