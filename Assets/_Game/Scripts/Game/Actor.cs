using System.Threading.Tasks;
using Tofunaut.TofuRPG.Game.AI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tofunaut.TofuRPG.Game
{
    public class Actor : MonoBehaviour
    {
        private static GameObject _actorViewPrefab;
        private static bool _isLoading;
        private static uint _idCounter;
        
        public uint Id { get; private set; }
        
        public async Task Initialize(ActorModel model)
        {
            Id = ++ _idCounter;
            gameObject.name = $"{model.Name}_{Id}";
            gameObject.layer = LayerMask.NameToLayer("Actor");
            
            // now add the components and initialize them

            switch (model.ActorInputSource)
            {
                case ActorModel.EActorInputSource.Player:
                    gameObject.AddComponent<PlayerActorInputProvider>().Initialize(this, model);
                    break;
                case ActorModel.EActorInputSource.AI:
                    gameObject.AddComponent<NPCActorInputProvider>().Initialize(this, model);
                    break;
            }
            
            if(model.ColliderSize.magnitude > 1)
                if(model.MoveSpeed > 0)
                    gameObject.AddComponent<ActorGridMover>().Initialize(this, model);
                else
                    gameObject.AddComponent<ActorGridCollider>().Initialize(this, model);

            gameObject.AddComponent<Interactor>().Initialize(this, model);
            
            // instantiate the view last, return if there is no view
            if (string.IsNullOrEmpty(model.ViewAsset))
                return;
            
            var viewPrefab = await Addressables.LoadAssetAsync<GameObject>(model.ViewAsset).Task;
            Instantiate(viewPrefab, transform, false);
        }
    }
}