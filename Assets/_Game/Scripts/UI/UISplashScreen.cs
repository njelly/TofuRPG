using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tofunaut.TofuRPG
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UISplashScreen : MonoBehaviour
    {
        public float fadeInLength = 1;
        public float lingerLength = 2;
        public float fadeOutLength = 1;
        public bool canSkip = true;
        public UnityEvent OnSkip;

        private CanvasGroup _canvasGroup;
        private TofuAnimator.Sequence _sequence;

        private void OnEnable()
        {
            if(!_canvasGroup)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            TofuAnimator.Sequence _sequence = gameObject.Sequence()
                .Curve(TofuAnimator.EEaseType.Linear, fadeInLength, (float newValue) =>
                {
                    _canvasGroup.alpha = Mathf.Lerp(0, 1, newValue);
                })
                .Then()
                .Wait(lingerLength)
                .Then()
                .Curve(TofuAnimator.EEaseType.Linear, fadeOutLength, (float newValue) =>
                {
                    _canvasGroup.alpha = Mathf.Lerp(1, 0, newValue);
                })
                .Then()
                .Execute(() =>
                {
                    Skip();
                });

            _sequence.Play();
        }

        public void Skip()
        {
            OnSkip?.Invoke();
        }
    }
}