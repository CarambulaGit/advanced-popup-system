using System.Threading;
using System.Threading.Tasks;
using AdvancedPS.Core.System;

namespace AdvancedPS.Core
{
    public class AdvancedPopupInstantiate : IAdvancedPopup
    {
        public override Operation Show(IDefaultSettings settings = null, bool deepShow = false)
        {
            return null;
        }

        public override Task ShowAsync(CancellationToken token, IDefaultSettings settings = null, bool deepShow = false)
        {
            return Task.CompletedTask;
        }

        public override Operation Show<T>(IDefaultSettings settings = null, bool deepShow = false)
        {
            return null;
        }

        public override Task ShowAsync<T>(CancellationToken token, IDefaultSettings settings = null, bool deepShow = false)
        {
            return Task.CompletedTask;
        }

        public override Operation Show<T, J>(IDefaultSettings settings = null, bool deepShow = false)
        {
            return null;
        }

        public override Task ShowAsync<T, J>(CancellationToken token, IDefaultSettings settings = null, bool deepShow = false)
        {
            return Task.CompletedTask;
        }

        public override Operation Hide(IDefaultSettings settings = null, bool deepHide = false)
        {
            return null;
        }

        public override Task HideAsync(CancellationToken token, IDefaultSettings settings = null, bool deepHide = false)
        {
            return Task.CompletedTask;
        }

        public override Operation Hide<T>(IDefaultSettings settings = null, bool deepHide = false)
        {
            return null;
        }

        public override Task HideAsync<T>(CancellationToken token, IDefaultSettings settings = null, bool deepHide = false)
        {
            return Task.CompletedTask;
        }

        public override Operation Hide<T, J>(IDefaultSettings settings = null, bool deepHide = false)
        {
            return null;
        }

        public override Task HideAsync<T, J>(CancellationToken token, IDefaultSettings settings = null, bool deepHide = false)
        {
            return Task.CompletedTask;
        }
    }
}
