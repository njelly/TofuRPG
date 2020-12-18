using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public interface IGridCollider
    {
        int Layer { get; }
        Vector2Int Coord { get; }
        Vector2Int Size { get; }
        Vector2Int Offset { get; }
    }
}