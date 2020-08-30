using System;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Aimer : ActorComponent
    {
        public Vector2 AimVector => _aimVector;
        public bool IsAiming { get; private set; }

        [SerializeField] private Vector2 _aimVector;

        private GridCollider _gridCollider;

        private void Start()
        {
            _aimVector = _actor.Facing.ToVector2();
            _gridCollider = gameObject.GetComponent<GridCollider>();
        }

        public override void ReceiveActorInput(ActorInput input)
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
    }
}