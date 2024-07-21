using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AdvancedPS.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace AdvancedPS.Core.System
{
    public abstract class IAdvancedPopup : MonoBehaviour
    {
        #region Public
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
        [HideInInspector] public RectTransform RootTransform;
        /// <summary>
        /// Canvas group of root popup.
        /// </summary>
        [HideInInspector] public CanvasGroup canvasGroup;
        /// <summary>
        /// State changed after animation started.
        /// </summary>
        [HideInInspector] public bool IsBeVisible;
        /// <summary>
        /// State changed after animation ended.
        /// </summary>
        [HideInInspector] public bool IsVisible;
        /// <summary>
        /// Child or dependent popups of the current one, use if you need more control via Show/Hide.
        /// </summary>
        [Tooltip("Child or dependent popups of the current one, use if you need more control via Show/Hide.")] [Space]
        public List<IAdvancedPopup> DeepPopups = new List<IAdvancedPopup>();
        /// <summary>
        /// Keys witch using for showing popup.
        /// </summary>
        public List<KeyCode> HotKeyShow = new List<KeyCode>();
        /// <summary>
        /// Keys witch using for hiding popup.
        /// </summary>
        public List<KeyCode> HotKeyHide = new List<KeyCode>();
        #endregion
        
        #region Protected
        /// <summary>
        /// Cached method for the showing animation. To change it by call 'AdvancedPopupSystem.GetDisplay'.
        /// </summary>
        protected IDisplay CachedShowDisplay { get; private set; }
        /// <summary>
        /// Cached method for the hiding animation. To change it by call 'AdvancedPopupSystem.GetDisplay'.
        /// </summary>
        protected IDisplay CachedHideDisplay { get; private set; }

        /// <summary>
        /// Cached settings for the showing animation. To change it by call 'AdvancedPopupSystem.GetDisplay'.
        /// </summary>
        [SerializeField] protected DefaultSettings CachedShowSettings;

        /// <summary>
        /// Cached settings for the hiding animation. To change it by call 'AdvancedPopupSystem.GetDisplay'.
        /// </summary>
        [SerializeField] protected DefaultSettings CachedHideSettings;
        #endregion
        
        #region Private
        [SerializeField] private string inspectorShowDisplay;
        [SerializeField] private string inspectorHideDisplay;
        private CancellationTokenSource _source;
        #endregion

        private void Awake()
        {
            if (!ManualInit)
                Init();
        }
        private void OnDestroy()
        {
            AdvancedPopupSystem.DeactivateAdvancedPopup(this);
        }

        /// <summary>
        /// Method invoking manual or from Awake if "ManualInit" - false. Please keep base.Init() first of all when override.
        /// To define the show or/and hide animation - invoke SetCachedDisplay method here. 
        /// </summary>
        public virtual void Init()
        {
            if (string.IsNullOrEmpty(inspectorHideDisplay) || string.IsNullOrEmpty(inspectorHideDisplay) 
                                                           || !SetCachedDisplayFromString(inspectorShowDisplay, inspectorHideDisplay))
            {
                SetCachedDisplay<ScaleDisplay, ScaleDisplay>();
            }
            
            if (RootTransform == null)
                RootTransform = GetComponent<RectTransform>();
            
            transform.localScale = Vector3.zero;
            
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            IsBeVisible = false;
            IsVisible = false;
            
            AdvancedPopupSystem.InitAdvancedPopup(this);
        }
        
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type,
        /// initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T>(DefaultSettings showSettings) where T : IDisplay, new()
        {
            CachedShowDisplay = AdvancedPopupSystem.GetDisplay<T>();
            CachedShowSettings = showSettings;
        }

        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type,
        /// initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <typeparam name="J">The type of advanced popup display to create and hide cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        /// <param name="hideSettings">The settings for the hiding animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T,J>(DefaultSettings showSettings = null, DefaultSettings hideSettings = null) where T : IDisplay, new() where J : IDisplay, new()
        {
            CachedShowDisplay = AdvancedPopupSystem.GetDisplay<T>();
            CachedShowSettings = showSettings;
            CachedHideDisplay = AdvancedPopupSystem.GetDisplay<J>();
            CachedHideSettings = hideSettings;
        }
        
        public bool SetCachedDisplayFromString(string showTypeName, string hideTypeName)
        {
            Type showDisplayType = Type.GetType(showTypeName);
            Type hideDisplayType = Type.GetType(hideTypeName);

            if (showDisplayType == null)
            {
                Debug.LogWarning($"Show type {showTypeName} not found.");
                return false;
            }
            if (hideDisplayType == null)
            {
                Debug.LogWarning($"Hide type {hideTypeName} not found.");
                return false;
            }

            var method = typeof(IAdvancedPopup).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.Name == nameof(SetCachedDisplay) 
                            && m.IsGenericMethodDefinition 
                            && m.GetGenericArguments().Length == 2).FirstOrDefault();
            if (method == null)
            {
                Debug.LogWarning($"Method {nameof(SetCachedDisplay)} not found or ambiguous in {typeof(IAdvancedPopup)}");
                return false;
            }
            //MethodInfo genericMethod = method.MakeGenericMethod(showType, hideType);

            //genericMethod.Invoke(this, new object[] { CachedShowSettings, CachedHideSettings });
            return true;
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
        public abstract Operation Show(DefaultSettings settings = null);
        /// <summary>
        /// Show popup by CachedDisplay type.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public abstract Task ShowAsync(CancellationToken token = default, DefaultSettings settings = null);

        /// <summary>
        /// Show popup by IAdvancedPopupDisplay generic T type for all popup's without await.
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public abstract Operation Show<T>(DefaultSettings settings = null)
            where T : IDisplay, new();
        /// <summary>
        /// Show popup by IAdvancedPopupDisplay generic T type for all popup's.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public abstract Task ShowAsync<T>(CancellationToken token = default, DefaultSettings settings = null)
            where T : IDisplay, new();
        #endregion


        #region HIDE
        /// <summary>
        /// Hide popup by CachedDisplay type without await.
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public abstract Operation Hide(DefaultSettings settings = null);
        /// <summary>
        /// Hide popup by CachedDisplay type.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public abstract Task HideAsync(CancellationToken token = default, DefaultSettings settings = null);
        
        /// <summary>
        /// Hide popup by IAdvancedPopupDisplay generic T type for all popup's without await.
        /// </summary>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public abstract Operation Hide<T>(DefaultSettings settings = null)
            where T : IDisplay, new();

        /// <summary>
        /// Hide popup by IAdvancedPopupDisplay generic T type for all popup's.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="settings"> The settings for the animation. If not provided, the default settings will be used. </param>
        public abstract Task HideAsync<T>(CancellationToken token = default, DefaultSettings settings = null)
            where T : IDisplay, new();
        #endregion
        
        protected CancellationTokenSource UpdateCancellationTokenSource()
        {
            if (_source != null)
            {
                _source.Cancel();
                _source.Dispose();
            }
            _source = new CancellationTokenSource();
            return _source;
        }
    }
}
