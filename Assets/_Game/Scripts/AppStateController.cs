using UnityEngine;
using UnityEngine.Events;

namespace Tofunaut.TofuUnity
{
    public abstract class AppStateController<T> : SingletonBehaviour<T> where T : MonoBehaviour
    {
        public bool IsComplete { get; private set; }
        public bool IsReady { get; protected set; }

        public virtual void Complete()
        {
            IsComplete = true;
        }
    }
}