using System;
using JetBrains.Annotations;
using Tofunaut.TofuRPG.Game;
using Tofunaut.TofuUnity;
using UnityEngine.AddressableAssets;

namespace Tofunaut.TofuRPG
{
    public class InGameStateController : AppStateController<InGameStateController>
    {
        public static GameConfig Config => _instance && _instance.IsReady ? _instance._gameConfig : null;
        public static Blackboard Blackboard => _instance ? _instance._blackboard : null;

        public AssetReference gameConfigAssetReference;

        private Blackboard _blackboard;
        private GameConfig _gameConfig;

        protected override void Awake()
        {
            base.Awake();
            
            _blackboard = new Blackboard();
        }

        private async void Start()
        {
            _gameConfig = await Addressables.LoadAssetAsync<GameConfig>(gameConfigAssetReference).Task;
            IsReady = true;
        }
    }
}