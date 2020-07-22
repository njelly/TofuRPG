using System.Collections.Generic;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class ActorInput
    {
        public Vector2 direction;
        public Button interact;

        public class Button
        {
            private float _timePressed;
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
                    _isDown = value;
                }
            }

            public bool WasPressed => _isDown && (Mathf.Abs(_timePressed - Time.time) <= float.Epsilon);
        }
    }

    public class ActorInputManager : SingletonBehaviour<ActorInputManager>
    {
        public float directionDeadZone = 0.2f;

        private List<Actor> _targets;
        private List<Actor> _toAdd;
        private List<Actor> _toRemove;
        private ActorInput _input;

        protected override void Awake()
        {
            base.Awake();

            _targets = new List<Actor>();
            _toAdd = new List<Actor>();
            _toRemove = new List<Actor>();
            _input = new ActorInput();
        }

        public static void AddTarget(Actor target)
        {
            _instance._toAdd.Add(target);
        }

        public static void RemoveTarget(Actor target)
        {
            _instance._toRemove.Add(target);
        }

        public void Update()
        {
            foreach (Actor target in _toAdd)
            {
                _targets.Add(target);
            }
            _toAdd.Clear();

            foreach (Actor target in _toRemove)
            {
                _targets.Remove(target);
            }
            _toRemove.Clear();

            PollInput();

            foreach (Actor target in _targets)
            {
                target.ReceiveInput(_input);
            }
        }

        private void PollInput()
        {
            _input.direction = Quaternion.LookRotation(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))).eulerAngles;
            if (_input.direction.sqrMagnitude < directionDeadZone)
            {
                _input.direction = Vector2.zero;
            }

            _input.interact.IsDown = Input.GetKey(KeyCode.Space);
        }
    }
}