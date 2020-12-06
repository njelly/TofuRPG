using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class GridCollisionManager : SingletonBehaviour<GridCollisionManager>
    {
        [SerializeField] private Vector2Int _size;
        [SerializeField] private Vector2Int _recenterInterval;
        public GridCollider centeredOn;

    #if UNITY_EDITOR
        [Header("Develop")] 
        public bool renderQuadTree;
    #endif

        private List<GridCollider> _gridColliders = new List<GridCollider>();
        private Vector2IntQuadTree<GridCollider> _quadTree;
        private Vector2Int _offset;

        protected override void Awake()
        {
            base.Awake();
            
            _offset = Vector2Int.zero;
            _quadTree = new Vector2IntQuadTree<GridCollider>(_size / -2, _size / 2);
        }

        private async void Update()
        {
            var newOffset = Vector2Int.zero;
            if (centeredOn)
            {
                var coord = centeredOn.Coord;
                newOffset = new Vector2Int(Mathf.CeilToInt(coord.x / (float)_recenterInterval.x) * _recenterInterval.x, 
                    Mathf.CeilToInt(coord.y / (float)_recenterInterval.y) * _recenterInterval.y);
            }

            if (newOffset != _offset)
            {
                _offset = newOffset;
                _quadTree.Clear();
                foreach (var gc in _gridColliders)
                    await Add(gc);
            }

#if UNITY_EDITOR
            if(renderQuadTree)
                RenderQuadTree(_quadTree);
#endif
        }

        public static async Task Add(GridCollider gc)
        {
            while (!_instance)
                await Task.Yield();
            
            _instance._gridColliders.Add(gc);

            var min = new Vector2Int(int.MinValue, int.MinValue);
            var max = new Vector2Int(int.MaxValue, int.MaxValue);

            GetMinMax(gc, gc.Coord, out var gcMin, out var gcMax);
            for (var x = gcMin.x; x < gcMax.x; x++)
                for (var y = gcMin.y; y < gcMax.y; y++)
                {
                    var adjustedCoord = new Vector2Int(x, y) - _instance._offset;
                    if (adjustedCoord.x < min.x || adjustedCoord.x >= max.x || adjustedCoord.y < min.y || adjustedCoord.y >= max.y)
                        continue;

                    _instance._quadTree.Add(gc, adjustedCoord);
                }
        }

        private static void GetMinMax(GridCollider gridCollider, Vector2Int at, out Vector2Int min, out Vector2Int max)
        {
            min = at + gridCollider.Offset;
            max = min + gridCollider.Size;
        }

        public static void Remove(GridCollider gc)
        {
            if (!_instance)
                return;
            
            _instance._gridColliders.Remove(gc);

            var min = _instance._size / -2;
            var max = _instance._size / 2;
            GetMinMax(gc, gc.Coord, out Vector2Int gcMin, out Vector2Int gcMax);
            for (var x = gcMin.x; x < gcMax.x; x++)
                for (var y = gcMin.y; y < gcMax.y; y++)
                {
                    var adjustedCoord = new Vector2Int(x, y) - _instance._offset;
                    if (adjustedCoord.x < min.x || adjustedCoord.x >= max.x || adjustedCoord.y < min.y || adjustedCoord.y >= max.y)
                        continue;

                    _instance._quadTree.Remove(gc, adjustedCoord);
                }
        }

        public static bool TryMove(GridCollider gc, Vector2Int from, Vector2Int to)
        {
            if (!CanOccupy(gc, to))
                return false;

            _instance._quadTree.Translate(gc, from, to);

            return true;
        }

        public static bool CanOccupy(GridCollider gc, Vector2Int coord)
        {
            if (!_instance)
                return false;
            
            var min = _instance._size / -2;
            var max = _instance._size / 2;
            GetMinMax(gc, coord, out var gcMin, out var gcMax);
            for (var x = gcMin.x; x < gcMax.x; x++)
                for (var y = gcMin.y; y < gcMax.y; y++)
                {
                    var layerMask = 0;
                    var adjustedCoord = new Vector2Int(x, y) - _instance._offset;
                    if (adjustedCoord.x < min.x || adjustedCoord.x >= max.x || adjustedCoord.y < min.y || adjustedCoord.y >= max.y)
                        return false;

                    if (_instance._quadTree.TryGet(adjustedCoord, out var gridColliders))
                        foreach (var collider in gridColliders)
                        {
                            if (collider == gc)
                                // ignore self
                                continue;

                            layerMask |= collider.gameObject.layer;
                        }
                    
                    if ((gc.gameObject.layer & layerMask) != 0)
                        return false;
                }

            return true;
        }

        public static GridCollider[] GetCollidersAt(Vector2Int coord)
        {
            var adjustedCoord = coord - _instance._offset;
            var toReturn = new List<GridCollider>();
            if (_instance._quadTree.TryGet(adjustedCoord, out List<GridCollider> colliders))
                toReturn.AddRange(colliders);
            
            return toReturn.ToArray();
        }

        public static void CenterOn(GridCollider gridCollider)
        {
            _instance.centeredOn = gridCollider;
        }

        private void RenderQuadTree<T>(Vector2IntQuadTree<T> tree)
        {
            Debug.DrawLine(new Vector2(tree.Min.x, tree.Max.y) + _offset + Vector2.one * 0.5f, new Vector2(tree.Max.x, tree.Max.y) + _offset + Vector2.one * 0.5f, tree.Depth % 2 == 0 ? Color.red : Color.green);
            Debug.DrawLine(new Vector2(tree.Max.x, tree.Max.y) + _offset + Vector2.one * 0.5f, new Vector2(tree.Max.x, tree.Min.y) + _offset + Vector2.one * 0.5f, tree.Depth % 2 == 0 ? Color.red : Color.green);

            if (tree.Quadrants == null) 
                return;
            
            foreach (var t in tree.Quadrants)
                if (t != null)
                    RenderQuadTree(t);
        }

        private void OnValidate()
        {
            _size.x = Mathf.Max(2, _size.x);
            _size.y = Mathf.Max(2, _size.y);
            _recenterInterval.x = Mathf.Max(1, _recenterInterval.x);
            _recenterInterval.y = Mathf.Max(1, _recenterInterval.y);
        }
    }
}