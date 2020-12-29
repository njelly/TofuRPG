
using Tofunaut.TofuRPG.Game;
using Tofunaut.TofuRPG.UI;
using Tofunaut.TofuUnity;
using UnityEngine.AddressableAssets;
using Task = System.Threading.Tasks.Task;

namespace Tofunaut.TofuRPG
{
    public class InGameStateController : AppStateController<InGameStateController>
    {
        public static GameConfig Config => _instance && _instance.IsReady ? _instance._gameConfig : null;
        public static Blackboard Blackboard => _instance ? _instance._blackboard : null;

        public LoadingScreenView loadingScreenView;
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
            ViewControllerStack.Push(loadingScreenView);
            loadingScreenView.SetPercentComplete(0f);
            
            _gameConfig = await Addressables.LoadAssetAsync<GameConfig>(gameConfigAssetReference).Task;
            loadingScreenView.SetPercentComplete(0.5f);
            
            await Task.Delay(1000);
            
            ViewControllerStack.Pop(loadingScreenView);
            
            IsReady = true;
        }
    }
}