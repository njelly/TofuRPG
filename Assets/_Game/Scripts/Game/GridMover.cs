using System.Collections.Generic;
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
        }

        private void OnEnable()
        {
            _actor.AddReceiver(this);
        }

        private void Update()
        {
            Debug.Log(_input.direction.ToString("F2"));
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

            if (!CanOccupy(newCoord))
            {
                // bump!
                return;
            }

            Vector2 prevPosition = new Vector2(Coord.x, Coord.y);
            _coord = newCoord;

            _moveSequence = gameObject.Sequence()
                .Curve(EEaseType.Linear, 1f / moveSpeed, (float newValue) =>
                {

                })
                .Then()
                .Execute(() =>
                {
                    _moveSequence = null;

                });
            _moveSequence.Play();
        }

        public bool CanOccupy(Vector2Int otherCoord)
        {
            if (otherCoord == Coord)
            {
                return true;
            }

            return false;
        }

        public void ReceiveActorInput(ActorInput input)
        {
            _input = input;
        }
    }
}