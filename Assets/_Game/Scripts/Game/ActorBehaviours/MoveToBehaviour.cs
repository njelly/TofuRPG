using System.Collections.Generic;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class MoveToBehaviour : ActorBehaviour
    {
        public Vector2Int target;
        public float failAfterCantMoveTime;

        [Header("Random Reset")]
        [Tooltip("The extents of the rectangle to draw around the current coord from which to choose a random target")]
        public Vector2Int randomizeRangeOnEnable = Vector2Int.zero;
        [Tooltip("How many random coords to check before failing")]
        public int numRandomRolls;

        private GridMover _gridMover;
        private Vector2Int _currentCoord;
        private float _lastCoordChangeTime;
        private Vector2Int _previousTarget;
        private Queue<Vector2Int> _path;

        public override void Initialize(Actor actor)
        {
            _gridMover = actor.GetComponent<GridMover>();

            target = GetRandomTarget();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_gridMover)
            {
                target = GetRandomTarget();
            }
        }

        public override bool CheckComplete()
        {
            return _gridMover.Coord == target;
        }

        public override bool CheckFailed()
        {
            if (failAfterCantMoveTime <= 0)
            {
                return false;
            }

            return Time.time - _lastCoordChangeTime > failAfterCantMoveTime;
        }

        public override void PollInput(ref ActorInput input)
        {
            if (target != _previousTarget)
            {
                _path = new Queue<Vector2Int>(Vector2IntPathfinder.GetPath(_gridMover.Coord, target, (Vector2Int coord) =>
                {
                    return GridCollisionManager.CanOccupy(_gridMover.Collider, coord);
                }));
                _previousTarget = target;
            }

            if (_path == null || _path.Count <= 0)
            {
                return;
            }

            if (_path.Peek() == _gridMover.Coord)
            {
                _path.Dequeue();
            }

            if (_path.Count <= 0)
            {
                input.direction.SetDirection(Vector2Int.zero);
                return;
            }

            input.direction.SetDirection(_path.Peek() - _gridMover.Coord);
            Debug.DrawLine(new Vector3(_gridMover.Coord.x, _gridMover.Coord.y), new Vector3(target.x, target.y, 0f), Color.blue);
        }

        private Vector2Int GetRandomTarget()
        {
            Vector2Int minRange = _gridMover.Coord - randomizeRangeOnEnable;
            Vector2Int maxRange = _gridMover.Coord + randomizeRangeOnEnable;
            for (int i = 0; i < numRandomRolls; i++)
            {
                Vector2Int target = new Vector2Int(Random.Range(minRange.x, maxRange.x + 1), Random.Range(minRange.y, maxRange.y + 1));
                if (GridCollisionManager.CanOccupy(_gridMover.Collider, target))
                {
                    return target;
                }
            }
            return _gridMover.Coord;
        }
    }
}