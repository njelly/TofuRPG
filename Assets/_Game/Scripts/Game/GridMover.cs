using System;
using System.Linq;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuUnity;
using UnityEngine;
using static Tofunaut.TofuUnity.TofuAnimator;

namespace Tofunaut.TofuRPG.Game
{
    public class GridMover : GridCollider
    {
        public ECardinalDirection4 MoveDirection { get; private set; }
        public bool IsMoving => _moveSequence != null;

        [Header("Movement")]
        public float moveSpeed;
        public float moveHesitationTime;

        private Sequence _moveSequence;
        private IActorInputProvider _actorInputProvider;

        private void Start()
        {
            var components = GetComponents<MonoBehaviour>();
            _actorInputProvider = components.OfType<IActorInputProvider>().FirstOrDefault();
        }

        protected override void Update()
        {
            base.Update();
            
            if (_actorInputProvider == null)
                return;

            var actorInput = _actorInputProvider.ActorInput;
            if (actorInput.Direction.Direction.HasLength())
                TryMoveTo(Coord + actorInput.Direction.Direction.ToCardinalDirection4().ToVector2Int());
        }

        public override bool TryMoveTo(Vector2Int newCoord)
        {
            var prevCoord = Coord;

            if (_moveSequence != null)
                // can't move while animation is playing
                return false;

            MoveDirection = ((Vector2)(newCoord - prevCoord)).ToCardinalDirection4();

            if (!base.TryMoveTo(newCoord))
                return false;

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
    }
}