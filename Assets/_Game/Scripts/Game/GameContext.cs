using System.Linq;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class GameContext : SingletonBehaviour<GameContext>
    {
        public static IGridCollisionManager GridCollisionManager => _instance._gridCollisionManager;
        public static IGameCameraController GameCameraController => _instance._gameCameraController;

        private IGridCollisionManager _gridCollisionManager;
        private IGameCameraController _gameCameraController;

        protected override void Awake()
        {
            base.Awake();

            _gridCollisionManager = GetComponentsInChildren<MonoBehaviour>().OfType<IGridCollisionManager>().First();
            _gameCameraController = GetComponentsInChildren<MonoBehaviour>().OfType<IGameCameraController>().First();
        }
    }
}
