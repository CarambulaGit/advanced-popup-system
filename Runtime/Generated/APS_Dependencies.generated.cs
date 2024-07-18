using System.Collections.Generic;

namespace AdvancedPS.Core.System
{
    public static class APS_Dependencies
    {
        public static readonly Dictionary<IDisplay, IDefaultSettings> DisplaySettingsDependency =
            new Dictionary<IDisplay, IDefaultSettings>()
            {
                { new FadeDisplay(), new FadeSettings() },
                { new ScaleDisplay(), new ScaleSettings() },
                { new SlideDisplay(), new SlideSettings() },
            };
    }
}