using System.Threading;
using System.Threading.Tasks;
using AdvancedPS.Core.System;

namespace AdvancedPS.Core
{
    public class AdvancedPopupInstantiate : IAdvancedPopup
    {
        public override Operation Show(DefaultSettings settings = null)
        {
            return null;
        }

        public override Task ShowAsync(CancellationToken token = default, DefaultSettings settings = null)
        {
            return Task.CompletedTask;
        }

        public override Operation Show<T>(DefaultSettings settings = null)
        {
            return null;
        }

        public override Task ShowAsync<T>(CancellationToken token = default, DefaultSettings settings = null)
        {
            return Task.CompletedTask;
        }

        public override Operation Hide(DefaultSettings settings = null)
        {
            return null;
        }

        public override Task HideAsync(CancellationToken token = default, DefaultSettings settings = null)
        {
            return Task.CompletedTask;
        }

        public override Operation Hide<T>(DefaultSettings settings = null)
        {
            return null;
        }

        public override Task HideAsync<T>(CancellationToken token = default, DefaultSettings settings = null)
        {
            return Task.CompletedTask;
        }
    }
}
