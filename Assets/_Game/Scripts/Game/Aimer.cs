using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Aimer : MonoBehaviour, Actor.IActorInputReceiver
    {
        public Vector2 AimVector => _aimVector;
        public bool IsAiming { get; private set; }

        [SerializeField] private Vector2 _aimVector;

        private Actor _actor;
        private Interactor _interactor;
        private GridMover _gridMover;

        private void Awake()
        {
            _actor = gameObject.GetComponent<Actor>();

            _interactor = gameObject.GetComponent<Interactor>();
            if (_interactor)
            {
                _aimVector = _interactor.Facing.ToVector2();
            }

            _gridMover = gameObject.GetComponent<GridMover>();
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

            if (_interactor)
            {
                _aimVector = _interactor.Facing.ToVector2();
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