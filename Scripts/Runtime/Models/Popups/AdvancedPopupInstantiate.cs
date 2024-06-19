using System.Threading;
using System.Threading.Tasks;

namespace AdvancedPS.Core.Popup
{
    public class AdvancedPopupInstantiate : IAdvancedPopup
    {
        public override void Show(bool deepShow = false, CancellationToken cancellationToken = default)
        {
        }

        public override Task ShowAsync(bool deepShow = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override void Show<T>(bool deepShow = false, CancellationToken cancellationToken = default)
        {
        }

        public override Task ShowAsync<T>(bool deepShow = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override void Show<T, J>(bool deepShow = false, CancellationToken cancellationToken = default)
        {
        }

        public override Task ShowAsync<T, J>(bool deepShow = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override void Hide(bool deepHide = false, CancellationToken cancellationToken = default)
        {
        }

        public override Task HideAsync(bool deepHide = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override void Hide<T>(bool deepHide = false, CancellationToken cancellationToken = default)
        {
        }

        public override Task HideAsync<T>(bool deepHide = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override void Hide<T, J>(bool deepHide = false, CancellationToken cancellationToken = default)
        {
        }

        public override Task HideAsync<T, J>(bool deepHide = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
