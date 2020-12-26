using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tofunaut.TofuRPG.Game
{
    public class ActorModelNameAttribute : Attribute
    {
    
    }
    
    [ExecuteInEditMode]
    public class ActorSpawner : MonoBehaviour
    {
        [ActorModelName] public string actorModelKey;
        public int initialXp;
        public AssetReference actorViewOverride;

        private string _prevActorModelKey;
        private SpriteRenderer _spriteRenderer;
        private GameConfig _gameConfig;

        private async void Start()
        {
            while (InGameStateController.Config == null)
                await Task.Yield();
            
            if(!InGameStateController.Config.TryGetActorModel(actorModelKey, out var model))
                Debug.LogError($"no ActorModel found for {actorModelKey}");
            else
            {
                var t = transform;
                await Spawn(model, initialXp, actorViewOverride, t.position, t.parent);
            }
            
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "actor_spawner", true);
        }

        public static async Task Spawn(ActorModel model, int initialXp, AssetReference actorViewOverride, Vector3 position, Transform parent)
        {
            var gameObject = new GameObject();
            var t = gameObject.transform;
            t.position = position;
            t.SetParent(parent, true);

            await gameObject.AddComponent<Actor>().Initialize(model, initialXp, actorViewOverride);
        }
    }
}