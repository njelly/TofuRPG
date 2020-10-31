using System;
using System.Linq;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuUnity;
using UnityEngine;
using static Tofunaut.TofuUnity.TofuAnimator;

namespace Tofunaut.TofuRPG.Game
{
    public class GridMover : GridCollider, IFacing
    {
        public ECardinalDirection4 Facing { get; private set; }
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
            
            ProcessActorInput();
        }

        private void ProcessActorInput()
        {
            if (_actorInputProvider == null)
                return;
            
            var actorInput = _actorInputProvider.ActorInput;
            if (!actorInput.Direction.Direction.HasLength()) 
                return;
            
            var newCoord = Coord + actorInput.Direction.Direction.ToCardinalDirection4().ToVector2Int();
            if (_moveSequence == null)
                Facing = ((Vector2)(newCoord - Coord)).ToCardinalDirection4();
            if (actorInput.Direction.TimeHeld > moveHesitationTime)
                TryMoveTo(newCoord);
        }

        public override bool TryMoveTo(Vector2Int newCoord)
        {
            var prevCoord = Coord;
            if (_moveSequence != null)
                // can't move while animation is playing
                return false;
            
            Facing = ((Vector2)(newCoord - prevCoord)).ToCardinalDirection4();

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
                    ProcessActorInput();
                });
            _moveSequence.Play();

            return true;
        }
    }
}