using System;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class GridCollider : MonoBehaviour
    {
        public Vector2Int Coord => _coord;
        public Vector2Int Size => _size;
        public Vector2Int Offset => _offset;

        [Header("Collision")]
        [SerializeField] private Vector2Int _size;
        [SerializeField] private Vector2Int _offset;

        protected Vector2Int _coord;

        protected virtual void Awake()
        {
            _coord = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

            GridCollisionManager.GridRecentered += OnGridRecentered;
        }

        private void OnEnable()
        {
            GridCollisionManager.AddToGrid(this);
        }

        private void OnDisable()
        {
            GridCollisionManager.RemoveFromGrid(this);
        }

        protected virtual void OnGridRecentered(object sender, EventArgs e)
        {
            GridCollisionManager.AddToGrid(this);
        }

        public void OnDrawGizmos()
        {
            Vector2Int min = _coord + _offset;
            Vector2Int max = _coord + _offset + _size;
            Gizmos.DrawLine(new Vector3(max.x, max.y) - Vector3.one * 0.5f, new Vector3(max.x, min.y) - Vector3.one * 0.5f);
            Gizmos.DrawLine(new Vector3(max.x, min.y) - Vector3.one * 0.5f, new Vector3(min.x, min.y) - Vector3.one * 0.5f);
            Gizmos.DrawLine(new Vector3(min.x, min.y) - Vector3.one * 0.5f, new Vector3(min.x, max.y) - Vector3.one * 0.5f);
            Gizmos.DrawLine(new Vector3(min.x, max.y) - Vector3.one * 0.5f, new Vector3(max.x, max.y) - Vector3.one * 0.5f);
        }
    }
}