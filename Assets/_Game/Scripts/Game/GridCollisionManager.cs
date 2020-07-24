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
            _regionOffset = new Vector2Int(centeredOnCoord.x / _regionSize.x, centeredOnCoord.y / _regionSize.y);

            if (prevRegionOffset != _regionOffset)
            {
                ResetGrid();
                GridRecentered?.Invoke(this, EventArgs.Empty);
            }
        }

        public static void AddToGrid(GridCollider gc)
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

                    _instance._layerMaskGrid[x, y].Remove(gc.gameObject.layer);
                }
            }
        }

        public static bool CanOccupy(GridCollider gc, Vector2Int coord) => CanOccupy(gc, coord, Vector2Int.zero);
        public static bool CanOccupy(GridCollider gc, Vector2Int coord, Vector2Int offsetFromCoord)
        {
            if (coord.x < 0 || coord.x >= _instance._totalWorldSize.x || coord.y < 0 || coord.y > _instance._totalWorldSize.y)
            {
                // cannot occupy outside world
                return false;
            }

            Vector2Int internalRegionCoord = new Vector2Int(coord.x % _instance._regionSize.x, coord.y % _instance._regionSize.y);
            Vector2Int worldOffset = new Vector2Int(
                internalRegionCoord.x + (_instance._regionSize.x * _instance._regionOffset.x),
                internalRegionCoord.y + (_instance._regionSize.y * _instance._regionOffset.y));
            Vector2Int adjustedCoord = coord - worldOffset;

            ConvertToVector2IntMinMaxOnGrid(gc, out Vector2Int min, out Vector2Int max);
            min += offsetFromCoord;
            max += offsetFromCoord;
            for (int x = min.x; x <= max.x; x++)
            {
                if (x < 0 || x >= _instance._totalWorldSize.x)
                {
                    return false;
                }

                for (int y = min.y; y <= max.y; y++)
                {
                    if (y < 0 || y >= _instance._totalWorldSize.y)
                    {
                        return false;
                    }

                    if ((GetLayerMask(x, y) & gc.gameObject.layer) != 0)
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

        private void ResetGrid()
        {
            foreach (List<int> layerMaskList in _layerMaskGrid)
            {
                layerMaskList.Clear();
            }
        }

        public static void CenterOn(GameObject centeredOn)
        {
            _instance.centeredOn = centeredOn;
            _instance.UpdateGrid();
        }

        public static void ConvertToVector2IntMinMaxOnGrid(GridCollider gc, out Vector2Int min, out Vector2Int max)
        {
            Vector2Int internalRegionCoord = new Vector2Int(gc.Coord.x % _instance._regionSize.x, gc.Coord.y % _instance._regionSize.y);
            Vector2Int worldOffset = new Vector2Int(
                internalRegionCoord.x + (_instance._regionSize.x * _instance._regionOffset.x),
                internalRegionCoord.y + (_instance._regionSize.y * _instance._regionOffset.y));

            min = gc.Coord - worldOffset;
            max = min + gc.Size;
        }
    }
}