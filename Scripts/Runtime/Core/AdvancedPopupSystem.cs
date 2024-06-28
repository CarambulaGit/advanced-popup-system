using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdvancedPS.Core.Enum;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AdvancedPS.Core
{
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
        /// <param name="settings"> Settings for animation </param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        public static void PopupShow(PopupLayerEnum layer, DisplaySettings settings = default, bool deepShow = false)
        {
            List<IAdvancedPopup> popupsToShow = GetPopupsByLayer(layer);
            if (popupsToShow.Count <= 0)
            {
                Debug.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                return;
            }

            popupsToShow.ForEach(popup => popup.Show(settings, deepShow));
        }

        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> Settings for animation </param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        public static void PopupShow<T>(PopupLayerEnum layer, DisplaySettings settings = default, bool deepShow = false) where T : IAdvancedPopupDisplay, new()
        {
            List<IAdvancedPopup> popupsToShow = GetPopupsByLayer(layer);
            if (popupsToShow.Count <= 0)
            {
                Debug.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                return;
            }

            popupsToShow.ForEach(popup => popup.Show<T>(settings, deepShow));
        }

        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> Settings for animation </param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        public static void PopupShow<T, J>(PopupLayerEnum layer, DisplaySettings settings = default, bool deepShow = false)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            List<IAdvancedPopup> popupsToShow = GetPopupsByLayer(layer);
            if (popupsToShow.Count <= 0)
            {
                Debug.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                return;
            }

            popupsToShow.ForEach(popup => popup.Show<T,J>(settings, deepShow));
        }
        #endregion
        
        #region Manual Popup Hide
        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> Settings for animation </param>
        /// <param name="deepHide">If true, shows all "DeepPopups" with the specified layer.</param>
        public static void PopupHide(PopupLayerEnum layer, DisplaySettings settings = default, bool deepHide = false)
        {
            List<IAdvancedPopup> popupsToHide = GetPopupsByLayer(layer);
            if (popupsToHide.Count <= 0)
            {
                Debug.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                return;
            }

            popupsToHide.ForEach(popup => popup.Hide(settings, deepHide));
        }

        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> Settings for animation </param>
        /// <param name="deepHide">If true, shows all "DeepPopups" with the specified layer.</param>
        public static void PopupHide<T>(PopupLayerEnum layer, DisplaySettings settings = default, bool deepHide = false) where T : IAdvancedPopupDisplay, new()
        {
            List<IAdvancedPopup> popupsToHide = GetPopupsByLayer(layer);
            if (popupsToHide.Count <= 0)
            {
                Debug.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                return;
            }

            popupsToHide.ForEach(popup => popup.Hide<T>(settings, deepHide));
        }

        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> Settings for animation </param>
        /// <param name="deepHide">If true, shows all "DeepPopups" with the specified layer.</param>
        public static void PopupHide<T, J>(PopupLayerEnum layer, DisplaySettings settings = default, bool deepHide = false)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            List<IAdvancedPopup> popupsToHide = GetPopupsByLayer(layer);
            if (popupsToHide.Count <= 0)
            {
                Debug.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");
                return;
            }

            popupsToHide.ForEach(popup => popup.Hide<T,J>(settings, deepHide));
        }
        #endregion
        
        #region Layer systems

        /// <summary>
        /// Show all popups with the specified layer.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="settings"> Settings for animation </param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        /// <param name="deepHide">If true, hides all "DeepPopups" without the specified layer.</param>
        public static void LayerShow(PopupLayerEnum layer, DisplaySettings settings = default, bool deepShow = false, bool deepHide = true)
        {
            _ = LayerShowAsync(layer, settings, deepShow, deepHide);
        }

        private static async Task LayerShowAsync(PopupLayerEnum layer, DisplaySettings settings, bool deepShow, bool deepHide)
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
                await HidePopupsAsync(layer, settings, deepHide, cToken);
                
                if (cToken.IsCancellationRequested || !Application.isPlaying)
                    return;
                
                await ShowPopupsAsync(popupsToShow, settings, deepShow, cToken);
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
        /// <param name="settings"> Settings for animation </param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        /// <param name="deepHide">If true, hides all "DeepPopups" without the specified layer.</param>
        public static void LayerShow<T>(PopupLayerEnum layer, DisplaySettings settings = default, bool deepShow = false, bool deepHide = true) where T : IAdvancedPopupDisplay, new()
        {
            _ = LayerShowAsync<T>(layer, settings, deepShow, deepHide);
        }

        private static async Task LayerShowAsync<T>(PopupLayerEnum layer, DisplaySettings settings, bool deepShow, bool deepHide) where T : IAdvancedPopupDisplay, new()
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

                await HidePopupsAsync<T>(layer, settings, deepHide, cToken);
                if (cToken.IsCancellationRequested || !Application.isPlaying)
                    return;
                
                await ShowPopupsAsync<T>(popupsToShow, settings, deepShow, cToken);
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
        /// <param name="settings"> Settings for animation </param>
        /// <param name="deepShow">If true, shows all "DeepPopups" with the specified layer.</param>
        /// <param name="deepHide">If true, hides all "DeepPopups" without the specified layer.</param>
        public static void LayerShow<T, J>(PopupLayerEnum layer, DisplaySettings settings = default, bool deepShow = false, bool deepHide = true)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            _ = LayerShowAsync<T, J>(layer, settings, deepShow, deepHide);
        }

        private static async Task LayerShowAsync<T, J>(PopupLayerEnum layer, DisplaySettings settings, bool deepShow, bool deepHide)
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

                await HidePopupsAsync<T, J>(layer, settings, deepHide, cToken);
                if (cToken.IsCancellationRequested || !Application.isPlaying)
                    return;
                
                await ShowPopupsAsync<T, J>(popupsToShow, settings, deepShow, cToken);
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
        /// <param name="settings"> Settings for animation </param>
        /// <param name="deepHide">If true, hides all "DeepPopups" without layer.</param>
        public static void HideAll(DisplaySettings settings = default, bool deepHide = false)
        {
            _ = HideAllAsync(settings, deepHide);
        }

        private static async Task HideAllAsync(DisplaySettings settings, bool deepHide)
        {
            try
            {
                CancellationToken cToken = UpdateCancellationTokenSource();
                await HideAllPopupsAsync(settings, deepHide, cToken);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Hide all popups by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="settings"> Settings for animation </param>
        /// <param name="deepHide">If true, hides all "DeepPopups".</param>
        public static void HideAll<T>(DisplaySettings settings = default, bool deepHide = false) where T : IAdvancedPopupDisplay, new()
        {
            _ = HideAllAsync<T>(settings, deepHide);
        }

        private static async Task HideAllAsync<T>(DisplaySettings settings, bool deepHide) where T : IAdvancedPopupDisplay, new()
        {
            try
            {
                CancellationToken cToken = UpdateCancellationTokenSource();
                await HideAllPopupsAsync<T>(settings, deepHide, cToken);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Hide all popups by IAdvancedPopupDisplay generic T type for this popup and J type for "DeepPopups".
        /// </summary>
        /// <param name="settings"> Settings for animation </param>
        /// <param name="deepHide">If true, hides all "DeepPopups".</param>
        public static void HideAll<T, J>(DisplaySettings settings = default, bool deepHide = true) where T : IAdvancedPopupDisplay, new()
            where J : IAdvancedPopupDisplay, new()
        {
            _ = HideAllAsync<T, J>(settings, deepHide);
        }

        private static async Task HideAllAsync<T, J>(DisplaySettings settings, bool deepHide) where T : IAdvancedPopupDisplay, new()
            where J : IAdvancedPopupDisplay, new()
        {
            try
            {
                CancellationToken cToken = UpdateCancellationTokenSource();
                await HideAllPopupsAsync<T, J>(settings, deepHide, cToken);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception occurred: {ex.Message}");
            }
        }
        #endregion

        #region Private
        /// <summary>
        /// Get popups by layer in any loaded scene.
        /// </summary>
        private static List<IAdvancedPopup> GetPopupsByLayer(PopupLayerEnum layer)
        {
            IAdvancedPopup[] activePopups = Object.FindObjectsOfType<IAdvancedPopup>();
            return activePopups.Where(popup => popup.PopupLayer.HasFlag(layer) && activePopups.Contains(popup)).ToList();
        }

        /// <summary>
        /// Show popups with specified display type.
        /// </summary>
        private static async Task ShowPopupsAsync(IEnumerable<IAdvancedPopup> popups, DisplaySettings settings, bool deepShow, CancellationToken cToken)
        {
            List<Task> tasks = popups.Select(popup => popup.ShowAsync(settings, deepShow, cancellationToken: cToken)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Show popups with specified display type.
        /// </summary>
        private static async Task ShowPopupsAsync<T>(IEnumerable<IAdvancedPopup> popups, DisplaySettings settings, bool deepShow, CancellationToken cToken) where T : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = popups.Select(popup => popup.ShowAsync<T>(settings, deepShow, cancellationToken: cToken)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Show popups with specified display type.
        /// </summary>
        private static async Task ShowPopupsAsync<T, J>(IEnumerable<IAdvancedPopup> popups, DisplaySettings settings, bool deepShow, CancellationToken cToken)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = popups.Select(popup => popup.ShowAsync<T, J>(settings, deepShow, cancellationToken: cToken)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide popups with specified display type.
        /// </summary>
        private static async Task HidePopupsAsync(PopupLayerEnum layer, DisplaySettings settings, bool deepHide, CancellationToken cToken)
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in Popups.Where(popup => !popup.PopupLayer.HasFlag(layer)))
            {
                tasks.Add(popup.HideAsync(settings, deepHide, cancellationToken: cToken));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide popups with specified display type.
        /// </summary>
        private static async Task HidePopupsAsync<T>(PopupLayerEnum layer, DisplaySettings settings, bool deepHide, CancellationToken cToken) where T : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in Popups.Where(popup => !popup.PopupLayer.HasFlag(layer)))
            {
                tasks.Add(popup.HideAsync<T>(settings, deepHide, cancellationToken: cToken));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide popups with specified display type.
        /// </summary>
        private static async Task HidePopupsAsync<T, J>(PopupLayerEnum layer, DisplaySettings settings, bool deepHide, CancellationToken cToken)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = new List<Task>();
            foreach (IAdvancedPopup popup in Popups.Where(popup => !popup.PopupLayer.HasFlag(layer)))
            {
                tasks.Add(popup.HideAsync<T, J>(settings, deepHide, cancellationToken: cToken));
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide all popups.
        /// </summary>
        private static async Task HideAllPopupsAsync(DisplaySettings settings, bool deepHide, CancellationToken cToken)
        {
            List<Task> tasks = Popups.Select(popup => popup.HideAsync(settings, deepHide, cancellationToken: cToken)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide all popups.
        /// </summary>
        private static async Task HideAllPopupsAsync<T>(DisplaySettings settings, bool deepHide, CancellationToken cToken) where T : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = Popups.Select(popup => popup.HideAsync<T>(settings, deepHide, cancellationToken: cToken)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide all popups.
        /// </summary>
        private static async Task HideAllPopupsAsync<T, J>(DisplaySettings settings, bool deepHide, CancellationToken cToken)
            where T : IAdvancedPopupDisplay, new() where J : IAdvancedPopupDisplay, new()
        {
            List<Task> tasks = Popups.Select(popup => popup.HideAsync<T, J>(settings, deepHide, cancellationToken: cToken)).ToList();
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
        #endregion
    }
}