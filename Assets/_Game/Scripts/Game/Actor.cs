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
        public PlayerInput.Button interact;
        public PlayerInput.Button aim;

        public ActorInput()
        {
            direction = new PlayerInput.DirectionButton();
            interact = new PlayerInput.Button();
            aim = new PlayerInput.Button();
        }
    }

    public class Actor : MonoBehaviour, PlayerInputManager.IPlayerInputReceiver, Actor.IActorInputReceiver
    {
        public interface IActorInputReceiver
        {
            void ReceiveActorInput(ActorInput input);
        }

        [Serializable]
        public class ActorEvents
        {
            [Serializable] public class AimerEvent : UnityEvent<Aimer> { }
            public AimerEvent OnAimerBeganAiming;
            public AimerEvent OnAimerEndedAiming;
            [Serializable] public class InteractorEvent : UnityEvent<Interactor> { }
            public InteractorEvent OnInteractorBeganInteraction;
            public InteractorEvent OnInteractorEndedInteraction;
        }

        public ActorEvents Events => _events;

        [Header("Actor")]
        [SerializeField] private ActorBehaviour _behaviourPrefab;

        [Header("Player Input")]
        [SerializeField] private bool _recieveOnStart;

        [Space(20)]
        [SerializeField] private ActorEvents _events;

        private ActorInput _input = new ActorInput();
        private List<IActorInputReceiver> _receivers = new List<IActorInputReceiver>();
        private List<IActorInputReceiver> _toAdd = new List<IActorInputReceiver>();
        private List<IActorInputReceiver> _toRemove = new List<IActorInputReceiver>();
        private bool _receivingPlayerInput;

        private void Start()
        {
            if (_behaviourPrefab)
            {
                SetBehaviour(_behaviourPrefab);
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
                if (_input.interact.Held)
                {
                    _input.interact.timeReleased = Time.time;
                }
            }
            else
            {
                _input.direction.SetDirection(playerInput.direction);
                _input.interact.timePressed = playerInput.select.timePressed;
                _input.interact.timeReleased = playerInput.select.timeReleased;

                if (!_input.aim.Held)
                {
                    _input.aim.timePressed = Mathf.Max(playerInput.rightTrigger.timePressed, playerInput.shift.timePressed);
                }
                else
                {
                    _input.aim.timeReleased = Mathf.Max(playerInput.rightTrigger.timeReleased, playerInput.shift.timeReleased);
                }
            }

            foreach (IActorInputReceiver receiver in _receivers)
            {
                receiver.ReceiveActorInput(_input);
            }
        }

        public void ReceiveActorInput(ActorInput input)
        {
            _input = input;
            foreach (IActorInputReceiver receiver in _receivers)
            {
                receiver.ReceiveActorInput(_input);
            }
        }

        public void SetBehaviour(ActorBehaviour behaviourPrefab)
        {
            behaviourPrefab.Initialize(this);
        }
    }
}