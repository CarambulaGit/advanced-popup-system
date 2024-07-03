using System;

namespace AdvancedPS.Core
{
    public abstract class IDefaultSettings
    {
        /// <summary>
        /// Duration animation in seconds.
        /// </summary>
        public float Duration { get; set; }
        /// <summary>
        /// Event should Invoke when animation will end completely.
        /// </summary>
        public Action OnAnimationEnd { get; set; }
        /// <summary>
        /// Setting default values.
        /// </summary>
        protected IDefaultSettings()
        {
            Duration = 0.5f;
        }
    }
}
