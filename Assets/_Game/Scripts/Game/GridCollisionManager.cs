using System;
using System.Collections.Generic;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class GridCollisionManager : SingletonBehaviour<GridCollisionManager>
    {
        public Vector2Int size;
        public Vector2Int recenterInterval;
        public GridCollider centeredOn;

        private Vector2IntQuadTree<GridCollider> _quadTree;
        private List<GridCollider> _gridColliders;
        private Vector2Int _offset;

        protected override void Awake()
        {
            base.Awake();

            _gridColliders = new List<GridCollider>();
            _offset = Vector2Int.zero;
            _quadTree = new Vector2IntQuadTree<GridCollider>(size / -2, size / 2);
        }

        private void Update()
        {
            Vector2Int newOffset = Vector2Int.zero;
            if (centeredOn)
            {
                Vector2Int coord = centeredOn.Coord;
                newOffset = new Vector2Int(coord.x - (coord.x % recenterInterval.x), coord.y - (coord.y % recenterInterval.y));
            }

            if (newOffset != _offset)
            {
                _quadTree.Clear();
                foreach (GridCollider gc in _gridColliders)
                {
                    Vector2Int adjustedCoord = gc.Coord - _offset;
                    if (adjustedCoord.x < (size.x / -2) || adjustedCoord.x >= (size.x / 2) || adjustedCoord.y < (size.y / -2) || adjustedCoord.y >= (size.y / 2))
                    {
                        continue;
                    }

                    _quadTree.Add(gc, adjustedCoord);
                }
            }

            RenderQuadTree(_quadTree);
        }

        private void RecenterQuadTree()
        {
            _quadTree.Clear();
        }

        public static void Add(GridCollider gc)
        {
            Debug.Log($"trying to add {gc.gameObject.name}");
            if (!_instance)
            {
                Debug.Log("no instance");
                return;
            }

            _instance._gridColliders.Add(gc);
            Vector2Int adjustedCoord = gc.Coord - _instance._offset;
            if (adjustedCoord.x < (_instance.size.x / -2) || adjustedCoord.x >= (_instance.size.x / 2) || adjustedCoord.y < (_instance.size.y / -2) || adjustedCoord.y >= (_instance.size.y / 2))
            {
                Debug.Log("out of bounds");
                return;
            }

            Debug.Log($"add {gc.name} to {adjustedCoord}");
            _instance._quadTree.Add(gc, adjustedCoord);
        }

        public static void Remove(GridCollider gc)
        {
            if (!_instance)
            {
                return;
            }

            _instance._gridColliders.Remove(gc);
            Vector2Int adjustedCoord = gc.Coord - _instance._offset;
            if (adjustedCoord.x < (_instance.size.x / -2) || adjustedCoord.x >= (_instance.size.x / 2) || adjustedCoord.y < (_instance.size.y / -2) || adjustedCoord.y >= (_instance.size.y / 2))
            {
                return;
            }

            _instance._quadTree.Remove(gc, adjustedCoord);
        }

        public static bool TryMove(GridCollider gc, Vector2Int from, Vector2Int to)
        {
            if (!CanOccupy(gc, to))
            {
                return false;
            }

            _instance._quadTree.Remove(gc, from);
            _instance._quadTree.Add(gc, to);

            return true;
        }

        public static bool CanOccupy(GridCollider gc, Vector2Int coord)
        {
            int layerMask = 0;

            if (_instance._quadTree.TryGet(coord, out List<GridCollider> gridColliders))
            {
                foreach (GridCollider collider in gridColliders)
                {
                    layerMask |= gc.gameObject.layer;
                }
            }

            return (gc.gameObject.layer & layerMask) == 0;
        }

        private void RenderQuadTree<T>(Vector2IntQuadTree<T> tree)
        {
            Debug.DrawLine(new Vector2(tree.Min.x, tree.Max.y) + _offset + Vector2.one * 0.5f, new Vector2(tree.Max.x, tree.Max.y) + _offset + Vector2.one * 0.5f, tree.Depth % 2 == 0 ? Color.red : Color.green);
            Debug.DrawLine(new Vector2(tree.Max.x, tree.Max.y) + _offset + Vector2.one * 0.5f, new Vector2(tree.Max.x, tree.Min.y) + _offset + Vector2.one * 0.5f, tree.Depth % 2 == 0 ? Color.red : Color.green);

            if (tree.Quadrants != null)
            {
                for (int i = 0; i < tree.Quadrants.Length; i++)
                {
                    if (tree.Quadrants[i] != null)
                    {
                        RenderQuadTree(tree.Quadrants[i]);
                    }
                }
            }
        }

        private void OnValidate()
        {
            size.x = Mathf.Max(2, size.x);
            size.y = Mathf.Max(2, size.y);
            recenterInterval.x = Mathf.Max(1, recenterInterval.x);
            recenterInterval.y = Mathf.Max(1, recenterInterval.y);
        }
    }
}