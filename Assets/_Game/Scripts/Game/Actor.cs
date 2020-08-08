using System;
using System.Collections.Generic;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class ActorInput
    {
        public PlayerInput.DirectionButton direction;
        public PlayerInput.Button interact;

        public ActorInput()
        {
            direction = new PlayerInput.DirectionButton();
            interact = new PlayerInput.Button();
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
            }
        }
    }

    public class Actor : MonoBehaviour, PlayerInputManager.IPlayerInputReceiver
    {
        public interface IActorInputReceiver
        {
            void ReceiveActorInput(ActorInput input);
        }

        public class Signals
        {
            public event EventHandler OnInteractorBeganInteraction;
            public event EventHandler OnInteractorEndedInteraction;
        }

        public Signals Signals { get; private set; }

        [Header("Actor")]
        [SerializeField] private ActorBrain _brain;

        [Header("Player Input")]
        [SerializeField] private bool _recieveOnStart;

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