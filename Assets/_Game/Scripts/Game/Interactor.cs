using System.Linq;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [RequireComponent(typeof(Actor))]
    [RequireComponent(typeof(ActorFacing))]
    [RequireComponent(typeof(GridCollider))]
    public class Interactor : MonoBehaviour, IActorInputReceiver
    {
        public Actor Actor { get; private set; }
        public IInteractable InteractingWith { get; private set; }

        private ActorFacing _actorFacing;
        private GridCollider _gridCollider;

        private void Awake()
        {
            Actor = GetComponent<Actor>();
            _actorFacing = GetComponent<ActorFacing>();
            _gridCollider = GetComponent<GridCollider>();
        }

        public void ReceiveActorInput(ActorInput actorInput)
        {
            if(InteractingWith == null && actorInput.interact.Pressed)
            {
                Vector2Int interactWithCoord = _gridCollider.Coord + _actorFacing.Direction.ToVector2Int();
                IInteractable interactable = GameContext.GridCollisionManager.GetCollidersAt(interactWithCoord).OfType<IInteractable>().FirstOrDefault();
                if (interactable != null)
                {
                    InteractingWith = interactable;
                    InteractingWith.BeginInteraction(Actor);
                }
            }
            else if (InteractingWith != null && actorInput.interact.Released)
            {
                InteractingWith.EndInteraction(Actor);
                InteractingWith = null;
            }
        }
    }
}