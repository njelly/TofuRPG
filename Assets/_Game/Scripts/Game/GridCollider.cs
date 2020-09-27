﻿using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class GridCollider : MonoBehaviour
    {
        public Vector2Int Coord { get; protected set; }
        public Vector2Int Size => _size;
        public Vector2Int Offset => _offset;

        [Header("Collision")]
        [SerializeField] private Vector2Int _size;
        [SerializeField] private Vector2Int _offset;

        protected virtual void Awake()
        {
            Coord = new Vector2Int(Mathf.CeilToInt(transform.position.x), Mathf.CeilToInt(transform.position.y));
        }

        protected virtual void OnEnable()
        {
            GameContext.GridCollisionManager.Add(this);
        }

        protected virtual void OnDisable()
        {
            if(GameContext.HasInstance)
            {
                GameContext.GridCollisionManager.Remove(this);
            }
        }

        public virtual bool TryMoveTo(Vector2Int newCoord)
        {
            if (GameContext.GridCollisionManager.TryMove(this, Coord, newCoord))
            {
                Coord = newCoord;
                return true;
            }

            return false;
        }

        protected virtual void Update()
        {
            Vector2Int min = Coord + _offset;
            Vector2Int max = Coord + _offset + _size;
            Debug.DrawLine(new Vector3(max.x, max.y) - Vector3.one * 0.5f, new Vector3(max.x, min.y) - Vector3.one * 0.5f);
            Debug.DrawLine(new Vector3(max.x, min.y) - Vector3.one * 0.5f, new Vector3(min.x, min.y) - Vector3.one * 0.5f);
            Debug.DrawLine(new Vector3(min.x, min.y) - Vector3.one * 0.5f, new Vector3(min.x, max.y) - Vector3.one * 0.5f);
            Debug.DrawLine(new Vector3(min.x, max.y) - Vector3.one * 0.5f, new Vector3(max.x, max.y) - Vector3.one * 0.5f);
        }

    }
}