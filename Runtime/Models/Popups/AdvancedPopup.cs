using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AdvancedPS.Core.System;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedPS.Core
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
        public override Operation Show(IDefaultSettings settings = null, bool deepShow = false)
        {
            return new Operation(async token =>
            {
                await ShowAsync(token, settings, deepShow);
            }, UpdateCancellationTokenSource());
        }
        public override async Task ShowAsync(CancellationToken token, IDefaultSettings settings = null, bool deepShow = false)
        {
            if (_subscribed) return;
                
            Subscribe();
            
            List<Task> tasks = new List<Task>
            {
                CachedDisplay.ShowMethod(RootTransform, settings ??= CachedSettings, token)
            };

            if (deepShow)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.ShowAsync(token, settings, true));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Show the popup with a specific display type.
        /// </summary>
        public override Operation Show<T>(IDefaultSettings settings = null, bool deepShow = false)
        {
            return new Operation(async token =>
            {
                await ShowAsync<T>(token, settings, deepShow);
            }, UpdateCancellationTokenSource());
        }
        public override async Task ShowAsync<T>(CancellationToken token, IDefaultSettings settings = null, bool deepShow = false)
        {
            if (_subscribed) return;
                
            Subscribe();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = AdvancedPopupSystem.GetDisplay<T>();
            tasks.Add(popupDisplay.ShowMethod(RootTransform, settings ??= CachedSettings, token));

            if (deepShow)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.ShowAsync<T>(token, settings, true));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Show the popup with specific display types for the popup and deep popups.
        /// </summary>
        public override Operation Show<T,J>(IDefaultSettings settings = null, bool deepShow = false)
        {
            return new Operation(async token =>
            {
                await ShowAsync<T,J>(token, settings, deepShow);
            }, UpdateCancellationTokenSource());
        }
        public override async Task ShowAsync<T, J>(CancellationToken token, IDefaultSettings settings = null, bool deepShow = false)
        {
            if (_subscribed) return;
                
            Subscribe();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = AdvancedPopupSystem.GetDisplay<T>();
            tasks.Add(popupDisplay.ShowMethod(RootTransform, settings ??= CachedSettings, token));

            if (deepShow)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.ShowAsync<J>(token, settings, true));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide the popup.
        /// </summary>
        public override Operation Hide(IDefaultSettings settings = null, bool deepHide = false)
        {
            return new Operation(async token =>
            {
                await HideAsync(token, settings, deepHide);
            }, UpdateCancellationTokenSource());
        }
        public override async Task HideAsync(CancellationToken token, IDefaultSettings settings = null, bool deepHide = false)
        {
            if (_awaitingHide || !_subscribed) return;
            _awaitingHide = true;

            List<Task> tasks = new List<Task>
            {
                CachedDisplay.HideMethod(RootTransform, settings ??= CachedSettings, token)
            };

            if (deepHide)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.HideAsync(token, settings, true));
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
        public override Operation Hide<T>(IDefaultSettings settings = null, bool deepHide = false)
        {
            return new Operation(async token =>
            {
                await HideAsync<T>(token, settings, deepHide);
            }, UpdateCancellationTokenSource());
        }
        public override async Task HideAsync<T>(CancellationToken token, IDefaultSettings settings = null, bool deepHide = false)
        {
            if (_awaitingHide || !_subscribed) return;
            _awaitingHide = true;

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = AdvancedPopupSystem.GetDisplay<T>();
            tasks.Add(popupDisplay.HideMethod(RootTransform, settings ??= CachedSettings, token));

            if (deepHide)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.HideAsync<T>(token, settings, true));
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
        public override Operation Hide<T,J>(IDefaultSettings settings = null, bool deepHide = false)
        {
            return new Operation(async token =>
            {
                await HideAsync<T,J>(token, settings, deepHide);
            }, UpdateCancellationTokenSource());
        }
        public override async Task HideAsync<T, J>(CancellationToken token, IDefaultSettings settings = null, bool deepHide = false)
        {
            if (_awaitingHide || !_subscribed) return;
            _awaitingHide = true;

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = AdvancedPopupSystem.GetDisplay<T>();
            tasks.Add(popupDisplay.HideMethod(RootTransform, settings ??= CachedSettings, token));

            if (deepHide)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.HideAsync<J>(token, settings, true));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);

            if (_subscribed)
                Unsubscribe();
        }
    }
}