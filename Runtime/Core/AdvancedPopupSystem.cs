using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdvancedPS.Core.System;
using AdvancedPS.Core.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AdvancedPS.Core
{
    /// <summary>
    /// Core manager for "Advanced Popup System"
    /// </summary>
    public static class AdvancedPopupSystem
    {
        #region VARIABLES
        private static readonly List<IAdvancedPopup> Popups = new List<IAdvancedPopup>();
        private static readonly List<IAdvancedPopupDisplay> Displays = new List<IAdvancedPopupDisplay>();

        private static CancellationTokenSource _source;
        #endregion

        #region Other
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
                Displays.Add(display);
            }

            return display;
        }
        #endregion

        #region Manual Popup Show

        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        public static Operation PopupShow(PopupLayerEnum layer, bool deepShow = false)
        {
            return new Operation(token =>
            {
                List<IAdvancedPopup> popupsToShow = GetPopupsByLayer(layer);
                if (popupsToShow.Count <= 0)
                {
                    APLogger.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                    return Task.CompletedTask;
                }

                popupsToShow.ForEach(popup => popup.ShowAsync(token, null, deepShow));
                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        public static Operation PopupShow<T>(PopupLayerEnum layer, IDefaultSettings settings = null, bool deepShow = false) where T : IAdvancedPopupDisplay, new()
        {
            return new Operation(token =>
            {
                List<IAdvancedPopup> popupsToShow = GetPopupsByLayer(layer);
                if (popupsToShow.Count <= 0)
                {
                    APLogger.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                    return Task.CompletedTask;
                }

                popupsToShow.ForEach(popup => popup.ShowAsync<T>(token, settings, deepShow));
                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        public static Operation PopupShow<T, J>(PopupLayerEnum layer, IDefaultSettings settings = null, bool deepShow = false)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            return new Operation(token =>
            {
                List<IAdvancedPopup> popupsToShow = GetPopupsByLayer(layer);
                if (popupsToShow.Count <= 0)
                {
                    APLogger.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                    return Task.CompletedTask;
                }

                popupsToShow.ForEach(popup => popup.ShowAsync<T, J>(token, settings, deepShow));
                return Task.CompletedTask;
            });
        }
        #endregion
        
        #region Manual Popup Hide
        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="deepHide">If true, shows all "DeepPopups" with the specified layer.</param>
        public static Operation PopupHide(PopupLayerEnum layer, bool deepHide = false)
        {
            return new Operation(token =>
            {
                List<IAdvancedPopup> popupsToHide = GetPopupsByLayer(layer);
                if (popupsToHide.Count <= 0)
                {
                    APLogger.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                    return Task.CompletedTask;
                }

                popupsToHide.ForEach(popup => popup.HideAsync(token, null, deepHide));
                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepHide">If true, shows all "DeepPopups" with the specified layer.</param>
        public static Operation PopupHide<T>(PopupLayerEnum layer, IDefaultSettings settings = null, bool deepHide = false) where T : IAdvancedPopupDisplay, new()
        {
            return new Operation(token =>
            {
                List<IAdvancedPopup> popupsToHide = GetPopupsByLayer(layer);
                if (popupsToHide.Count <= 0)
                {
                    APLogger.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                    return Task.CompletedTask;
                }

                popupsToHide.ForEach(popup => popup.HideAsync<T>(token, settings, deepHide));
                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepHide">If true, shows all "DeepPopups" with the specified layer.</param>
        public static Operation PopupHide<T, J>(PopupLayerEnum layer, IDefaultSettings settings = null, bool deepHide = false)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            return new Operation(token =>
            {
                List<IAdvancedPopup> popupsToHide = GetPopupsByLayer(layer);
                if (popupsToHide.Count <= 0)
                {
                    APLogger.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                    return Task.CompletedTask;
                }

                popupsToHide.ForEach(popup => popup.HideAsync<T, J>(token, settings, deepHide));
                return Task.CompletedTask;
            });
        }
        #endregion
        
        #region Layer systems

        /// <summary>
        /// Show all popups with the specified layer.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        /// <param name="deepHide">If true, hides all "DeepPopups" without the specified layer.</param>
        public static Operation LayerShow(PopupLayerEnum layer, bool deepShow = false, bool deepHide = true)
        {
            return new Operation(async token =>
            {
                try
                {
                    List<IAdvancedPopup> popupsToShow = GetPopupsByLayer(layer);
                    if (popupsToShow.Count <= 0)
                    {
                        APLogger.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                        return;
                    }
                    
                    await HidePopupsAsync(token, layer, null, deepHide);

                    if (token.IsCancellationRequested || !Application.isPlaying)
                        return;

                    await ShowPopupsAsync(token, popupsToShow, null, deepShow);
                }
                catch (Exception ex)
                {
                    APLogger.LogError($"Exception occurred: {ex.Message}");
                }
            }, UpdateCancellationTokenSource());
        }

        /// <summary>
        /// Show all popups with the specified layer by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        /// <param name="deepHide">If true, hides all "DeepPopups" without the specified layer.</param>
        public static Operation LayerShow<T>(PopupLayerEnum layer, IDefaultSettings settings = null,  bool deepShow = false, bool deepHide = true) where T : IAdvancedPopupDisplay, new()
        {
            return new Operation(async token =>
            {
                try
                {
                    List<IAdvancedPopup> popupsToShow = GetPopupsByLayer(layer);
                    if (popupsToShow.Count <= 0)
                    {
                        APLogger.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                        return;
                    }

                    await HidePopupsAsync<T>(token, layer, settings, deepHide);
                    
                    if (token.IsCancellationRequested || !Application.isPlaying)
                        return;

                    await ShowPopupsAsync<T>(token, popupsToShow, settings, deepShow);
                }
                catch (Exception ex)
                {
                    APLogger.LogError($"Exception occurred: {ex.Message}");
                }
            }, UpdateCancellationTokenSource());
        }

        /// <summary>
        /// Show all popups with the specified layer by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        /// <param name="deepHide">If true, hides all "DeepPopups" without the specified layer.</param>
        public static Operation LayerShow<T, J>(PopupLayerEnum layer, IDefaultSettings settings = null, bool deepShow = false, bool deepHide = true)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            return new Operation(async token =>
            {
                try
                {
                    List<IAdvancedPopup> popupsToShow = GetPopupsByLayer(layer);
                    if (popupsToShow.Count <= 0)
                    {
                        APLogger.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                        return;
                    }

                    await HidePopupsAsync<T, J>(token, layer, settings, deepHide);
                    
                    if (token.IsCancellationRequested || !Application.isPlaying)
                        return;

                    await ShowPopupsAsync<T, J>(token, popupsToShow, settings, deepShow);
                }
                catch (Exception ex)
                {
                    APLogger.LogError($"Exception occurred: {ex.Message}");
                }
            }, UpdateCancellationTokenSource());
        }
        #endregion

        #region HIDE ALL

        /// <summary>
        /// Hide all popups by CachedDisplay type.
        /// </summary>
        /// <param name="deepHide">If true, hides all "DeepPopups" without layer.</param>
        public static Operation HideAll(bool deepHide = false)
        {
            return new Operation(async token =>
            {
                try
                {
                    await HideAllPopupsAsync(token, null, deepHide);
                }
                catch (Exception ex)
                {
                    APLogger.LogError($"Exception occurred: {ex.Message}");
                }
            }, UpdateCancellationTokenSource());
        }

        /// <summary>
        /// Hide all popups by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepHide">If true, hides all "DeepPopups".</param>
        public static Operation HideAll<T>(IDefaultSettings settings = null, bool deepHide = false) where T : IAdvancedPopupDisplay, new()
        {
            return new Operation(async token =>
            {
                try
                {
                    await HideAllPopupsAsync<T>(token, settings, deepHide);
                }
                catch (Exception ex)
                {
                    APLogger.LogError($"Exception occurred: {ex.Message}");
                }
            }, UpdateCancellationTokenSource());
        }

        /// <summary>
        /// Hide all popups by IAdvancedPopupDisplay generic T type for this popup and J type for "DeepPopups".
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepHide">If true, hides all "DeepPopups".</param>
        public static Operation HideAll<T, J>(IDefaultSettings settings = null, bool deepHide = true) where T : IAdvancedPopupDisplay, new()
            where J : IAdvancedPopupDisplay, new()
        {
            return new Operation(async token =>
            {
                try
                {
                    await HideAllPopupsAsync<T, J>(token, settings, deepHide);
                }
                catch (Exception ex)
                {
                    APLogger.LogError($"Exception occurred: {ex.Message}");
                }
            }, UpdateCancellationTokenSource());
        }
        #endregion

        #region Private
        /// <summary>
        /// Get popups by layer in any loaded scene.
        /// </summary>
        private static List<IAdvancedPopup> GetPopupsByLayer(PopupLayerEnum layer)
        {
            IAdvancedPopup[] activePopups = Object.FindObjectsOfType<IAdvancedPopup>();
            return activePopups.Where(popup => popup.PopupLayer.HasFlag(layer)).ToList();
        }

        /// <summary>
        /// Show popups with specified display type.
        /// </summary>
        private static async Task ShowPopupsAsync(CancellationToken token, IEnumerable<IAdvancedPopup> popups, IDefaultSettings settings, bool deepShow)
        {
            List<Task> tasks = popups.Select(popup => popup.ShowAsync(token, settings, deepShow)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Show popups with specified display type.
        /// </summary>
        private static async Task ShowPopupsAsync<T>(CancellationToken token, IEnumerable<IAdvancedPopup> popups, IDefaultSettings settings, bool deepShow) 
            where T : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = popups.Select(popup => popup.ShowAsync<T>(token, settings, deepShow)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Show popups with specified display type.
        /// </summary>
        private static async Task ShowPopupsAsync<T, J>(CancellationToken token, IEnumerable<IAdvancedPopup> popups, IDefaultSettings settings, bool deepShow)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = popups.Select(popup => popup.ShowAsync<T, J>(token, settings, deepShow)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide popups with specified display type.
        /// </summary>
        private static async Task HidePopupsAsync(CancellationToken token, PopupLayerEnum layer, IDefaultSettings settings, bool deepHide)
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in Popups.Where(popup => !popup.PopupLayer.HasFlag(layer)))
            {
                tasks.Add(popup.HideAsync(token, settings, deepHide));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide popups with specified display type.
        /// </summary>
        private static async Task HidePopupsAsync<T>(CancellationToken token, PopupLayerEnum layer, IDefaultSettings settings, bool deepHide) 
            where T : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in Popups.Where(popup => !popup.PopupLayer.HasFlag(layer)))
            {
                tasks.Add(popup.HideAsync<T>(token, settings, deepHide));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide popups with specified display type.
        /// </summary>
        private static async Task HidePopupsAsync<T, J>(CancellationToken token, PopupLayerEnum layer, IDefaultSettings settings, bool deepHide)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in Popups.Where(popup => !popup.PopupLayer.HasFlag(layer)))
            {
                tasks.Add(popup.HideAsync<T, J>(token, settings, deepHide));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide all popups.
        /// </summary>
        private static async Task HideAllPopupsAsync(CancellationToken token, IDefaultSettings settings, bool deepHide)
        {
            List<Task> tasks = Popups.Select(popup => popup.HideAsync(token, settings, deepHide)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide all popups.
        /// </summary>
        private static async Task HideAllPopupsAsync<T>(CancellationToken token, IDefaultSettings settings, bool deepHide) 
            where T : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = Popups.Select(popup => popup.HideAsync<T>(token, settings, deepHide)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide all popups.
        /// </summary>
        private static async Task HideAllPopupsAsync<T, J>(CancellationToken token, IDefaultSettings settings, bool deepHide)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = Popups.Select(popup => popup.HideAsync<T, J>(token, settings, deepHide)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }
        
        private static CancellationTokenSource UpdateCancellationTokenSource()
        {
            if (_source != null)
            {
                _source.Cancel();
                _source.Dispose();
            }
            _source = new CancellationTokenSource();
            return _source;
        }
        #endregion
    }
}