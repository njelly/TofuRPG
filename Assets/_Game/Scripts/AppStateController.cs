using UnityEngine;
using UnityEngine.Events;

namespace Tofunaut.TofuUnity
{
    public abstract class AppStateController<T> : SingletonBehaviour<T> where T : MonoBehaviour
    {
        public UnityEvent OnComplete;

        public virtual void Complete()
        {
            OnComplete?.Invoke();
        }
    }
}