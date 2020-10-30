﻿using UnityEngine;
using UnityEngine.Events;

namespace Tofunaut.TofuUnity
{
    public abstract class AppStateController : MonoBehaviour
    {
        public LogService log;
        public UnityEvent OnComplete;

        public virtual void Complete()
        {
            OnComplete?.Invoke();
        }
    }
}