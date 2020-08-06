using Tofunaut.TofuUnity;
using UnityEngine;
using static Tofunaut.TofuUnity.TofuAnimator;

namespace Tofunaut.TofuRPG.Game
{
    [RequireComponent(typeof(Actor))]
    public class GridMover : GridCollider, Actor.IActorInputReceiver
    {
        [Header("Movement")]
        public float moveSpeed;
        public float moveHesitationTime;

        private Actor _actor;
        private ActorInput _input;
        private TofuAnimator.Sequence _moveSequence;
        private float _lastZeroDirectionTime;

        protected override void Awake()
        {
            base.Awake();

            _actor = gameObject.GetComponent<Actor>();
            _input = new ActorInput();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _actor.AddReceiver(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_actor)
            {
                _actor.RemoveReceiver(this);
            }
        }

        protected override void Update()
        {
            base.Update();

            if (_input.direction.Direction.sqrMagnitude > float.Epsilon)
            {
                if (_moveSequence == null)
                {
                    TryMoveTo(Coord + _input.direction.Direction.ToCardinalDirection4().ToVector2Int());
                }
            }
            else
            {
                _lastZeroDirectionTime = Time.time;
            }
        }

        private void TryMoveTo(Vector2Int newCoord)
        {
            if (Time.time - _lastZeroDirectionTime < moveHesitationTime)
            {
                return;
            }

            if (_moveSequence != null)
            {
                // can't move while animation is playing
                return;
            }

            if (!GridCollisionManager.TryMove(this, Coord, newCoord))
            {
                // bump!
                return;
            }

            Vector2 prevPosition = new Vector2(Coord.x, Coord.y);
            Coord = newCoord;

            _moveSequence = gameObject.Sequence()
                .Curve(EEaseType.Linear, 1f / moveSpeed, (float newValue) =>
                {
                    transform.position = Vector2.LerpUnclamped(prevPosition, Coord, newValue);
                })
                .Then()
                .Execute(() =>
                {
                    _moveSequence = null;
                    if (_input.direction.Direction.sqrMagnitude > float.Epsilon)
                    {
                        TryMoveTo(Coord + _input.direction.Direction.ToCardinalDirection4().ToVector2Int());
                    }
                });
            _moveSequence.Play();
        }

        public void ReceiveActorInput(ActorInput input)
        {
            _input = input;
        }
    }
}