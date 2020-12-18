using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public abstract class ActorComponent : MonoBehaviour
    {
        public abstract void Initialize(Actor actor, ActorModel model);
    }
}