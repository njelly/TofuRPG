using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public abstract class ActorViewComponent : MonoBehaviour
    {
        public ActorView View { get; private set; }
        public bool IsInitialized => View;

        public virtual void Initialize(ActorView actorView)
        {
            View = actorView;
        }
    }
}