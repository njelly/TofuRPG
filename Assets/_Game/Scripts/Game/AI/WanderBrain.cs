using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuUnity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tofunaut.TofuRPG.Game.AI
{
    [CreateAssetMenu(fileName = "new WanderBrain", menuName = "TofuRPG/AI/WanderBrain")]
    public class WanderBrain : NPCBrain
    {
        public enum State
        {
            Waiting,
            Walking,
        }
        
        public float minWaitTime;
        public float maxWaitTime;
        public int range;
        public float repathWhenBlockedTimer;

        private Vector2Int _center;
        private bool _isWaiting;
        private float _blockedTimer;
        private State _state;
        private float _waitTimer;
        
        public override void Initialize(GameObject actor)
        {
            base.Initialize(actor);

            _center = _collider.Coord;
            _state = State.Waiting;
        }

        public override async void Update()
        {
            base.Update();

            switch (_state)
            {
                case State.Waiting:
                    UpdateWaiting();
                    break;
                case State.Walking:
                    UpdateWalking();
                    break;
            }
        }

        private void UpdateWaiting()
        {
            _waitTimer -= Time.deltaTime;

            if (_waitTimer > 0f)
                return;

            Path = ChoosePath();
            _state = State.Walking;
            _blockedTimer = 0f;
        }

        private void UpdateWalking()
        {
            if (HasPath)
            {
                var gridMover = (ActorGridMover) _collider;
                if (gridMover && !gridMover.IsMoving)
                    _blockedTimer += Time.deltaTime;

                if (_blockedTimer > repathWhenBlockedTimer)
                {
                    _blockedTimer = 0f;
                    Path.Clear();
                }
            }
            else
            {
                _waitTimer = Random.Range(minWaitTime, maxWaitTime);
                _state = State.Waiting;
            }
        }

        protected override Queue<Vector2Int> ChoosePath()
        {
            var target = _collider.Coord;
            var attempts = 0;
            const int maxAttempts = 10;
            for (; attempts < maxAttempts; attempts++)
            {
                target = _center + (Random.insideUnitCircle * range).RoundToVector2Int();
                if (GridCollisionManager.CanOccupy(_collider, target))
                    break;
            }

            if (attempts >= maxAttempts)
                return Path;

            Vector2Int[] pathArray;
            try
            {
                pathArray = Vector2IntPathfinder.GetPath(_collider.Coord, target,
                    coord => GridCollisionManager.CanOccupy(_collider, coord), 999);
                
                // ensure the length of the path is never beyond our range (ex: we tried to path to the other side of a very long wall)
                if (pathArray.Length > range)
                    pathArray = new ArraySegment<Vector2Int>(pathArray, 0, range).ToArray();
            }
            catch
            {
                return new Queue<Vector2Int>();
            }

            var toReturn = new Queue<Vector2Int>();
            foreach (var pathPoint in pathArray)
                toReturn.Enqueue(pathPoint);

            return toReturn;
        }

        protected override IInteractable ChooseTargetInteractable()
        {
            return null;
        }

        public void OnValidate()
        {
            minWaitTime = Mathf.Max(minWaitTime, 0f);
            maxWaitTime = Mathf.Clamp(maxWaitTime, minWaitTime, float.MaxValue);
            range = Mathf.Max(range, 0);
        }
    }
}