using System;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class StartScreenRootController : MonoBehaviour, IStartScreenRootController, StartScreenRootView.IListener
    {
        public event EventHandler PlayGameRequested;
        public event EventHandler QuitGameRequested;

        public StartScreenRootView viewPrefab;

        private StartScreenRootView _instantiatedViewPrefab;

        private void OnEnable()
        {
            _instantiatedViewPrefab = Instantiate(viewPrefab, transform);
            _instantiatedViewPrefab.listener = this;
        }

        private void OnDisable()
        {
            Destroy(_instantiatedViewPrefab);
        }

        public void StartScreenRootView_OnPlayClicked()
        {
            PlayGameRequested?.Invoke(this, EventArgs.Empty);
        }

        public void StartScreenRootView_OnQuitClicked()
        {
            QuitGameRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}