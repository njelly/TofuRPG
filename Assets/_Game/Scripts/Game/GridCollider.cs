using Tofunaut.TofuRPG.Game;
using UnityEngine;

namespace Tofunaut.TofuRPG.Scripts.Game
{
    public class GridCollider : MonoBehaviour, IGridCollider
    {
        public int Layer => gameObject.layer;
        public Vector2Int Size => _size;
        public Vector2Int Coord { get; private set; }
        public Vector2Int Offset => _offset;

        [SerializeField] private Vector2Int _size;
        [SerializeField] private Vector2Int _offset;

        public async void Start()
        {
            var position = transform.position;
            Coord = new Vector2Int(Mathf.CeilToInt(position.x), Mathf.CeilToInt(position.y));
            await GridCollisionManager.Add(this);
        }
    }
}