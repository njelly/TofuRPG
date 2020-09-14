using System.Collections.Generic;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class GridCollisionManager : MonoBehaviour, IGridCollisionManager
    {
        public Vector2Int size;
        public Vector2Int recenterInterval;
        public GridCollider centeredOn;

        private List<GridCollider> _gridColliders = new List<GridCollider>();
        private Vector2IntQuadTree<GridCollider> _quadTree;
        private Vector2Int _offset;

        private void Awake()
        {
            _offset = Vector2Int.zero;
            _quadTree = new Vector2IntQuadTree<GridCollider>(size / -2, size / 2);
        }

        private void Update()
        {
            Vector2Int newOffset = Vector2Int.zero;
            if (centeredOn)
            {
                Vector2Int coord = centeredOn.Coord;
                newOffset = new Vector2Int(Mathf.CeilToInt(coord.x / (float)recenterInterval.x) * recenterInterval.x, Mathf.CeilToInt(coord.y / (float)recenterInterval.y) * recenterInterval.y);
            }

            if (newOffset != _offset)
            {
                _offset = newOffset;
                _quadTree.Clear();
                foreach (GridCollider gc in _gridColliders)
                {
                    Add(gc);
                }
            }

            RenderQuadTree(_quadTree);
        }

        private void RecenterQuadTree()
        {
            _quadTree.Clear();
        }

        public void Add(GridCollider gc)
        {
            _gridColliders.Add(gc);

            Vector2Int min = new Vector2Int(int.MinValue, int.MinValue);
            Vector2Int max = new Vector2Int(int.MaxValue, int.MaxValue);

            GetMinMax(gc, gc.Coord, out Vector2Int gcMin, out Vector2Int gcMax);
            for (int x = gcMin.x; x < gcMax.x; x++)
            {
                for (int y = gcMin.y; y < gcMax.y; y++)
                {
                    Vector2Int adjustedCoord = new Vector2Int(x, y) - _offset;
                    if (adjustedCoord.x < min.x || adjustedCoord.x >= max.x || adjustedCoord.y < min.y || adjustedCoord.y >= max.y)
                    {
                        continue;
                    }

                    _quadTree.Add(gc, adjustedCoord);
                }
            }
        }

        private static void GetMinMax(GridCollider gridCollider, Vector2Int at, out Vector2Int min, out Vector2Int max)
        {
            min = at + gridCollider.Offset;
            max = min + gridCollider.Size;
        }

        public void Remove(GridCollider gc)
        {
            _gridColliders.Remove(gc);

            Vector2Int min = size / -2;
            Vector2Int max = size / 2;
            GetMinMax(gc, gc.Coord, out Vector2Int gcMin, out Vector2Int gcMax);
            for (int x = gcMin.x; x < gcMax.x; x++)
            {
                for (int y = gcMin.y; y < gcMax.y; y++)
                {
                    Vector2Int adjustedCoord = new Vector2Int(x, y) - _offset;
                    if (adjustedCoord.x < min.x || adjustedCoord.x >= max.x || adjustedCoord.y < min.y || adjustedCoord.y >= max.y)
                    {
                        continue;
                    }

                    _quadTree.Remove(gc, adjustedCoord);
                }
            };
        }

        public bool TryMove(GridCollider gc, Vector2Int from, Vector2Int to)
        {
            if (!CanOccupy(gc, to))
            {
                return false;
            }

            Remove(gc);
            Add(gc);

            return true;
        }

        public bool CanOccupy(GridCollider gc, Vector2Int coord)
        {
            Vector2Int min = size / -2;
            Vector2Int max = size / 2;
            GetMinMax(gc, coord, out Vector2Int gcMin, out Vector2Int gcMax);
            for (int x = gcMin.x; x < gcMax.x; x++)
            {
                for (int y = gcMin.y; y < gcMax.y; y++)
                {
                    int layerMask = 0;
                    Vector2Int adjustedCoord = new Vector2Int(x, y) - _offset;
                    if (adjustedCoord.x < min.x || adjustedCoord.x >= max.x || adjustedCoord.y < min.y || adjustedCoord.y >= max.y)
                    {
                        return false;
                    }

                    if (_quadTree.TryGet(adjustedCoord, out List<GridCollider> gridColliders))
                    {
                        foreach (GridCollider collider in gridColliders)
                        {
                            if (collider == gc)
                            {
                                // ignore self
                                continue;
                            }

                            layerMask |= collider.gameObject.layer;
                        }
                    }

                    if ((gc.gameObject.layer & layerMask) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public List<GridCollider> GetCollidersAt(Vector2Int coord)
        {
            Vector2Int adjustedCoord = coord - _offset;
            List<GridCollider> toReturn = new List<GridCollider>();
            if (_quadTree.TryGet(adjustedCoord, out List<GridCollider> colliders))
            {
                toReturn.AddRange(colliders);
            }
            return toReturn;
        }

        //public static Vector2Int ConvertToGridPosition(Vector3 position) => ConvertToGridPosition(new Vector3(position.x, position.y));
        //public static Vector2Int ConvertToGridPosition(Vector2 position)
        //{
        //    return new Vector2Int(Mathf.CeilToInt(position.x), Mathf.CeilToInt(position.y));
        //}

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