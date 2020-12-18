using Tofunaut.TofuRPG.Game.Interfaces;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class ActorGridCollider : ActorComponent, ICoordProvider, IGridCollider
    {
        public int Layer => gameObject.layer;
        public Vector2Int Coord { get; protected set; }
        public Vector2Int Size => _size;
        public Vector2Int Offset => _offset;

        private Vector2Int _size;
        private Vector2Int _offset;

        protected virtual void Awake()
        {
            var position = transform.position;
            Coord = new Vector2Int(Mathf.CeilToInt(position.x), Mathf.CeilToInt(position.y));
        }

        protected virtual void OnDisable()
        {
            GridCollisionManager.Remove(this);
        }

        protected virtual void Update()
        {
            var min = Coord + _offset;
            var max = Coord + _offset + _size;
            Debug.DrawLine(new Vector3(max.x, max.y) - Vector3.one * 0.5f, new Vector3(max.x, min.y) - Vector3.one * 0.5f);
            Debug.DrawLine(new Vector3(max.x, min.y) - Vector3.one * 0.5f, new Vector3(min.x, min.y) - Vector3.one * 0.5f);
            Debug.DrawLine(new Vector3(min.x, min.y) - Vector3.one * 0.5f, new Vector3(min.x, max.y) - Vector3.one * 0.5f);
            Debug.DrawLine(new Vector3(min.x, max.y) - Vector3.one * 0.5f, new Vector3(max.x, max.y) - Vector3.one * 0.5f);
        }

        public override async void Initialize(Actor actor, ActorModel model)
        {
            _size = model.ColliderSize;
            _offset = model.ColliderOffset;
            await GridCollisionManager.Add(this);
        }

        public virtual bool TryMoveTo(Vector2Int newCoord)
        {
            if (!GridCollisionManager.TryMove(this, Coord, newCoord)) 
                return false;
            
            Coord = newCoord;
            return true;
        }

    }
}