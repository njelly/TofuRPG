using System;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class InitializationState : MonoBehaviour, IInitializationState
    {
        public event EventHandler OnComplete;

        public bool IsComplete { get; private set; }

        public InitializationView viewPrefab;

        private void Start()
        {
            // TODO: login? load default assets?
            Complete();
        }

        private void Complete()
        {
            IsComplete = true;
            OnComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}