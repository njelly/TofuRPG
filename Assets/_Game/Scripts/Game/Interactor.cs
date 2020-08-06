using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [RequireComponent(typeof(Actor))]
    [RequireComponent(typeof(GridCollider))]
    public class Interactor : MonoBehaviour, Actor.IActorInputReceiver
    {
        public interface IInteractable
        {
            void BeginInteraction(Interactor interactor);
            void EndInteraction(Interactor interactor);
        }

        public ECardinalDirection4 Facing { get; private set; }
        public IInteractable InteractingWith { get; private set; }

        private Actor _actor;
        private GridCollider _gridCollider;
        private ECardinalDirection4 _prevFacing;

        private void Awake()
        {
            _actor = gameObject.GetComponent<Actor>();
            _gridCollider = gameObject.GetComponent<GridCollider>();
        }

        private void OnEnable()
        {
            _actor.AddReceiver(this);
        }

        private void OnDisable()
        {
            if (_actor)
            {
                _actor.RemoveReceiver(this);
            }
        }

        public void ReceiveActorInput(ActorInput input)
        {
            if (input.direction.sqrMagnitude > float.Epsilon)
            {
                Facing = input.direction.ToCardinalDirection4();
            }

            if (InteractingWith == null)
            {
                if (input.interact.WasPressed)
                {
                    InteractingWith = TryGetInteractableAt(_gridCollider.Coord + Facing.ToVector2Int());
                    if (InteractingWith != null)
                    {
                        InteractingWith.BeginInteraction(this);
                    }
                }
            }
            else
            {
                if (input.interact.WasReleased)
                {
                    InteractingWith = null;
                    InteractingWith.EndInteraction(this);
                }
            }
        }

        public IInteractable TryGetInteractableAt(Vector2Int coordinate)
        {
            foreach (GridCollider gridCollider in GridCollisionManager.GetCollidersAt(coordinate))
            {
                Debug.Log(gridCollider.name);
                IInteractable interactable = gridCollider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    return interactable;
                }
            }

            return null;
        }
    }
}