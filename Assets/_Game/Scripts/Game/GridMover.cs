using System;
using Tofunaut.TofuUnity;
using UnityEngine;
using static Tofunaut.TofuUnity.TofuAnimator;

namespace Tofunaut.TofuRPG.Game
{
    public class GridMover : GridCollider, Actor.IActorInputReceiver
    {
        public ECardinalDirection4 MoveDirection { get; private set; }
        public bool IsMoving => _moveSequence != null;

        [Header("Movement")]
        public float moveSpeed;
        public float moveHesitationTime;

        private Sequence _moveSequence;
        private float _lastZeroDirectionTime;

        public override bool TryMoveTo(Vector2Int newCoord)
        {
            Vector2Int prevCoord = Coord;

            if (Time.time - _lastZeroDirectionTime < moveHesitationTime)
            {
                return false;
            }

            if (_moveSequence != null)
            {
                // can't move while animation is playing
                return false;
            }

            MoveDirection = ((Vector2)(newCoord - prevCoord)).ToCardinalDirection4();

            if (!base.TryMoveTo(newCoord))
            {
                return false;
            }

            _moveSequence = gameObject.Sequence()
                .Curve(EEaseType.Linear, 1f / moveSpeed, (float newValue) =>
                {
                    transform.position = Vector2.LerpUnclamped(prevCoord, Coord, newValue);
                })
                .Then()
                .Execute(() =>
                {
                    _moveSequence = null;
                });
            _moveSequence.Play();

            return true;
        }

        public void ReceiveActorInput(ActorInput actorInput)
        {
            if (actorInput.direction.TimeHeld > moveHesitationTime)
            {
                TryMoveTo(Coord + actorInput.direction.Direction.ToCardinalDirection4().ToVector2Int());
            }
        }
    }
}