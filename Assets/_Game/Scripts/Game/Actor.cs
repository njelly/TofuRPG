using System;
using System.Collections.Generic;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.Events;

namespace Tofunaut.TofuRPG.Game
{
    public class ActorInput
    {
        public PlayerInput.DirectionButton direction;
        public PlayerInput.Button action;
        public PlayerInput.Button aim;

        public ActorInput()
        {
            direction = new PlayerInput.DirectionButton();
            action = new PlayerInput.Button();
            aim = new PlayerInput.Button();
        }
    }

    /// <summary>
    /// An Actor is something that receives ActorInput (from a Player or some other AI system) and interacts with the scene.
    /// </summary>
    [RequireComponent(typeof(GridCollider))]
    public class Actor : MonoBehaviour, PlayerInputManager.IPlayerInputReceiver, Actor.IActorInputReceiver
    {
        public interface IActorInputReceiver
        {
            void ReceiveActorInput(ActorInput input);
        }

        public interface IInteractable
        {
            void BeginInteraction(Actor actor);
            void EndInteraction(Actor actor);
        }

        [Serializable]
        public class ActorEvents
        {
            [Serializable] public class ActorEvent : UnityEvent<Actor> { }
            public ActorEvent OnActorBeganInteraction;
            public ActorEvent OnActorEndedInteraction;

            [Serializable] public class AimerEvent : UnityEvent<Aimer> { }
            public AimerEvent OnAimerBeganAiming;
            public AimerEvent OnAimerEndedAiming;
        }

        [Flags]
        public enum EState
        {
            None = 0,
            IsAiming = 1 << 0,
        }

        public ActorEvents Events => _events;
        public ECardinalDirection4 Facing { get; private set; }
        public EState State { get; private set; }
        public IInteractable InteractingWith { get; private set; }
        public bool CanInteract => (State & EState.IsAiming) == 0;

        [Header("Actor")]
        [SerializeField] private ActorBehaviour _behaviourPrefab;

        [Header("Player Input")]
        [SerializeField] private bool _recieveOnStart;

        [Space(20)]
        [SerializeField] private ActorEvents _events;

        private GridCollider _gridCollider;
        private ActorInput _input = new ActorInput();
        private List<IActorInputReceiver> _receivers = new List<IActorInputReceiver>();
        private List<IActorInputReceiver> _toAdd = new List<IActorInputReceiver>();
        private List<IActorInputReceiver> _toRemove = new List<IActorInputReceiver>();
        private bool _receivingPlayerInput;
        private ActorBehaviour _instantiatedActorBehaviour;

        private void Start()
        {
            _gridCollider = GetComponent<GridCollider>();

            if (_behaviourPrefab)
            {
                SetActorBehaviour(_behaviourPrefab);
            }
            else if (_recieveOnStart)
            {
                PlayerInputManager.AddReceiver(this);
            }
        }

        private void Update()
        {
            foreach (IActorInputReceiver receiver in _toAdd)
            {
                _receivers.Add(receiver);
            }
            _toAdd.Clear();

            foreach (IActorInputReceiver receiver in _toRemove)
            {
                _receivers.Remove(receiver);
            }
            _toRemove.Clear();
        }

        private void OnDestroy()
        {
            PlayerInputManager.RemoveReceiver(this);
        }

        public void AddReceiver(IActorInputReceiver receiver)
        {
            if (receiver is Actor && (Actor)receiver == this)
            {
                Debug.LogError("can't add as receiver from itself");
                return;
            }

            _toAdd.Add(receiver);
        }

        public void RemoveReceiver(IActorInputReceiver receiver)
        {
            _toRemove.Add(receiver);
        }

        public void ReceivePlayerInput(PlayerInput playerInput)
        {
            _receivingPlayerInput = playerInput != null;

            if (playerInput == null)
            {
                _input.direction.SetDirection(Vector2.zero);
                if (_input.action.Held)
                {
                    _input.action.timeReleased = Time.time;
                }
            }
            else
            {
                _input.direction.SetDirection(playerInput.direction);
                _input.action.timePressed = playerInput.select.timePressed;
                _input.action.timeReleased = playerInput.select.timeReleased;

                if (!_input.aim.Held)
                {
                    _input.aim.timePressed = Mathf.Max(playerInput.rightTrigger.timePressed, playerInput.shift.timePressed);
                }
                else
                {
                    _input.aim.timeReleased = Mathf.Max(playerInput.rightTrigger.timeReleased, playerInput.shift.timeReleased);
                }
            }

            ReceiveActorInput(_input);
        }

        public void ReceiveActorInput(ActorInput input)
        {
            if (input.direction.Direction.sqrMagnitude > float.Epsilon)
            {
                Facing = input.direction.Direction.ToCardinalDirection4();
            }

            if (InteractingWith == null)
            {
                // only change facing dir when not interacting with anything
                if (input.direction.Direction.sqrMagnitude > float.Epsilon)
                {
                    Facing = input.direction.Direction.ToCardinalDirection4();
                }

                if (CanInteract && input.action.Pressed)
                {
                    InteractingWith = TryGetInteractableAt(_gridCollider.Coord + Facing.ToVector2Int());
                    if (InteractingWith != null)
                    {
                        InteractingWith.BeginInteraction(this);
                        Events.OnActorBeganInteraction?.Invoke(this);
                    }
                }
            }
            else
            {
                if (input.action.Released || !CanInteract)
                {
                    InteractingWith.EndInteraction(this);
                    InteractingWith = null;
                    Events.OnActorBeganInteraction?.Invoke(this);
                }
            }

            _input = input;
            foreach (IActorInputReceiver receiver in _receivers)
            {
                receiver.ReceiveActorInput(_input);
            }
        }

        public void SetActorBehaviour(ActorBehaviour behaviourPrefab)
        {
            if (_instantiatedActorBehaviour)
            {
                Destroy(_instantiatedActorBehaviour.gameObject);
            }

            _instantiatedActorBehaviour = Instantiate(behaviourPrefab, Vector3.zero, Quaternion.identity, transform);
            _instantiatedActorBehaviour.Initialize(this);
        }

        public static IInteractable TryGetInteractableAt(Vector2Int coordinate)
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
    }
}