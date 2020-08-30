using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    ///<summary>
    /// ActorComponents receive input and modify states that are only internally mutable.
    ///</summary>
    [RequireComponent(typeof(Actor))]
    public abstract class ActorComponent : MonoBehaviour
    {
        protected Actor _actor;

        protected virtual void Awake()
        {
            _actor = GetComponent<Actor>();
        }

        public abstract void ReceiveActorInput(ActorInput input);
    }
}