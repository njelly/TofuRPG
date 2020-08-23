using System;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Aimer : MonoBehaviour, Actor.IActorInputReceiver
    {
        [Flags]
        private enum EState // use bit flags to keep long list of arbitrary states
        {
            None = 0,
            DontAimWhileInteracting = 1 << 0,
        }

        public Vector2 AimVector => _aimVector;
        public bool IsAiming { get; private set; }

        [SerializeField] private Vector2 _aimVector;

        private Actor _actor;
        private GridCollider _gridCollider;

        private void Start()
        {
            _actor = gameObject.GetComponent<Actor>();
            _aimVector = _actor.Facing.ToVector2();
            _gridCollider = gameObject.GetComponent<GridCollider>();
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
            if (input.aim.Held)
            {
                if (input.direction.Held)
                {
                    _aimVector = input.direction;
                }

                TryToggleAiming(true);

                return;
            }

            if (_actor)
            {
                _aimVector = _actor.Facing.ToVector2();
            }

            TryToggleAiming(false);
        }

        private void TryToggleAiming(bool on)
        {
            if (IsAiming == on)
            {
                return;
            }

            IsAiming = on;

            if (IsAiming)
            {
                _actor.Events.OnAimerBeganAiming?.Invoke(this);
            }
            else
            {
                _actor.Events.OnAimerEndedAiming?.Invoke(this);
            }
        }

        public void DontAimWhileInteracting(Actor actor)
        {

        }
    }
}