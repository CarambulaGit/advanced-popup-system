namespace AdvancedPS.Core.System
{
    public abstract partial class IAdvancedPopup
    {
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type, initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T>(FadeSettings showSettings = null) where T : FadeDisplay, new()
        {
            if (showSettings == null)
                showSettings = new FadeSettings();
            SetCachedDisplayInternal<T>(showSettings);
        }
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type, initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <typeparam name="J">The type of advanced popup display to create and hide cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        /// <param name="hideSettings">The settings for the hiding animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T,J>(FadeSettings showSettings = null, FadeSettings hideSettings = null) where T : FadeDisplay, new() where J : FadeDisplay, new()
        {
            if (showSettings == null)
                showSettings = new FadeSettings();
            if (hideSettings == null)
                hideSettings = new FadeSettings();
            SetCachedDisplayInternal<T,J>(showSettings, hideSettings);
        }
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type, initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <typeparam name="J">The type of advanced popup display to create and hide cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        /// <param name="hideSettings">The settings for the hiding animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T,J>(FadeSettings showSettings = null, ScaleSettings hideSettings = null) where T : FadeDisplay, new() where J : ScaleDisplay, new()
        {
            if (showSettings == null)
                showSettings = new FadeSettings();
            if (hideSettings == null)
                hideSettings = new ScaleSettings();
            SetCachedDisplayInternal<T,J>(showSettings, hideSettings);
        }
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type, initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <typeparam name="J">The type of advanced popup display to create and hide cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        /// <param name="hideSettings">The settings for the hiding animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T,J>(FadeSettings showSettings = null, SlideSettings hideSettings = null) where T : FadeDisplay, new() where J : SlideDisplay, new()
        {
            if (showSettings == null)
                showSettings = new FadeSettings();
            if (hideSettings == null)
                hideSettings = new SlideSettings();
            SetCachedDisplayInternal<T,J>(showSettings, hideSettings);
        }
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type, initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T>(ScaleSettings showSettings = null) where T : ScaleDisplay, new()
        {
            if (showSettings == null)
                showSettings = new ScaleSettings();
            SetCachedDisplayInternal<T>(showSettings);
        }
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type, initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <typeparam name="J">The type of advanced popup display to create and hide cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        /// <param name="hideSettings">The settings for the hiding animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T,J>(ScaleSettings showSettings = null, FadeSettings hideSettings = null) where T : ScaleDisplay, new() where J : FadeDisplay, new()
        {
            if (showSettings == null)
                showSettings = new ScaleSettings();
            if (hideSettings == null)
                hideSettings = new FadeSettings();
            SetCachedDisplayInternal<T,J>(showSettings, hideSettings);
        }
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type, initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <typeparam name="J">The type of advanced popup display to create and hide cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        /// <param name="hideSettings">The settings for the hiding animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T,J>(ScaleSettings showSettings = null, ScaleSettings hideSettings = null) where T : ScaleDisplay, new() where J : ScaleDisplay, new()
        {
            if (showSettings == null)
                showSettings = new ScaleSettings();
            if (hideSettings == null)
                hideSettings = new ScaleSettings();
            SetCachedDisplayInternal<T,J>(showSettings, hideSettings);
        }
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type, initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <typeparam name="J">The type of advanced popup display to create and hide cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        /// <param name="hideSettings">The settings for the hiding animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T,J>(ScaleSettings showSettings = null, SlideSettings hideSettings = null) where T : ScaleDisplay, new() where J : SlideDisplay, new()
        {
            if (showSettings == null)
                showSettings = new ScaleSettings();
            if (hideSettings == null)
                hideSettings = new SlideSettings();
            SetCachedDisplayInternal<T,J>(showSettings, hideSettings);
        }
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type, initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T>(SlideSettings showSettings = null) where T : SlideDisplay, new()
        {
            if (showSettings == null)
                showSettings = new SlideSettings();
            SetCachedDisplayInternal<T>(showSettings);
        }
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type, initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <typeparam name="J">The type of advanced popup display to create and hide cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        /// <param name="hideSettings">The settings for the hiding animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T,J>(SlideSettings showSettings = null, FadeSettings hideSettings = null) where T : SlideDisplay, new() where J : FadeDisplay, new()
        {
            if (showSettings == null)
                showSettings = new SlideSettings();
            if (hideSettings == null)
                hideSettings = new FadeSettings();
            SetCachedDisplayInternal<T,J>(showSettings, hideSettings);
        }
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type, initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <typeparam name="J">The type of advanced popup display to create and hide cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        /// <param name="hideSettings">The settings for the hiding animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T,J>(SlideSettings showSettings = null, ScaleSettings hideSettings = null) where T : SlideDisplay, new() where J : ScaleDisplay, new()
        {
            if (showSettings == null)
                showSettings = new SlideSettings();
            if (hideSettings == null)
                hideSettings = new ScaleSettings();
            SetCachedDisplayInternal<T,J>(showSettings, hideSettings);
        }
        /// <summary>
        /// Sets the cached display to an instance of the specified advanced popup display type, initialized with the provided settings.
        /// </summary>
        /// <typeparam name="T">The type of advanced popup display to create and show cache.</typeparam>
        /// <typeparam name="J">The type of advanced popup display to create and hide cache.</typeparam>
        /// <param name="showSettings">The settings for the showing animation. If not provided, the default settings will be used.</param>
        /// <param name="hideSettings">The settings for the hiding animation. If not provided, the default settings will be used.</param>
        public void SetCachedDisplay<T,J>(SlideSettings showSettings = null, SlideSettings hideSettings = null) where T : SlideDisplay, new() where J : SlideDisplay, new()
        {
            if (showSettings == null)
                showSettings = new SlideSettings();
            if (hideSettings == null)
                hideSettings = new SlideSettings();
            SetCachedDisplayInternal<T,J>(showSettings, hideSettings);
        }
    }
}
