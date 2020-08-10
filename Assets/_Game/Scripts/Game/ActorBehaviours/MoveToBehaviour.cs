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
        public Vector2Int randomizeRangeOnEnable = Vector2Int.zero;
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
                    return GridCollisionManager.CanOccupy(_gridMover, coord);
                }));
                _previousTarget = target;
            }

            if (_path == null || _path.Count <= 0)
            {
                return;
            }
            Debug.Log("poll 0");

            if (_path.Peek() == _gridMover.Coord)
            {
                _path.Dequeue();
            }

            if (_path.Count <= 0)
            {
                input.direction.SetDirection(Vector2Int.zero);
                return;
            }
            Debug.Log("poll 1");

            input.direction.SetDirection(_path.Peek() - _gridMover.Coord);
        }

        private Vector2Int GetRandomTarget()
        {
            for (int i = 0; i < numRandomRolls; i++)
            {
                Vector2Int coord = _gridMover.Coord + new Vector2Int(Random.Range(-randomizeRangeOnEnable.x, randomizeRangeOnEnable.x), Random.Range(-randomizeRangeOnEnable.y, randomizeRangeOnEnable.y));
                if (GridCollisionManager.CanOccupy(_gridMover, coord))
                {
                    Debug.Log($"returning {coord}");
                    return coord;
                }
            }
            return _gridMover.Coord;
        }
    }
}