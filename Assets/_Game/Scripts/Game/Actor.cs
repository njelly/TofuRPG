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

        public void InterpretPlayerInput(PlayerInput playerInput)
        {
            if (playerInput == null)
            {
                direction.SetDirection(Vector2.zero);
                if (interact.Held)
                {
                    interact.timeReleased = Time.time;
                }
            }
            else
            {
                direction.SetDirection(playerInput.direction);
                interact.timePressed = playerInput.select.timePressed;
                interact.timeReleased = playerInput.select.timeReleased;
                aim.timePressed = playerInput.rightTrigger.timePressed;
                aim.timeReleased = playerInput.rightTrigger.timeReleased;
            }
        }
    }

    public class Actor : MonoBehaviour, PlayerInputManager.IPlayerInputReceiver
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
        [SerializeField] private ActorBrain _brain;

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
            if (_recieveOnStart)
            {
                PlayerInputManager.AddReceiver(this);
            }
        }

        private void Update()
        {
            if (!_receivingPlayerInput && _brain)
            {
                _input = _brain.GetActorInput();
            }

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
            _toAdd.Add(receiver);
        }

        public void RemoveReceiver(IActorInputReceiver receiver)
        {
            _toRemove.Add(receiver);
        }

        public void ReceivePlayerInput(PlayerInput input)
        {
            _receivingPlayerInput = input != null;
            _input.InterpretPlayerInput(input);
            foreach (IActorInputReceiver receiver in _receivers)
            {
                receiver.ReceiveActorInput(_input);
            }
        }
    }
}