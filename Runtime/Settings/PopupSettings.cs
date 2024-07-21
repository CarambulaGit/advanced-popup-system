using System;

namespace AdvancedPS.Core.System
{
    [Serializable]
    public class PopupSettings
    {
        public bool CustomIconsEnabled { get; set; }
        public bool KeyEventSystemEnabled { get; set; }
        public string LogType { get; set; }
    }
}