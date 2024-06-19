using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AdvancedPS.Core
{
    public abstract class IAdvancedPopupDisplay
    {
        public abstract void InitMethod();
        public abstract Task ShowMethod(Transform transform, CancellationToken cancellationToken = default);
        public abstract Task HideMethod(Transform transform, CancellationToken cancellationToken = default);
    }
}
