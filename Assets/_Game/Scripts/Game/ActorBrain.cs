using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public abstract class ActorBrain : MonoBehaviour
    {
        public abstract ActorInput GetActorInput();
    }
}