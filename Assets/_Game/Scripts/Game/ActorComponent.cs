using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public abstract class ActorComponent : MonoBehaviour
    {
        public Actor Actor { get; private set; }

        public virtual void Initialize(Actor actor, ActorModel model)
        {
            Actor = actor;
        }
    }
}