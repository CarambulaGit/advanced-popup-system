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
        public Button closeButton;
        
        [HideInInspector] public CanvasGroup canvasGroup;
        
        private bool _subscribed;

        /// <summary>
        /// Initialize the popup.
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
            Hide(true);
        }

        /// <summary>
        /// Subscribe to the close button press event.
        /// </summary>
        public virtual void Subscribe()
        {
            if (closeButton) closeButton.onClick.AddListener(OnCloseButtonPress);
            _subscribed = true;
        }

        /// <summary>
        /// Unsubscribe from the close button press event.
        /// </summary>
        public virtual void Unsubscribe()
        {
            if (closeButton) closeButton.onClick.RemoveListener(OnCloseButtonPress);
            _subscribed = false;
        }

        /// <summary>
        /// Show the popup.
        /// </summary>
        public override void Show(bool deepShow = false, CancellationToken cancellationToken = default)
        {
            _ = ShowAsync(deepShow, cancellationToken);
        }
        public override async Task ShowAsync(bool deepShow = false, CancellationToken cancellationToken = default)
        {
            if (cancellationToken == default)
                cancellationToken = UpdateCancellationTokenSource();

            if (!_subscribed)
                Subscribe();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = base.CachedDisplay;
            tasks.Add(popupDisplay.ShowMethod(RootTransform, cancellationToken));

            if (deepShow)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.ShowAsync(true, cancellationToken));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Show the popup with a specific display type.
        /// </summary>
        public override void Show<T>(bool deepShow = false, CancellationToken cancellationToken = default)
        {
            _ = ShowAsync<T>(deepShow, cancellationToken);
        }
        public override async Task ShowAsync<T>(bool deepShow = false, CancellationToken cancellationToken = default)
        {
            if (cancellationToken == default)
                cancellationToken = UpdateCancellationTokenSource();

            if (!_subscribed)
                Subscribe();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = AdvancedPopupSystem.GetDisplay<T>();
            tasks.Add(popupDisplay.ShowMethod(RootTransform, cancellationToken));

            if (deepShow)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.ShowAsync<T>(true, cancellationToken));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Show the popup with specific display types for the popup and deep popups.
        /// </summary>
        public override void Show<T,J>(bool deepShow = false, CancellationToken cancellationToken = default)
        {
            _ = ShowAsync<T,J>(deepShow, cancellationToken);
        }
        public override async Task ShowAsync<T, J>(bool deepShow = false, CancellationToken cancellationToken = default)
        {
            if (cancellationToken == default)
                cancellationToken = UpdateCancellationTokenSource();

            if (!_subscribed)
                Subscribe();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = AdvancedPopupSystem.GetDisplay<T>();
            tasks.Add(popupDisplay.ShowMethod(RootTransform, cancellationToken));

            if (deepShow)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.ShowAsync<J>(true, cancellationToken));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide the popup.
        /// </summary>
        public override void Hide(bool deepHide = false, CancellationToken cancellationToken = default)
        {
            _ = HideAsync(deepHide, cancellationToken);
        }
        public override async Task HideAsync(bool deepHide = false, CancellationToken cancellationToken = default)
        {
            if (cancellationToken == default)
                cancellationToken = UpdateCancellationTokenSource();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = base.CachedDisplay;
            tasks.Add(popupDisplay.HideMethod(RootTransform, cancellationToken));

            if (deepHide)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.HideAsync(true, cancellationToken));
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
        public override void Hide<T>(bool deepHide = false, CancellationToken cancellationToken = default)
        {
            _ = HideAsync<T>(deepHide, cancellationToken);
        }
        public override async Task HideAsync<T>(bool deepHide = false, CancellationToken cancellationToken = default)
        {
            if (cancellationToken == default)
                cancellationToken = UpdateCancellationTokenSource();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = AdvancedPopupSystem.GetDisplay<T>();
            tasks.Add(popupDisplay.HideMethod(RootTransform, cancellationToken));

            if (deepHide)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.HideAsync<T>(true, cancellationToken));
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
        public override void Hide<T,J>(bool deepHide = false, CancellationToken cancellationToken = default)
        {
            _ = HideAsync<T,J>(deepHide, cancellationToken);
        }
        public override async Task HideAsync<T, J>(bool deepHide = false, CancellationToken cancellationToken = default)
        {
            if (cancellationToken == default)
                cancellationToken = UpdateCancellationTokenSource();

            List<Task> tasks = new List<Task>();

            IAdvancedPopupDisplay popupDisplay = AdvancedPopupSystem.GetDisplay<T>();
            tasks.Add(popupDisplay.HideMethod(RootTransform, cancellationToken));

            if (deepHide)
            {
                foreach (IAdvancedPopup popup in DeepPopups)
                {
                    tasks.Add(popup.HideAsync<J>(true, cancellationToken));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);

            if (_subscribed)
                Unsubscribe();
        }
    }
}