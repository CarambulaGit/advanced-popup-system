using System;
using AdvancedPS.Core.System;
using UnityEngine;

namespace AdvancedPS.Core
{
    [Serializable]
    public class SlideSettings : BaseSettings
    {
        /// <summary>
        /// From witch side popup will start move.
        /// </summary>
        public SlideEnum SlideEnum { get; set; }
        /// <summary>
        /// The position to which the popup will aim. Default value Vector.zero - it will try aim to center of Canvas.
        /// </summary>
        public Vector3 TargetPosition { get; set; }

        /// <summary>
        /// Setting default values.
        /// </summary>
        public SlideSettings()
        {
            Duration = 0.75f;
            SlideEnum = SlideEnum.Down;
            TargetPosition = Vector3.zero;
        }
    }
}