using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public interface IGridCollisionManager
    {
        void Add(GridCollider gridCollider);
        void Remove(GridCollider gridCollider);
        bool TryMove(GridCollider gridCollider, Vector2Int from, Vector2Int to);
        bool CanOccupy(GridCollider gridCollider, Vector2Int coord);
        void CenterOn(GridCollider gridCollider);
        GridCollider[] GetCollidersAt(Vector2Int coord);
    }
}