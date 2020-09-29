using System.Collections.Generic;
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
        public Vector2Int InteractOffset => _interactOffset;

        [SerializeField] private Vector2Int _interactOffset;

        private ActorFacing _actorFacing;
        private GridCollider _gridCollider;

        private void Awake()
        {
            Actor = GetComponent<Actor>();
            _actorFacing = GetComponent<ActorFacing>();
            _gridCollider = GetComponent<GridCollider>();
        }

        private void LateUpdate()
        {
            if(_actorFacing)
            {
                _interactOffset = _actorFacing.Direction.ToVector2Int();
            }
        }

        public void ReceiveActorInput(ActorInput actorInput)
        {
            if(InteractingWith == null && actorInput.interact.Pressed)
            {
                Vector2Int interactWithCoord = _gridCollider.Coord + _interactOffset;
                GridCollider[] gridColliders = GameContext.GridCollisionManager.GetCollidersAt(interactWithCoord);
                foreach(GridCollider gc in gridColliders)
                {
                    IInteractable[] interactables = gc.GetComponents<MonoBehaviour>().OfType<IInteractable>().ToArray();
                    if(interactables.Length > 0)
                    {
                        InteractingWith = interactables[0];
                        Actor.SetFlag(EActorFlag.IsInteracting, true);
                        InteractingWith.BeginInteraction(Actor);
                        break;
                    }
                }
            }
            else if (InteractingWith != null && actorInput.interact.Released)
            {
                InteractingWith.EndInteraction(Actor);
                Actor.SetFlag(EActorFlag.IsInteracting, false);
                InteractingWith = null;
            }
        }
    }
}