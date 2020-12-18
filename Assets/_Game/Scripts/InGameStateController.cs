using System;
using JetBrains.Annotations;
using Tofunaut.TofuRPG.Game;
using Tofunaut.TofuUnity;
using UnityEngine.AddressableAssets;

namespace Tofunaut.TofuRPG
{
    public class InGameStateController : AppStateController<InGameStateController>
    {
        public static ActorConfig ActorConfig => _instance ? _instance.IsReady ? _instance._actorConfig : null : null;
        public static Blackboard Blackboard => _instance ? _instance._blackboard : null;

        public AssetReference actorConfigAssetReference;

        private Blackboard _blackboard;
        private ActorConfig _actorConfig;

        protected override void Awake()
        {
            base.Awake();
            
            _blackboard = new Blackboard();
        }

        private async void Start()
        {
            _actorConfig = await Addressables.LoadAssetAsync<ActorConfig>(actorConfigAssetReference).Task;
            IsReady = true;
        }
    }
}