using System;
using Tofunaut.TofuUnity;
using UnityEngine;
using static Tofunaut.TofuUnity.TofuAnimator;

namespace Tofunaut.TofuRPG.Game
{
    public class GridMover : GridCollider, Actor.IActorInputReceiver
    {

        [Header("Movement")]
        public float moveSpeed;
        public float moveHesitationTime;

        private TofuAnimator.Sequence _moveSequence;
        private float _lastZeroDirectionTime;
        private bool _stopForAimer;
        private ActorInput _actorInput;

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
                    ReceiveActorInput(_actorInput);
                });
            _moveSequence.Play();

            return true;
        }

        public void ReceiveActorInput(ActorInput actorInput)
        {
            _actorInput = actorInput;
            if (_actorInput.direction.Held)
            {
                TryMoveTo(Coord + _actorInput.direction.Direction.ToCardinalDirection4().ToVector2Int());
            }
        }
    }
}