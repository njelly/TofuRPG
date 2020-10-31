using UnityEngine;

namespace Tofunaut.TofuRPG.Game.Interfaces
{
    public class ActorInput
    {
        public Vector2 direction;
        public bool interact;
    }

    public interface IActorInputProvider
    {
        ActorInput ActorInput { get; }
    }
}