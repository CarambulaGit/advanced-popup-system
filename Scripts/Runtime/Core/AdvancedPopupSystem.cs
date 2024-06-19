using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdvancedPS.Core.Enum;
using UnityEngine;

namespace AdvancedPS.Core
{
    public static class AdvancedPopupSystem
    {
        private static readonly List<IAdvancedPopup> Popups = new List<IAdvancedPopup>();
        private static readonly List<IAdvancedPopupDisplay> Displays = new List<IAdvancedPopupDisplay>();
        
        private static CancellationTokenSource _source;

        /// <summary>
        /// Push popup entity into AdvancedPopupSystem for caching, without it AdvancedPopupSystem will not use popup in our logic.
        /// </summary>
        public static void InitAdvancedPopup<T>(T popup) where T : IAdvancedPopup
        {
            if (!Popups.Contains(popup))
                Popups.Add(popup);
        }
        
        /// <summary>
        /// Get display entity with caching in AdvancedPopupSystem for better performance, because we don't need duplicate entity.
        /// </summary>
        public static IAdvancedPopupDisplay GetDisplay<T>() where T : IAdvancedPopupDisplay, new()
        {
            IAdvancedPopupDisplay display = Displays.FirstOrDefault(popupDisplay => popupDisplay is T);
            if (display == default)
            {
                display = new T();
                display.InitMethod();
                
                Displays.Add(display);
            }

            return display;
        }

        #region Layer systems

        /// <summary>
        /// Show all popups with the specified layer.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        /// <param name="deepHide">If true, hides all "DeepPopups" without the specified layer.</param>
        public static void LayerShow(PopupLayerEnum layer, bool deepShow = false, bool deepHide = true)
        {
            _ = LayerShowAsync(layer, deepShow, deepHide);
        }

