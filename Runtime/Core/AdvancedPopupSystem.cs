using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdvancedPS.Core.System;
using AdvancedPS.Core.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdvancedPS.Core
{
    /// <summary>
    /// Core manager for "Advanced Popup System"
    /// </summary>
    public static partial class AdvancedPopupSystem
    {
        #region VARIABLES
        public static readonly List<IAdvancedPopup> AllPopups = new List<IAdvancedPopup>();
        private static readonly List<IDisplay> AllDisplays = new List<IDisplay>();

        private static CancellationTokenSource _source;
        #endregion

        #region Other
        /// <summary>
        /// Push popup entity into AdvancedPopupSystem for caching, without it AdvancedPopupSystem will not use popup in our logic.
        /// </summary>
        public static void InitAdvancedPopup(IAdvancedPopup popup)
        {
            if (!AllPopups.Contains(popup))
            {
                AllPopups.Add(popup);
                SortPopups();
            }
        }
        /// <summary>
        /// Remove popup entity from AdvancedPopupSystem cache, now GC can clean object at all.
        /// </summary>
        public static void DeactivateAdvancedPopup(IAdvancedPopup popup)
        {
            if (AllPopups.Contains(popup))
                AllPopups.Remove(popup);
        }
        
        private static void SortPopups()
        {
            Scene activeScene = SceneManager.GetActiveScene();

            // Sort popups in active scene
            List<IAdvancedPopup> activeScenePopups = AllPopups
                .Where(popup => popup.gameObject.scene == activeScene)
                .OrderByDescending(popup => GetHierarchyDepth(popup.transform))
                .ToList();

            // Sort popups in background scenes
            List<IAdvancedPopup> otherScenesPopups = AllPopups
                .Where(popup => popup.gameObject.scene != activeScene)
                .OrderByDescending(popup => GetHierarchyDepth(popup.transform))
                .ToList();

            AllPopups.Clear();
            AllPopups.AddRange(activeScenePopups);
            AllPopups.AddRange(otherScenesPopups);
        }

        private static int GetHierarchyDepth(Transform transform)
        {
            int depth = 0;
            while (transform.parent != null)
            {
                depth++;
                transform = transform.parent;
            }
            return depth;
        }
        
        /// <summary>
        /// Get display entity with caching in AdvancedPopupSystem for better performance, because we don't need duplicate entity.
        /// </summary>
        public static IDisplay GetDisplay<T>() where T : IDisplay, new()
        {
            IDisplay display = AllDisplays.FirstOrDefault(popupDisplay => popupDisplay is T);
            if (display == default)
            {
                display = new T();
                AllDisplays.Add(display);
            }

            return display;
        }
        #endregion

        #region Manual Popup Show

        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        public static Operation PopupShow(PopupLayerEnum layer)
        {
            return new Operation(async token =>
            {
                try
                {
                    if (token.IsCancellationRequested || !Application.isPlaying)
                        return;

                    await ShowPopupsAsync(token, GetPopupsByLayer(layer), null);
                }
                catch (Exception ex)
                {
                    APLogger.LogError($"Exception occurred: {ex.Message}");
                }
            }, UpdateCancellationTokenSource());
        }

        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        private static Operation PopupShowGeneric<T>(PopupLayerEnum layer, BaseSettings settings = null) where T : IDisplay, new()
        {
            return new Operation(async token =>
            {
                try
                {
                    if (token.IsCancellationRequested || !Application.isPlaying)
                        return;

                    await ShowPopupsAsync<T>(token, GetPopupsByLayer(layer), settings);
                }
                catch (Exception ex)
                {
                    APLogger.LogError($"Exception occurred: {ex.Message}");
                }
            }, UpdateCancellationTokenSource());
        }
        #endregion
        
        #region Manual Popup Hide
        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        public static Operation PopupHide(PopupLayerEnum layer)
        {
            return new Operation(async token =>
            {
                try
                {
                    if (token.IsCancellationRequested || !Application.isPlaying)
                        return;

                    await HidePopupsAsync(token, GetPopupsByLayer(layer), null);
                }
                catch (Exception ex)
                {
                    APLogger.LogError($"Exception occurred: {ex.Message}");
                }
            }, UpdateCancellationTokenSource());
        }

        /// <summary>
        /// Hide/Remove popup with the specified layer without hiding the rest layers by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        private static Operation PopupHideGeneric<T>(PopupLayerEnum layer, BaseSettings settings = null) where T : IDisplay, new()
        {
            return new Operation(async token =>
            {
                try
                {
                    if (token.IsCancellationRequested || !Application.isPlaying)
                        return;

                    await HidePopupsAsync<T>(token, GetPopupsByLayer(layer), settings);
                }
                catch (Exception ex)
                {
                    APLogger.LogError($"Exception occurred: {ex.Message}");
                }
            }, UpdateCancellationTokenSource());
        }
        #endregion
        
        #region Layer systems

        /// <summary>
        /// Show all popups with the specified layer.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        public static Operation LayerShow(PopupLayerEnum layer)
        {
            return new Operation(async token =>
            {
                try
                {
                    await HidePopupsAsync(token, GetPopupsExcludingLayer(layer), null);

                    if (token.IsCancellationRequested || !Application.isPlaying)
                        return;

                    await ShowPopupsAsync(token, GetPopupsByLayer(layer), null);
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
        private static Operation LayerShowGeneric<T>(PopupLayerEnum layer, BaseSettings settings = null) where T : IDisplay, new()
        {
            return new Operation(async token =>
            {
                try
                {
                    await HidePopupsAsync<T>(token, GetPopupsExcludingLayer(layer), settings);
                    
                    if (token.IsCancellationRequested || !Application.isPlaying)
                        return;

                    await ShowPopupsAsync<T>(token, GetPopupsByLayer(layer), settings);
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
        /// <param name="showSettings"> The settings for the open popup animation. If not provided, the default settings will be used. </param>
        /// <param name="hideSettings"> The settings for the open hide animation. If not provided, the default settings will be used. </param>
        private static Operation LayerShowGeneric<T, J>(PopupLayerEnum layer, BaseSettings showSettings = null, BaseSettings hideSettings = null)
            where T : IDisplay, new() where J : IDisplay, new()
        {
            return new Operation(async token =>
            {
                try
                {
                    await HidePopupsAsync<J>(token, GetPopupsExcludingLayer(layer), hideSettings);
                    
                    if (token.IsCancellationRequested || !Application.isPlaying)
                        return;

                    await ShowPopupsAsync<T>(token, GetPopupsByLayer(layer), showSettings);
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
                    await HideAllPopupsAsync(token, null);
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
        private static Operation HideAllGeneric<T>(BaseSettings settings = null) where T : IDisplay, new()
        {
            return new Operation(async token =>
            {
                try
                {
                    await HideAllPopupsAsync<T>(token, settings);
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
        private static IEnumerable<IAdvancedPopup> GetPopupsByLayer(PopupLayerEnum layer)
        {
            List<IAdvancedPopup> popups = AllPopups.Where(popup => popup.PopupLayer.HasFlag(layer)).ToList();
            if (popups.Count == 0)
                APLogger.LogError($"AdvancedPopupSystem not found popup/s by '{layer}' layer!");

            return popups;
        }
        
        /// <summary>
        /// Get popups excluding a specific layer in any loaded scene.
        /// </summary>
        private static IEnumerable<IAdvancedPopup> GetPopupsExcludingLayer(PopupLayerEnum layer)
        {
            List<IAdvancedPopup> popups = AllPopups.Where(popup => !popup.PopupLayer.HasFlag(layer)).ToList();
            if (popups.Count == 0)
                APLogger.LogError($"AdvancedPopupSystem not found popup/s excluding '{layer}' layer!");

            return popups;
        }

        /// <summary>
        /// Show popups with specified display type.
        /// </summary>
        private static async Task ShowPopupsAsync(CancellationToken token, IEnumerable<IAdvancedPopup> popups, BaseSettings settings)
        {
            List<Task> tasks = popups.Select(popup => popup.ShowAsync(token, settings)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Show popups with specified display type.
        /// </summary>
        private static async Task ShowPopupsAsync<T>(CancellationToken token, IEnumerable<IAdvancedPopup> popups, BaseSettings settings) 
            where T : IDisplay, new()
        {
            List<Task> tasks = popups.Select(popup => popup.ShowAsync<T>(token, settings)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide popups with specified display type.
        /// </summary>
        private static async Task HidePopupsAsync(CancellationToken token, IEnumerable<IAdvancedPopup> popups, BaseSettings settings)
        {
            List<Task> tasks = popups.Select(popup => popup.HideAsync(token, settings)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide popups with specified display type.
        /// </summary>
        private static async Task HidePopupsAsync<T>(CancellationToken token, IEnumerable<IAdvancedPopup> popups, BaseSettings settings) 
            where T : IDisplay, new()
        {
            List<Task> tasks = popups.Select(popup => popup.HideAsync<T>(token, settings)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide all popups.
        /// </summary>
        private static async Task HideAllPopupsAsync(CancellationToken token, BaseSettings settings)
        {
            List<Task> tasks = AllPopups.Select(popup => popup.HideAsync(token, settings)).ToList();
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Hide all popups.
        /// </summary>
        private static async Task HideAllPopupsAsync<T>(CancellationToken token, BaseSettings settings) 
            where T : IDisplay, new()
        {
            List<Task> tasks = AllPopups.Select(popup => popup.HideAsync<T>(token, settings)).ToList();
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