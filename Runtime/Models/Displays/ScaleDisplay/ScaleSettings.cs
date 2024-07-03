using UnityEngine;

namespace AdvancedPS.Core
{
    public class ScaleSettings : IDefaultSettings
    {
        /// <summary>
        /// Target scale of GameObject on shown.
        /// </summary>
        public Vector3 ShowScale { get; set; }
        /// <summary>
        /// Target scale of GameObject on hidden.
        /// </summary>
        public Vector3 HideScale { get; set; }
        
        public ScaleSettings()
        {
            Duration = 0.5f;
            HideScale = Vector3.zero;
            ShowScale = Vector3.one;
        }
    }
}