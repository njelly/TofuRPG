using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [RequireComponent(typeof(Actor))]
    public class ActorFacing : MonoBehaviour, IActorInputReceiver
    {
        public ECardinalDirection4 Direction { get; private set; }

        private GridMover _gridMover;

        private void Awake()
        {
            _gridMover = GetComponent<GridMover>();
        }

        public void ReceiveActorInput(ActorInput actorInput)
        {
            if(!actorInput.direction.Held)
            {
                return;
            }

            ECardinalDirection4 direction = actorInput.direction.Direction.ToCardinalDirection4();

            // if the actor has a GridMover component, only change direction if moving in the same direction as the actorInput.direction
            if(_gridMover && _gridMover.IsMoving && direction != _gridMover.MoveDirection)
            {
                direction = Direction;
            }

            Direction = direction;
        }
    }
}