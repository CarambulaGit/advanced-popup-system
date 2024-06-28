using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AdvancedPS.Core
{
    public class ImmediatelyScaleDisplay : IAdvancedPopupDisplay
    {
        protected Vector3 ShowScale = Vector3.one;
        protected Vector3 HideScale = Vector3.zero;
        
        public override Task ShowMethod(Transform transform, DisplaySettings settings, CancellationToken cancellationToken = default)
        {
            CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        
            transform.localScale = ShowScale;

            settings.OnComplete?.Invoke();
            return Task.CompletedTask;
        }

        public override Task HideMethod(Transform transform, DisplaySettings settings, CancellationToken cancellationToken = default)
        {
            transform.localScale = HideScale;
        
            CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            
            settings.OnComplete?.Invoke();
            return Task.CompletedTask;
        }
    }
}
