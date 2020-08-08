using System.Collections.Generic;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class PlayerInput
    {
        public DirectionButton direction;
        public Button select;
        public Button back;

        public PlayerInput()
        {
            direction = new DirectionButton();
            select = new Button();
            back = new Button();
        }

        public class Button
        {
            public float timePressed;
            public float timeReleased;

            public bool Pressed => Time.time - timePressed < Time.deltaTime;
            public bool Held => timePressed > timeReleased;
            public bool Released => Time.time - timeReleased < Time.deltaTime;
        }

        public class DirectionButton : Button
        {
            public Vector2 Direction { get; private set; } = Vector2.zero;

            public void SetDirection(Vector2 direction)
            {
                if (Direction != direction)
                {
                    if (direction.sqrMagnitude >= float.Epsilon)
                    {
                        timePressed = Time.time;
                    }
                    else
                    {
                        timeReleased = Time.time;
                    }
                }

                Direction = direction;
            }

            public static implicit operator Vector2(DirectionButton button)
            {
                return button.Held ? button.Direction : Vector2.zero;
            }
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
            Vector2 rawDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (rawDir.sqrMagnitude > float.Epsilon)
            {
                _input.direction.SetDirection(rawDir.normalized);
            }
            else
            {
                _input.direction.SetDirection(Vector2.zero);
            }

            if (Input.GetButtonDown("Submit"))
            {
                _input.select.timePressed = Time.time;
            }
            else if (Input.GetButtonUp("Submit"))
            {
                _input.select.timeReleased = Time.time;
            }
        }
    }
}