using AdvancedPS.Core.System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using AdvancedPS.Core.Utils;
using System;
namespace AdvancedPS.Core
{
    public static partial class AdvancedPopupSystem
    {
        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public static Operation PopupShow<T>(PopupLayerEnum layer, FadeSettings settings = null) where T : FadeDisplay, new()
        {
            return PopupShowGeneric<T>(layer, settings);
        }
        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public static Operation PopupShow<T>(PopupLayerEnum layer, ScaleSettings settings = null) where T : ScaleDisplay, new()
        {
            return PopupShowGeneric<T>(layer, settings);
        }
        /// <summary>
        /// Show/Add popup with the specified layer without hiding the rest layers by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public static Operation PopupShow<T>(PopupLayerEnum layer, SlideSettings settings = null) where T : SlideDisplay, new()
        {
            return PopupShowGeneric<T>(layer, settings);
        }
        /// <summary>
        /// Hide/Remove popup with the specified layer without hiding the rest layers by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public static Operation PopupHide<T>(PopupLayerEnum layer, FadeSettings settings = null) where T : FadeDisplay, new()
        {
            return PopupHideGeneric<T>(layer, settings);
        }
        /// <summary>
        /// Hide/Remove popup with the specified layer without hiding the rest layers by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public static Operation PopupHide<T>(PopupLayerEnum layer, ScaleSettings settings = null) where T : ScaleDisplay, new()
        {
            return PopupHideGeneric<T>(layer, settings);
        }
        /// <summary>
        /// Hide/Remove popup with the specified layer without hiding the rest layers by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popup for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public static Operation PopupHide<T>(PopupLayerEnum layer, SlideSettings settings = null) where T : SlideDisplay, new()
        {
            return PopupHideGeneric<T>(layer, settings);
        }
        /// <summary>
        /// Show all popups with the specified layer by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public static Operation LayerShow<T>(PopupLayerEnum layer, FadeSettings settings = null) where T : FadeDisplay, new()
        {
            return LayerShowGeneric<T>(layer, settings);
        }
        /// <summary>
        /// Show all popups with the specified layer by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="showSettings"> The settings for the open popup animation. If not provided, the default settings will be used. </param>
        /// <param name="hideSettings"> The settings for the open hide animation. If not provided, the default settings will be used. </param>
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, FadeSettings showSettings = null, FadeSettings hideSettings = null) where T : FadeDisplay, new() where J : FadeDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        /// <summary>
        /// Show all popups with the specified layer by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="showSettings"> The settings for the open popup animation. If not provided, the default settings will be used. </param>
        /// <param name="hideSettings"> The settings for the open hide animation. If not provided, the default settings will be used. </param>
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, FadeSettings showSettings = null, ScaleSettings hideSettings = null) where T : FadeDisplay, new() where J : ScaleDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        /// <summary>
        /// Show all popups with the specified layer by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="showSettings"> The settings for the open popup animation. If not provided, the default settings will be used. </param>
        /// <param name="hideSettings"> The settings for the open hide animation. If not provided, the default settings will be used. </param>
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, FadeSettings showSettings = null, SlideSettings hideSettings = null) where T : FadeDisplay, new() where J : SlideDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        /// <summary>
        /// Show all popups with the specified layer by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public static Operation LayerShow<T>(PopupLayerEnum layer, ScaleSettings settings = null) where T : ScaleDisplay, new()
        {
            return LayerShowGeneric<T>(layer, settings);
        }
        /// <summary>
        /// Show all popups with the specified layer by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="showSettings"> The settings for the open popup animation. If not provided, the default settings will be used. </param>
        /// <param name="hideSettings"> The settings for the open hide animation. If not provided, the default settings will be used. </param>
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, ScaleSettings showSettings = null, FadeSettings hideSettings = null) where T : ScaleDisplay, new() where J : FadeDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        /// <summary>
        /// Show all popups with the specified layer by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="showSettings"> The settings for the open popup animation. If not provided, the default settings will be used. </param>
        /// <param name="hideSettings"> The settings for the open hide animation. If not provided, the default settings will be used. </param>
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, ScaleSettings showSettings = null, ScaleSettings hideSettings = null) where T : ScaleDisplay, new() where J : ScaleDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        /// <summary>
        /// Show all popups with the specified layer by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="showSettings"> The settings for the open popup animation. If not provided, the default settings will be used. </param>
        /// <param name="hideSettings"> The settings for the open hide animation. If not provided, the default settings will be used. </param>
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, ScaleSettings showSettings = null, SlideSettings hideSettings = null) where T : ScaleDisplay, new() where J : SlideDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        /// <summary>
        /// Show all popups with the specified layer by CachedDisplay type.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public static Operation LayerShow<T>(PopupLayerEnum layer, SlideSettings settings = null) where T : SlideDisplay, new()
        {
            return LayerShowGeneric<T>(layer, settings);
        }
        /// <summary>
        /// Show all popups with the specified layer by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="showSettings"> The settings for the open popup animation. If not provided, the default settings will be used. </param>
        /// <param name="hideSettings"> The settings for the open hide animation. If not provided, the default settings will be used. </param>
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, SlideSettings showSettings = null, FadeSettings hideSettings = null) where T : SlideDisplay, new() where J : FadeDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        /// <summary>
        /// Show all popups with the specified layer by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="showSettings"> The settings for the open popup animation. If not provided, the default settings will be used. </param>
        /// <param name="hideSettings"> The settings for the open hide animation. If not provided, the default settings will be used. </param>
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, SlideSettings showSettings = null, ScaleSettings hideSettings = null) where T : SlideDisplay, new() where J : ScaleDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        /// <summary>
        /// Show all popups with the specified layer by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="layer">The layer to show popups for.</param>
        /// <param name="showSettings"> The settings for the open popup animation. If not provided, the default settings will be used. </param>
        /// <param name="hideSettings"> The settings for the open hide animation. If not provided, the default settings will be used. </param>
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, SlideSettings showSettings = null, SlideSettings hideSettings = null) where T : SlideDisplay, new() where J : SlideDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        /// <summary>
        /// Hide all popups by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public static Operation HideAll<T>(FadeSettings settings = null) where T : FadeDisplay, new()
        {
            return HideAllGeneric<T>(settings);
        }
        /// <summary>
        /// Hide all popups by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public static Operation HideAll<T>(ScaleSettings settings = null) where T : ScaleDisplay, new()
        {
            return HideAllGeneric<T>(settings);
        }
        /// <summary>
        /// Hide all popups by IAdvancedPopupDisplay generic T type for all popups.
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public static Operation HideAll<T>(SlideSettings settings = null) where T : SlideDisplay, new()
        {
            return HideAllGeneric<T>(settings);
        }
    }
}
