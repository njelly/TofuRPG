using System;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class InitializationController : MonoBehaviour, IInitializationController, InitializationView.IListener
    {
        public event EventHandler OnComplete;

        public bool IsComplete { get; private set; }

        public InitializationView viewPrefab;

        private InitializationView _instantiatedView;

        private void OnEnable()
        {
            _instantiatedView = Instantiate(viewPrefab, transform);
            _instantiatedView.listener = this;
        }

        private void OnDisable()
        {
            Destroy(_instantiatedView.gameObject);
        }

        private void Complete()
        {
            IsComplete = true;
            OnComplete?.Invoke(this, EventArgs.Empty);
        }

        public void OnCompleteSplashScreens()
        {
            // TODO: login? load default assets?
            Complete();
        }
    }
}