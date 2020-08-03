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

        private Actor _actor;
        private ActorInput _input;
        private TofuAnimator.Sequence _moveSequence;

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

        protected override void Update()
        {
            base.Update();

            if (_moveSequence == null && _input.direction.sqrMagnitude > float.Epsilon)
            {
                TryMoveTo(Coord + _input.direction.ToCardinalDirection4().ToVector2Int());
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_actor)
            {
                _actor.RemoveReceiver(this);
            }
        }

        private void TryMoveTo(Vector2Int newCoord)
        {
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
                    if (_input.direction.sqrMagnitude > float.Epsilon)
                    {
                        TryMoveTo(Coord + _input.direction.ToCardinalDirection4().ToVector2Int());
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