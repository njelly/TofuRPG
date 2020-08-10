using System;
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
        public Button rightTrigger;
        public Button shift;

        public PlayerInput()
        {
            direction = new DirectionButton();
            select = new Button();
            back = new Button();
            rightTrigger = new Button();
            shift = new Button();
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
        private List<Tuple<int, IPlayerInputReceiver>> _receivers;
        private HashSet<Tuple<int, IPlayerInputReceiver>> _toAdd;
        private HashSet<IPlayerInputReceiver> _toRemove;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(this);

            _input = new PlayerInput();
            _receivers = new List<Tuple<int, IPlayerInputReceiver>>();
            _toAdd = new HashSet<Tuple<int, IPlayerInputReceiver>>();
            _toRemove = new HashSet<IPlayerInputReceiver>();
        }

        private void Update()
        {
            PollInput();

            bool didChange = false;
            foreach (Tuple<int, IPlayerInputReceiver> receiver in _toAdd)
            {
                didChange = true;
                _receivers.Add(receiver);
            }
            _toAdd.Clear();

            foreach (IPlayerInputReceiver receiver in _toRemove)
            {
                didChange = true;
                _receivers.RemoveAll((Tuple<int, IPlayerInputReceiver> tuple) => { return tuple.Item2 == receiver; });
            }
            _toRemove.Clear();

            if (_receivers.Count <= 0)
            {
                return;
            }

            if (didChange)
            {
                // sort so higher numbers come first
                _receivers.Sort((Tuple<int, IPlayerInputReceiver> a, Tuple<int, IPlayerInputReceiver> b) =>
                {
                    return b.Item1.CompareTo(a.Item1);
                });
            }

            int startPriority = _receivers[0].Item1;
            for (int i = 0; i < _receivers.Count; i++)
            {
                if (_receivers[i].Item1 != startPriority)
                {
                    // only send the highest priority receivers the input
                    break;
                }

                _receivers[i].Item2.ReceivePlayerInput(_input);
            }
        }

        public static void AddReceiver(IPlayerInputReceiver receiver) => AddReceiver(0, receiver);
        public static void AddReceiver(int priority, IPlayerInputReceiver receiver)
        {
            _instance._toAdd.Add(new Tuple<int, IPlayerInputReceiver>(priority, receiver));
        }

        public static void RemoveReceiver(IPlayerInputReceiver receiver)
        {
            if (_instance)
            {
                bool isReceiver = false;
                foreach (Tuple<int, IPlayerInputReceiver> t in _instance._receivers)
                {
                    if (t.Item2 == receiver)
                    {
                        isReceiver = true;
                        break;
                    }
                }

                if (!isReceiver)
                {
                    return;
                }

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

            float rightTriggerAxis = Input.GetAxis("XBoxRightTrigger");
            if (!_input.rightTrigger.Held && rightTriggerAxis > 0)
            {
                _input.rightTrigger.timePressed = Time.time;
            }
            else if (_input.rightTrigger.Held && rightTriggerAxis <= 0)
            {
                _input.rightTrigger.timeReleased = Time.time;
            }

            if (!_input.shift.Held && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
            {
                _input.shift.timePressed = Time.time;
            }
            else if (_input.shift.Held && (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)))
            {
                _input.shift.timeReleased = Time.time;
            }
        }
    }
}