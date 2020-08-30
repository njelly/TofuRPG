using System;
using Tofunaut.TofuUnity;
using UnityEngine;
using static Tofunaut.TofuUnity.TofuAnimator;

namespace Tofunaut.TofuRPG.Game
{
    [RequireComponent(typeof(Actor))]
    [RequireComponent(typeof(GridCollider))]
    public class GridMover : ActorComponent
    {
        public bool CanMove => (_actor.State & Actor.EState.IsAiming) != 0;
        public GridCollider Collider => _gridCollider;

        public Vector2Int Coord
        {
            get
            {
                if (_gridCollider)
                {
                    return _gridCollider.Coord;
                }
                return Vector2Int.zero;
            }
        }

        [Header("Movement")]
        public float moveSpeed;
        public float moveHesitationTime;

        private ActorInput _input;
        private TofuAnimator.Sequence _moveSequence;
        private float _lastZeroDirectionTime;
        private bool _stopForAimer;
        private GridCollider _gridCollider;

        protected override void Awake()
        {
            base.Awake();

            _actor = gameObject.GetComponent<Actor>();
            _gridCollider = gameObject.GetComponent<GridCollider>();
            _input = new ActorInput();
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

            Vector2 prevPosition = new Vector2(_gridCollider.Coord.x, _gridCollider.Coord.y);
            if (!_gridCollider.TryMoveTo(newCoord))
            {
                //bump
                return;
            }
            _moveSequence = gameObject.Sequence()
                .Curve(EEaseType.Linear, 1f / moveSpeed, (float newValue) =>
                {
                    transform.position = Vector2.LerpUnclamped(prevPosition, _gridCollider.Coord, newValue);
                })
                .Then()
                .Execute(() =>
                {
                    _moveSequence = null;
                    if (_input.direction.Direction.sqrMagnitude > float.Epsilon)
                    {
                        TryMoveTo(_gridCollider.Coord + _input.direction.Direction.ToCardinalDirection4().ToVector2Int());
                    }
                });
            _moveSequence.Play();
        }

        public override void ReceiveActorInput(ActorInput input)
        {
            if (CanMove && input.direction.Direction.sqrMagnitude > float.Epsilon)
            {
                if (_moveSequence == null)
                {
                    TryMoveTo(_gridCollider.Coord + _input.direction.Direction.ToCardinalDirection4().ToVector2Int());
                }
            }
            else
            {
                _lastZeroDirectionTime = Time.time;
            }
        }
    }
}