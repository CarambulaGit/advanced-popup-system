using System;

namespace AdvancedPS.Core
{
    public struct DisplaySettings
    {
        public float Duration { get; set; }
        public Action OnComplete { get; set; }
    }
}
