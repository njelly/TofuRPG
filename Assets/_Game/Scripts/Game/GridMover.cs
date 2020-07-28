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

        private void OnEnable()
        {
            _actor.AddReceiver(this);
        }

        private void Update()
        {
            if (_moveSequence == null && _input.direction.sqrMagnitude > float.Epsilon)
            {
                TryMoveTo(_coord + _input.direction.ToCardinalDirection4().ToVector2Int());
            }
        }

        private void OnDisable()
        {
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

            if (!GridCollisionManager.TryOccupy(this, newCoord))
            {
                // bump!
                return;
            }

            Vector2 prevPosition = new Vector2(Coord.x, Coord.y);
            Vector2 newPosition = new Vector2(newCoord.x, newCoord.y);
            _coord = newCoord;

            _moveSequence = gameObject.Sequence()
                .Curve(EEaseType.Linear, 1f / moveSpeed, (float newValue) =>
                {
                    transform.position = Vector2.LerpUnclamped(prevPosition, newPosition, newValue);
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