using AdvancedPS.Core.System;

namespace AdvancedPS.Core
{
    public class FadeSettings : DefaultSettings
    {
        /// <summary>
        /// Target alpha of CanvasGroup on shown.
        /// </summary>
        public float MaxValue { get; set; }
        /// <summary>
        /// Target alpha of CanvasGroup on hidden.
        /// </summary>
        public float MinValue { get; set; }

        public FadeSettings()
        {
            Duration = 0.5f;
            MaxValue = 1f;
            MinValue = 0f;
        }
    }
}