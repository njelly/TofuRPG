using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class ActorInput
    {
        public Vector2 direction;
        public PlayerInput.Button interact;

        public ActorInput()
        {
            direction = Vector2.zero;
            interact = new PlayerInput.Button();
        }

        public void InterpretPlayerInput(PlayerInput playerInput)
        {
            direction = playerInput.direction;
            interact = playerInput.select;
        }
    }

    public class Actor : MonoBehaviour, PlayerInputManager.IPlayerInputReceiver
    {
        public interface IActorInputReceiver
        {
            void ReceiveActorInput(ActorInput input);
        }

        [Header("Actor")]
        [SerializeField] private ActorBrain _brain;

        [Header("Player Input")]
        [SerializeField] private bool _recieveOnStart;

        private ActorInput _input;
        private List<IActorInputReceiver> _receivers;
        private List<IActorInputReceiver> _toAdd;
        private List<IActorInputReceiver> _toRemove;
        private bool _receivingPlayerInput;

        private void Awake()
        {
            _input = new ActorInput();
            _receivers = new List<IActorInputReceiver>();
            _toAdd = new List<IActorInputReceiver>();
            _toRemove = new List<IActorInputReceiver>();
        }

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

            foreach (IActorInputReceiver receiver in _receivers)
            {
                receiver.ReceiveActorInput(_input);
            }
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
            if (input == null)
            {
                _receivingPlayerInput = false;
                _input.direction = Vector2.zero;
                return;
            }

            _receivingPlayerInput = true;
            _input.InterpretPlayerInput(input);
        }
    }
}