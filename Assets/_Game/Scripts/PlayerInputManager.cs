using System.Collections.Generic;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class PlayerInput
    {
        public Vector2 direction;
        public Button select;
        public Button back;

        public PlayerInput()
        {
            direction = Vector2.zero;
            select = new Button();
            back = new Button();
        }

        public class Button
        {
            private float _timePressed;
            private float _timeReleased;
            private bool _isDown;

            public bool IsDown
            {
                get
                {
                    return _isDown;
                }
                set
                {
                    if (!_isDown && value)
                    {
                        _timePressed = Time.time;
                    }
                    if (_isDown && !value)
                    {
                        _timeReleased = Time.time;
                    }
                    _isDown = value;
                }
            }

            public bool WasPressed => _isDown && (Time.time - _timePressed - Time.deltaTime <= float.Epsilon);
            public bool WasReleased => !_isDown && (Time.time - _timeReleased - Time.deltaTime <= float.Epsilon);
        }
    }

    public class PlayerInputManager : SingletonBehaviour<PlayerInputManager>
    {
        public interface IPlayerInputReceiver
        {
            void ReceivePlayerInput(PlayerInput input);
        }

        public float directionDeadZone = 0.2f;

        private PlayerInput _input;
        private List<IPlayerInputReceiver> _receivers;
        private List<IPlayerInputReceiver> _toAdd;
        private List<IPlayerInputReceiver> _toRemove;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(this);

            _input = new PlayerInput();
            _receivers = new List<IPlayerInputReceiver>();
            _toAdd = new List<IPlayerInputReceiver>();
            _toRemove = new List<IPlayerInputReceiver>();
        }

        private void Update()
        {
            PollInput();

            foreach (IPlayerInputReceiver receiver in _toAdd)
            {
                _receivers.Add(receiver);
            }
            _toAdd.Clear();

            foreach (IPlayerInputReceiver receiver in _toRemove)
            {
                _receivers.Remove(receiver);
            }
            _toRemove.Clear();

            foreach (IPlayerInputReceiver receiver in _receivers)
            {
                receiver.ReceivePlayerInput(_input);
            }
        }

        public static void AddReceiver(IPlayerInputReceiver receiver)
        {
            _instance._toAdd.Add(receiver);
        }

        public static void RemoveReceiver(IPlayerInputReceiver receiver)
        {
            if (_instance)
            {
                _instance._toRemove.Add(receiver);
            }
        }

        private void PollInput()
        {
            Vector3 rawDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            if (rawDir.sqrMagnitude > float.Epsilon)
            {
                _input.direction = rawDir.normalized;
            }
            else
            {
                _input.direction = Vector2.zero;
            }

            _input.select.IsDown = Input.GetKey(KeyCode.Space);
        }
    }
}