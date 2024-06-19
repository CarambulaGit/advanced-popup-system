using System.Threading;
using System.Threading.Tasks;

namespace AdvancedPS.Core.Popup
{
    public class AdvancedPopupInstantiate : IAdvancedPopup
    {
        public override Task Show(bool deepShow = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override Task Show<T>(bool deepShow = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override Task Show<T, J>(bool deepShow = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override Task Hide(bool deepHide = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override Task Hide<T>(bool deepHide = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override Task Hide<T, J>(bool deepHide = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
