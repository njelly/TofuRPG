using System.Collections.Generic;
using System.Linq;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game.AI
{
    public abstract class NPCBrain : ScriptableObject
    {
        public bool HasPath => Path.Count > 0;

        public Queue<Vector2Int> Path { get; protected set; }
        public IInteractable TargetInteractable { get; protected set; }

        protected GridCollider _collider;

        private void Awake()
        {
            Path = new Queue<Vector2Int>();
        }

        public virtual void Initialize(GameObject actor)
        {
            var components = actor.GetComponents<MonoBehaviour>();
            _collider = components.OfType<GridCollider>().FirstOrDefault();
            Path = new Queue<Vector2Int>();
        }

        public virtual void Update()
        {
            UpdatePath();
            
            #if UNITY_EDITOR
            DrawPath();
            #endif
        }

        private void UpdatePath()
        {
            if (!HasPath)
                return;

            if (Path.Peek() == _collider.Coord)
                Path.Dequeue();
        }

        private void DrawPath()
        {
            if (!HasPath)
                return;

            var prevPos = _collider.Coord;
            foreach (var pos in Path)
            {
                Debug.DrawLine(prevPos.ToVector2(), pos.ToVector2(), Color.red);
                prevPos = pos;
            }
            
            Debug.DrawLine(prevPos.ToVector2(), _collider.Coord.ToVector2(), Color.yellow);
        }

        protected abstract Queue<Vector2Int> ChoosePath();
        protected abstract IInteractable ChooseTargetInteractable();
    }
}