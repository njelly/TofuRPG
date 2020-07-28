using System;
using System.Collections.Generic;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class GridCollisionManager : SingletonBehaviour<GridCollisionManager>
    {
        public static event EventHandler GridRecentered;

        [Tooltip("The gameobject to center the grid around.")]
        public GameObject centeredOn;

        [SerializeField] private Vector2Int _worldSize; // the width and height of the world (in regions), coordinates outside the world are not traversable
        [SerializeField] private Vector2Int _regionSize; // the size of a single region, when the "centeredOn" game object's region changes, we update the layer mask grid

        [Header("Debug")]
        [SerializeField] private bool _drawGridInScene;

        private Vector2Int _regionOffset;
        private List<int>[,] _layerMaskGrid;
        private Vector2Int _totalWorldSize;

        protected override void Awake()
        {
            base.Awake();

            _totalWorldSize = new Vector2Int(_regionSize.x * _worldSize.x, _regionSize.y * _worldSize.y);
            _layerMaskGrid = new List<int>[_totalWorldSize.x, _totalWorldSize.y];
            for (int x = 0; x < _layerMaskGrid.GetLength(0); x++)
            {
                for (int y = 0; y < _layerMaskGrid.GetLength(1); y++)
                {
                    _layerMaskGrid[x, y] = new List<int>();
                }
            }
        }

        private void Update()
        {
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            if (!centeredOn)
            {
                return;
            }

            Vector2Int prevRegionOffset = _regionOffset;
            Vector2Int centeredOnCoord = new Vector2Int(Mathf.RoundToInt(centeredOn.transform.position.x), Mathf.RoundToInt(centeredOn.transform.position.y));
            _regionOffset = new Vector2Int(Mathf.FloorToInt(centeredOnCoord.x / (float)_regionSize.x), Mathf.FloorToInt(centeredOnCoord.y / (float)_regionSize.y)) - (_worldSize / 2);

            if (prevRegionOffset != _regionOffset)
            {
                ResetGrid();
                GridRecentered?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ResetGrid()
        {
            foreach (List<int> layerMaskList in _layerMaskGrid)
            {
                layerMaskList.Clear();
            }
        }

        public static void AddToGrid(GridCollider gc)
        {
            ConvertToVector2IntMinMaxOnGrid(gc, gc.Coord, out Vector2Int min, out Vector2Int max);

            for (int x = min.x; x < max.x; x++)
            {
                if (x < 0 || x >= _instance._totalWorldSize.x)
                {
                    continue;
                }

                for (int y = min.y; y < max.y; y++)
                {
                    if (y < 0 || y >= _instance._totalWorldSize.y)
                    {
                        continue;
                    }

                    _instance._layerMaskGrid[x, y].Add(gc.gameObject.layer);
                }
            }
        }

        public static void RemoveFromGrid(GridCollider gc)
        {
            ConvertToVector2IntMinMaxOnGrid(gc, out Vector2Int min, out Vector2Int max);

            for (int x = min.x; x < max.x; x++)
            {
                if (x < 0 || x >= _instance._totalWorldSize.x)
                {
                    continue;
                }

                for (int y = min.y; y < max.y; y++)
                {
                    if (y < 0 || y >= _instance._totalWorldSize.y)
                    {
                        continue;
                    }

                    _instance._layerMaskGrid[x, y].Remove(gc.gameObject.layer);
                }
            }
        }

        public static void TranslateOnGrid(GridCollider gc, Vector2Int from, Vector2Int to)
        {
            ConvertToVector2IntMinMaxOnGrid(gc, out Vector2Int fromMin, out Vector2Int fromMax);
            Vector2Int toMin = fromMin + (to - from);
            Vector2Int toMax = fromMin + (to - from);

            // remove from previous position
            for (int x = fromMin.x; x < fromMin.x; x++)
            {
                if (x < 0 || x >= _instance._totalWorldSize.x)
                {
                    continue;
                }

                for (int y = fromMin.y; y < fromMin.y; y++)
                {
                    if (y < 0 || y >= _instance._totalWorldSize.y)
                    {
                        continue;
                    }

                    _instance._layerMaskGrid[x, y].Remove(gc.gameObject.layer);
                }
            }

            // add to new position
            for (int x = fromMin.x; x < fromMin.x; x++)
            {
                if (x < 0 || x >= _instance._totalWorldSize.x)
                {
                    continue;
                }

                for (int y = fromMin.y; y < fromMin.y; y++)
                {
                    if (y < 0 || y >= _instance._totalWorldSize.y)
                    {
                        continue;
                    }

                    _instance._layerMaskGrid[x, y].Add(gc.gameObject.layer);
                }
            }
        }

        public static bool TryOccupy(GridCollider gc, Vector2Int newCoord)
        {
            if (!CanOccupy(gc, newCoord))
            {
                return false;
            }
            else
            {
                TranslateOnGrid(gc, gc.Coord, newCoord);
                return true;
            }
        }

        public static bool CanOccupy(GridCollider gc, Vector2Int coord)
        {
            ConvertToVector2IntMinMaxOnGrid(gc, coord, out Vector2Int min, out Vector2Int max);
            Debug.Log($"{gc.gameObject.name} at {coord} converts to min: {min}, max: {max}");
            for (int x = min.x; x <= max.x; x++)
            {
                if (x < 0 || x >= _instance._totalWorldSize.x)
                {
                    // cannot occupy outside the world
                    return false;
                }

                for (int y = min.y; y <= max.y; y++)
                {
                    if (y < 0 || y >= _instance._totalWorldSize.y)
                    {
                        // cannot occupy outside the world
                        return false;
                    }

                    int layerMask = GetLayerMask(x, y);
                    int goLayer = gc.gameObject.layer;

                    if ((layerMask & goLayer) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static int GetLayerMask(Vector2Int coord) => GetLayerMask(coord.x, coord.y);
        public static int GetLayerMask(int x, int y)
        {
            int layerMask = 0;
            foreach (int layer in _instance._layerMaskGrid[x, y])
            {
                layerMask |= layer;
            }

            return layerMask;
        }

        public static void CenterOn(GameObject centeredOn)
        {
            _instance.centeredOn = centeredOn;
            _instance.UpdateGrid();
        }

        public static void ConvertToVector2IntMinMaxOnGrid(GridCollider gc, out Vector2Int min, out Vector2Int max) => ConvertToVector2IntMinMaxOnGrid(gc, gc.Coord, out min, out max);
        public static void ConvertToVector2IntMinMaxOnGrid(GridCollider gc, Vector2Int coord, out Vector2Int min, out Vector2Int max)
        {
            Vector2Int worldCoordOffset = new Vector2Int(_instance._regionSize.x * _instance._regionOffset.x, _instance._regionSize.y * _instance._regionOffset.y);
            Vector2Int adjustedCoord = coord - worldCoordOffset;
            min = adjustedCoord + gc.Offset;
            max = min + gc.Size;
        }

        public void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (_drawGridInScene)
            {
                Vector2 worldOffset = new Vector2(_regionOffset.x * _regionSize.x, _regionOffset.y * _regionSize.y);

                Color prevColor = Gizmos.color;
                Gizmos.color = Color.green;
                Vector2 totalWorldSize = new Vector2(_worldSize.x * _regionSize.x, _worldSize.y * _regionSize.y);
                for (int x = 0; x <= totalWorldSize.x; x++)
                {
                    Vector2 start = new Vector2(x, 0) - (Vector2.one / 2f) + worldOffset;
                    Vector2 end = new Vector2(x, totalWorldSize.y) - (Vector2.one / 2f) + worldOffset;
                    Gizmos.DrawLine(start, end);
                }

                for (int y = 0; y <= totalWorldSize.y; y++)
                {
                    Vector2 start = new Vector2(0, y) - (Vector2.one / 2f) + worldOffset;
                    Vector2 end = new Vector2(totalWorldSize.x, y) - (Vector2.one / 2f) + worldOffset;
                    Gizmos.DrawLine(start, end);
                }
                Gizmos.color = prevColor;

                for (int x = 0; x < _totalWorldSize.x; x++)
                {
                    for (int y = 0; y < _totalWorldSize.y; y++)
                    {
                        UnityEditor.Handles.Label(new Vector2(x, y) + worldOffset, $"{x},{y}: {GetLayerMask(x, y)}");
                    }
                }
            }
#endif
        }
    }
}