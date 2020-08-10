using System;
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

        [Flags]
        private enum EState // use bit flags to keep long list of arbitrary states
        {
            None = 0,
            DontInteractWhileAiming = 1 << 0,
        }

        public ECardinalDirection4 Facing { get; private set; }
        public IInteractable InteractingWith { get; private set; }
        public bool CanInteract
        {
            get
            {
                return (_state & EState.DontInteractWhileAiming) == 0;
            }
        }

        private Actor _actor;
        private GridCollider _gridCollider;
        private ECardinalDirection4 _prevFacing;
        private EState _state;

        private void Awake()
        {
            _actor = gameObject.GetComponent<Actor>();
            _gridCollider = gameObject.GetComponent<GridCollider>();
            _state = EState.None;
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
            if (InteractingWith == null)
            {
                if (input.direction.Direction.sqrMagnitude > float.Epsilon)
                {
                    Facing = input.direction.Direction.ToCardinalDirection4();
                }

                if (CanInteract && input.interact.Pressed)
                {
                    InteractingWith = TryGetInteractableAt(_gridCollider.Coord + Facing.ToVector2Int());
                    if (InteractingWith != null)
                    {
                        InteractingWith.BeginInteraction(this);
                        _actor.Events.OnInteractorBeganInteraction?.Invoke(this);
                    }
                }
            }
            else
            {
                if (input.interact.Released || !CanInteract)
                {
                    InteractingWith.EndInteraction(this);
                    InteractingWith = null;
                    _actor.Events.OnInteractorBeganInteraction?.Invoke(this);
                }
            }
        }

        public IInteractable TryGetInteractableAt(Vector2Int coordinate)
        {
            foreach (GridCollider gridCollider in GridCollisionManager.GetCollidersAt(coordinate))
            {
                IInteractable interactable = gridCollider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    return interactable;
                }
            }

            return null;
        }

        public void DontInteractWhileAiming(Aimer aimer)
        {
            if (aimer.IsAiming)
            {
                _state |= EState.DontInteractWhileAiming;
            }
            else
            {
                _state &= ~EState.DontInteractWhileAiming;
            }
        }
    }
}