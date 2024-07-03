using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AdvancedPS.Core.System
{
    public abstract class IAdvancedPopup : MonoBehaviour
    {
        /// <summary>
        /// Layer of popup, check all layers where you expect this popup can be shown.
        /// </summary>
        [Tooltip("Layer of popup, check all layers where you expect this popup can be shown.")]
        public PopupLayerEnum PopupLayer;
        /// <summary>
        /// true - if need manual initialize popup via Init() func for better resources control.
        /// </summary>
        [Tooltip("true - if need manual initialize popup via Init() func for better resources control.")]
        public bool ManualInit;
        
        /// <summary>
        /// Root transform.
        /// </summary>
        [Header("REF's")][Space(5)]
        [Tooltip("Root transform.")]
        public RectTransform RootTransform;
        /// <summary>
        /// Child or dependent popups of the current one, use if you need more control via Show/Hide.
        /// </summary>
        [Tooltip("Child or dependent popups of the current one, use if you need more control via Show/Hide.")]
        public List<IAdvancedPopup> DeepPopups;
        /// <summary>
        /// Cached method for the animation. To change it by call 'AdvancedPopupSystem.GetDisplay'.
        /// </summary>
        protected IAdvancedPopupDisplay CachedDisplay { get; private set; }
        /// <summary>
        /// Cached settings for the animation. To change it by call 'AdvancedPopupSystem.GetDisplay'.
        /// </summary>
        protected IDefaultSettings CachedSettings { get; private set; }
        
        protected CancellationTokenSource Source;

        private void Awake()
        {
            // Change CachedDisplay value if need it, like below - for better performance
            SetCachedDisplay<ScaleDisplay>(new ScaleSettings());
            
            if (!ManualInit)
                Init();
        }
        
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type,
        /// initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and cache.</typeparam>
        /// <param name="settings">The settings for the animation. If not provided, the default settings will be used.</param>
        protected void SetCachedDisplay<T>(IDefaultSettings settings) where T : IAdvancedPopupDisplay, new()
        {
            CachedDisplay = AdvancedPopupSystem.GetDisplay<T>();
            CachedSettings = settings;
        }

        /// <summary>
        /// Method invoking manual or from Awake if "ManualInit" - false. Please keep base.Init() first of all when override.
        /// </summary>
        public virtual void Init()
        {
            AdvancedPopupSystem.InitAdvancedPopup(this);

            if (RootTransform == null)
                RootTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// Check if popup exist in deep of this popup.
        /// </summary>
        /// <param name="popup"> popup what we are searching </param>
        public virtual bool ContainsDeepPopup(IAdvancedPopup popup)
        {
            return DeepPopups.Any(deepPopup => popup == deepPopup || deepPopup.ContainsDeepPopup(popup));
        }

        #region SHOW
        /// <summary>
        /// Show popup by CachedDisplay type without await.
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepShow"> true - show all "DeepPopups" of this popup </param>
        public abstract Operation Show(IDefaultSettings settings = null, bool deepShow = false);
        /// <summary>
        /// Show popup by CachedDisplay type.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepShow"> true - show all "DeepPopups" of this popup </param>
        public abstract Task ShowAsync(CancellationToken token, IDefaultSettings settings = null, bool deepShow = false);

        /// <summary>
        /// Show popup by IAdvancedPopupDisplay generic T type for all popup's without await.
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepShow"> true - show all "DeepPopups" of this popup </param>
        public abstract Operation Show<T>(IDefaultSettings settings = null, bool deepShow = false)
            where T : IAdvancedPopupDisplay, new();
        /// <summary>
        /// Show popup by IAdvancedPopupDisplay generic T type for all popup's.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepShow"> true - show all "DeepPopups" of this popup </param>
        public abstract Task ShowAsync<T>(CancellationToken token, IDefaultSettings settings = null, bool deepShow = false)
            where T : IAdvancedPopupDisplay, new();
        
        /// <summary>
        /// Show popup by IAdvancedPopupDisplay generic T type for this popup and J type for "DeepPopups" without await.
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepShow"> true - show all "DeepPopups" of this popup </param>
        public abstract Operation Show<T, J>(IDefaultSettings settings = null, bool deepShow = false)
            where T : IAdvancedPopupDisplay, new()
            where J : IAdvancedPopupDisplay, new();
        /// <summary>
        /// Show popup by IAdvancedPopupDisplay generic T type for this popup and J type for "DeepPopups".
        /// </summary>
        /// <param name="token"></param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepShow"> true - show all "DeepPopups" of this popup </param>
        public abstract Task ShowAsync<T, J>(CancellationToken token, IDefaultSettings settings = null, bool deepShow = false)
            where T : IAdvancedPopupDisplay, new()
            where J : IAdvancedPopupDisplay, new();
        #endregion


        #region HIDE
        /// <summary>
        /// Hide popup by CachedDisplay type without await.
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepHide"> true - hide all "DeepPopups" of this popup </param>
        public abstract Operation Hide(IDefaultSettings settings = null, bool deepHide = false);
        /// <summary>
        /// Hide popup by CachedDisplay type.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepHide"> true - hide all "DeepPopups" of this popup </param>
        public abstract Task HideAsync(CancellationToken token, IDefaultSettings settings = null, bool deepHide = false);
        
        /// <summary>
        /// Hide popup by IAdvancedPopupDisplay generic T type for all popup's without await.
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepHide"> true - hide all "DeepPopups" of this popup </param>
        public abstract Operation Hide<T>(IDefaultSettings settings = null, bool deepHide = false)
            where T : IAdvancedPopupDisplay, new();

        /// <summary>
        /// Hide popup by IAdvancedPopupDisplay generic T type for all popup's.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepHide"> true - hide all "DeepPopups" of this popup </param>
        public abstract Task HideAsync<T>(CancellationToken token, IDefaultSettings settings = null, bool deepHide = false)
            where T : IAdvancedPopupDisplay, new();
        
        /// <summary>
        /// Hide popup by IAdvancedPopupDisplay generic T type for this popup and J type for "DeepPopups" without await.
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepHide"> true - hide all "DeepPopups" of this popup </param>
        public abstract Operation Hide<T, J>(IDefaultSettings settings = null, bool deepHide = false)
            where T : IAdvancedPopupDisplay, new()
            where J : IAdvancedPopupDisplay, new();
        /// <summary>
        /// Hide popup by IAdvancedPopupDisplay generic T type for this popup and J type for "DeepPopups".
        /// </summary>
        /// <param name="token"></param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        /// <param name="deepHide"> true - hide all "DeepPopups" of this popup </param>
        public abstract Task HideAsync<T, J>(CancellationToken token, IDefaultSettings settings = null, bool deepHide = false)
            where T : IAdvancedPopupDisplay, new()
            where J : IAdvancedPopupDisplay, new();
        #endregion
        
        protected CancellationTokenSource UpdateCancellationTokenSource()
        {
            if (Source != null)
            {
                Source.Cancel();
                Source.Dispose();
            }
            Source = new CancellationTokenSource();
            return Source;
        }
    }
}