        private static async Task LayerShowAsync(PopupLayerEnum layer, bool deepShow, bool deepHide)
        {
            try
            {
                List<IAdvancedPopup> popupsToShow = GetPopupsByLayer(layer);
                if (popupsToShow.Count <= 0)
                {
                    Debug.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                    return;
                }

                CancellationToken cToken = UpdateCancellationTokenSource();

                await HidePopupsAsync(layer, deepHide, cToken);
                if (cToken.IsCancellationRequested || !Application.isPlaying)
                    return;
                
                await ShowPopupsAsync(popupsToShow, deepShow, cToken);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Show all popups with the specified layer by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        /// <param name="deepHide">If true, hides all "DeepPopups" without the specified layer.</param>
        public static void LayerShow<T>(PopupLayerEnum layer, bool deepShow = false, bool deepHide = true) where T : IAdvancedPopupDisplay, new()
        {
            _ = LayerShowAsync<T>(layer, deepShow, deepHide);
        }

        private static async Task LayerShowAsync<T>(PopupLayerEnum layer, bool deepShow, bool deepHide) where T : IAdvancedPopupDisplay, new()
        {
            try
            {
                List<IAdvancedPopup> popupsToShow = GetPopupsByLayer(layer);
                if (popupsToShow.Count <= 0)
                {
                    Debug.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                    return;
                }

                CancellationToken cToken = UpdateCancellationTokenSource();

                await HidePopupsAsync<T>(layer, deepHide, cToken);
                if (cToken.IsCancellationRequested || !Application.isPlaying)
                    return;
                
                await ShowPopupsAsync<T>(popupsToShow, deepShow, cToken);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Show all popups with the specified layer by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        /// <param name="deepHide">If true, hides all "DeepPopups" without the specified layer.</param>
        public static void LayerShow<T, J>(PopupLayerEnum layer, bool deepShow = false, bool deepHide = true)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            _ = LayerShowAsync<T, J>(layer, deepShow, deepHide);
        }

        private static async Task LayerShowAsync<T, J>(PopupLayerEnum layer, bool deepShow, bool deepHide)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            try
            {
                List<IAdvancedPopup> popupsToShow = GetPopupsByLayer(layer);
                if (popupsToShow.Count <= 0)
                {
                    Debug.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                    return;
                }

                CancellationToken cToken = UpdateCancellationTokenSource();

                await HidePopupsAsync<T, J>(layer, deepHide, cToken);
                if (cToken.IsCancellationRequested || !Application.isPlaying)
                    return;
                
                await ShowPopupsAsync<T, J>(popupsToShow, deepShow, cToken);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception occurred: {ex.Message}");
            }
        }
        #endregion

        #region HIDE ALL

        /// <summary>
        /// Hide all popups by CachedDisplay type.
        /// </summary>
        /// <param name="deepHide">If true, hides all "DeepPopups" without layer.</param>
        public static void HideAll(bool deepHide = false)
        {
            _ = HideAllAsync(deepHide);
        }

        private static async Task HideAllAsync(bool deepHide)
        {
            try
            {
                CancellationToken cToken = UpdateCancellationTokenSource();
                await HideAllPopupsAsync(deepHide, cToken);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Hide all popups by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="deepHide">If true, hides all "DeepPopups".</param>
        public static void HideAll<T>(bool deepHide = false) where T : IAdvancedPopupDisplay, new()
        {
            _ = HideAllAsync<T>(deepHide);
        }

        private static async Task HideAllAsync<T>(bool deepHide) where T : IAdvancedPopupDisplay, new()
        {
            try
            {
                CancellationToken cToken = UpdateCancellationTokenSource();
                await HideAllPopupsAsync<T>(deepHide, cToken);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Hide all popups by IAdvancedPopupDisplay generic T type for this popup and J type for "DeepPopups".
        /// </summary>
        /// <param name="deepHide">If true, hides all "DeepPopups".</param>
        public static void HideAll<T, J>(bool deepHide = true) where T : IAdvancedPopupDisplay, new()
            where J : IAdvancedPopupDisplay, new()
        {
            _ = HideAllAsync<T, J>(deepHide);
        }

        private static async Task HideAllAsync<T, J>(bool deepHide) where T : IAdvancedPopupDisplay, new()
            where J : IAdvancedPopupDisplay, new()
        {
            try
            {
                CancellationToken cToken = UpdateCancellationTokenSource();
                await HideAllPopupsAsync<T, J>(deepHide, cToken);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception occurred: {ex.Message}");
            }
        }
        #endregion

        /// <summary>
        /// Get popups by layer.
        /// </summary>
        private static List<IAdvancedPopup> GetPopupsByLayer(PopupLayerEnum layer)
        {
            return Popups.FindAll(popup => popup.PopupLayer.HasFlag(layer));
        }

        /// <summary>
        /// Show popups with specified display type.
        /// </summary>
        private static async Task ShowPopupsAsync(List<IAdvancedPopup> popups, bool deepShow, CancellationToken cToken)
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in popups)
            {
                popup.StopAnim();
                tasks.Add(popup.ShowAsync(deepShow, cancellationToken: cToken));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Show popups with specified display type.
        /// </summary>
        private static async Task ShowPopupsAsync<T>(List<IAdvancedPopup> popups, bool deepShow, CancellationToken cToken) where T : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in popups)
            {
                popup.StopAnim();
                tasks.Add(popup.ShowAsync<T>(deepShow, cancellationToken: cToken));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Show popups with specified display type.
        /// </summary>
        private static async Task ShowPopupsAsync<T, J>(List<IAdvancedPopup> popups, bool deepShow, CancellationToken cToken)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in popups)
            {
                popup.StopAnim();
                tasks.Add(popup.ShowAsync<T, J>(deepShow, cancellationToken: cToken));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide popups with specified display type.
        /// </summary>
        private static async Task HidePopupsAsync(PopupLayerEnum layer, bool deepHide, CancellationToken cToken)
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in Popups.Where(popup => !popup.PopupLayer.HasFlag(layer)))
            {
                popup.StopAnim();
                tasks.Add(popup.HideAsync(deepHide, cancellationToken: cToken));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide popups with specified display type.
        /// </summary>
        private static async Task HidePopupsAsync<T>(PopupLayerEnum layer, bool deepHide, CancellationToken cToken) where T : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in Popups.Where(popup => !popup.PopupLayer.HasFlag(layer)))
            {
                popup.StopAnim();
                tasks.Add(popup.HideAsync<T>(deepHide, cancellationToken: cToken));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide popups with specified display type.
        /// </summary>
        private static async Task HidePopupsAsync<T, J>(PopupLayerEnum layer, bool deepHide, CancellationToken cToken)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in Popups.Where(popup => !popup.PopupLayer.HasFlag(layer)))
            {
                popup.StopAnim();
                tasks.Add(popup.HideAsync<T, J>(deepHide, cancellationToken: cToken));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide all popups.
        /// </summary>
        private static async Task HideAllPopupsAsync(bool deepHide, CancellationToken cToken)
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in Popups)
            {
                popup.StopAnim();
                tasks.Add(popup.HideAsync(deepHide, cancellationToken: cToken));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide all popups.
        /// </summary>
        private static async Task HideAllPopupsAsync<T>(bool deepHide, CancellationToken cToken) where T : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in Popups)
            {
                popup.StopAnim();
                tasks.Add(popup.HideAsync<T>(deepHide, cancellationToken: cToken));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide all popups.
        /// </summary>
        private static async Task HideAllPopupsAsync<T, J>(bool deepHide, CancellationToken cToken)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in Popups)
            {
                popup.StopAnim();
                tasks.Add(popup.HideAsync<T, J>(deepHide, cancellationToken: cToken));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Stop previous tasks and start new.
        /// </summary>
        private static CancellationToken UpdateCancellationTokenSource()
        {
            if (_source != null)
            {
                _source.Cancel();
                _source.Dispose();
            }
            _source = new CancellationTokenSource();
            return _source.Token;
        }
    }
}