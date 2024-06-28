using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AdvancedPS.Core
{
    public abstract class IAdvancedPopupDisplay
    {
        public DisplaySettings DisplaySettings = new DisplaySettings
        {
            Duration = 0.5f
        };
        public abstract Task ShowMethod(Transform transform, DisplaySettings settings, CancellationToken cancellationToken = default);
        public abstract Task HideMethod(Transform transform, DisplaySettings settings, CancellationToken cancellationToken = default);
    }
}
