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
        public static Operation PopupShow<T>(PopupLayerEnum layer, FadeSettings settings = null) where T : FadeDisplay, new()
        {
            return PopupShowGeneric<T>(layer, settings);
        }
        public static Operation PopupShow<T>(PopupLayerEnum layer, ScaleSettings settings = null) where T : ScaleDisplay, new()
        {
            return PopupShowGeneric<T>(layer, settings);
        }
        public static Operation PopupShow<T>(PopupLayerEnum layer, SlideSettings settings = null) where T : SlideDisplay, new()
        {
            return PopupShowGeneric<T>(layer, settings);
        }
        public static Operation PopupHide<T>(PopupLayerEnum layer, FadeSettings settings = null) where T : FadeDisplay, new()
        {
            return PopupHideGeneric<T>(layer, settings);
        }
        public static Operation PopupHide<T>(PopupLayerEnum layer, ScaleSettings settings = null) where T : ScaleDisplay, new()
        {
            return PopupHideGeneric<T>(layer, settings);
        }
        public static Operation PopupHide<T>(PopupLayerEnum layer, SlideSettings settings = null) where T : SlideDisplay, new()
        {
            return PopupHideGeneric<T>(layer, settings);
        }
        public static Operation LayerShow<T>(PopupLayerEnum layer, FadeSettings settings = null) where T : FadeDisplay, new()
        {
            return LayerShowGeneric<T>(layer, settings);
        }
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, FadeSettings showSettings = null, FadeSettings hideSettings = null) where T : FadeDisplay, new() where J : FadeDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, FadeSettings showSettings = null, ScaleSettings hideSettings = null) where T : FadeDisplay, new() where J : ScaleDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, FadeSettings showSettings = null, SlideSettings hideSettings = null) where T : FadeDisplay, new() where J : SlideDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        public static Operation LayerShow<T>(PopupLayerEnum layer, ScaleSettings settings = null) where T : ScaleDisplay, new()
        {
            return LayerShowGeneric<T>(layer, settings);
        }
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, ScaleSettings showSettings = null, FadeSettings hideSettings = null) where T : ScaleDisplay, new() where J : FadeDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, ScaleSettings showSettings = null, ScaleSettings hideSettings = null) where T : ScaleDisplay, new() where J : ScaleDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, ScaleSettings showSettings = null, SlideSettings hideSettings = null) where T : ScaleDisplay, new() where J : SlideDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        public static Operation LayerShow<T>(PopupLayerEnum layer, SlideSettings settings = null) where T : SlideDisplay, new()
        {
            return LayerShowGeneric<T>(layer, settings);
        }
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, SlideSettings showSettings = null, FadeSettings hideSettings = null) where T : SlideDisplay, new() where J : FadeDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, SlideSettings showSettings = null, ScaleSettings hideSettings = null) where T : SlideDisplay, new() where J : ScaleDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        public static Operation LayerShow<T,J>(PopupLayerEnum layer, SlideSettings showSettings = null, SlideSettings hideSettings = null) where T : SlideDisplay, new() where J : SlideDisplay, new()
        {
            return LayerShowGeneric<T,J>(layer, showSettings, hideSettings);
        }
        public static Operation HideAll<T>(FadeSettings settings = null) where T : FadeDisplay, new()
        {
            return HideAllGeneric<T>(settings);
        }
        public static Operation HideAll<T>(ScaleSettings settings = null) where T : ScaleDisplay, new()
        {
            return HideAllGeneric<T>(settings);
        }
        public static Operation HideAll<T>(SlideSettings settings = null) where T : SlideDisplay, new()
        {
            return HideAllGeneric<T>(settings);
        }
    }
}
