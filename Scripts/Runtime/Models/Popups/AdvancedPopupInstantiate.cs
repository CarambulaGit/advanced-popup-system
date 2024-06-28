using System.Threading;
using System.Threading.Tasks;

namespace AdvancedPS.Core.Popup
{
    public class AdvancedPopupInstantiate : IAdvancedPopup
    {
        public override void Show(DisplaySettings settings = default, bool deepShow = false, CancellationToken cancellationToken = default)
        {
        }

        public override Task ShowAsync(DisplaySettings settings = default, bool deepShow = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override void Show<T>(DisplaySettings settings = default, bool deepShow = false, CancellationToken cancellationToken = default)
        {
        }

        public override Task ShowAsync<T>(DisplaySettings settings = default, bool deepShow = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override void Show<T, J>(DisplaySettings settings = default, bool deepShow = false, CancellationToken cancellationToken = default)
        {
        }

        public override Task ShowAsync<T, J>(DisplaySettings settings = default, bool deepShow = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override void Hide(DisplaySettings settings = default, bool deepHide = false, CancellationToken cancellationToken = default)
        {
        }

        public override Task HideAsync(DisplaySettings settings = default, bool deepHide = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override void Hide<T>(DisplaySettings settings = default, bool deepHide = false, CancellationToken cancellationToken = default)
        {
        }

        public override Task HideAsync<T>(DisplaySettings settings = default, bool deepHide = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override void Hide<T, J>(DisplaySettings settings = default, bool deepHide = false, CancellationToken cancellationToken = default)
        {
        }

        public override Task HideAsync<T, J>(DisplaySettings settings = default, bool deepHide = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
