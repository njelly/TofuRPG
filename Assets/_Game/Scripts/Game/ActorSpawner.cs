using System;
using System.Linq;
using System.Threading.Tasks;
using Tofunaut.TofuUnity;
using UnityEditor;

#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.AddressableAssets;

#endif

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

        private string _prevActorModelKey;
        private SpriteRenderer _spriteRenderer;
        private GameConfig _loadedGameConfig;

        private async void Start()
        {
            while (InGameStateController.Config == null)
                await Task.Yield();
            
            if(!InGameStateController.Config.TryGetActorModel(actorModelKey, out var model))
                Debug.LogError($"no ActorModel found for {actorModelKey}");
            else
            {
                var t = transform;
                await Spawn(model, initialXp, t.position, t.parent);
            }
            
            Destroy(gameObject);
        }

        public static async Task Spawn(ActorModel model, int initialXp, Vector3 position, Transform parent)
        {
            var gameObject = new GameObject();
            var t = gameObject.transform;
            t.position = position;
            t.SetParent(parent, true);

            await gameObject.AddComponent<Actor>().Initialize(model, initialXp);
        }

#if UNITY_EDITOR
        /*
        public void Update()
        {
            // automatically create a sprite renderer using the sprite from the view this will spawn
            if (!actorModelKey.Equals(_prevActorModelKey))
                SetSprite();
        }

        private async void SetSprite()
        {
            if (!_spriteRenderer)
            {
                _spriteRenderer = gameObject.RequireComponent<SpriteRenderer>();
                _spriteRenderer.sortingLayerName = "Actor";
                var orderInLayerByY = gameObject.RequireComponent<SpriteOrderInLayerByY>();
                orderInLayerByY.spriteRenderer = _spriteRenderer;
                orderInLayerByY.invert = true;
            }

            if (!_loadedGameConfig)
            {
                var guid = AssetDatabase.FindAssets("t:GameConfig").FirstOrDefault();
                if(string.IsNullOrEmpty(guid))
                    Debug.LogError("no GameConfig exists in the project");

                _loadedGameConfig = Instantiate(AssetDatabase.LoadAssetAtPath<GameConfig>(AssetDatabase.GUIDToAssetPath(guid)));
            }
            
            if (!_loadedGameConfig.TryGetActorModel(actorModelKey, out var actorModel))
                Debug.LogError($"no actor model found for {actorModelKey}");

            _prevActorModelKey = actorModelKey;
            
            // if there's no view asset, just return
            if (string.IsNullOrEmpty(actorModel.ViewAsset))
                return;
            
            var viewPrefab = await Addressables.LoadAssetAsync<GameObject>(actorModel.ViewAsset).Task;
            var spriteRendPrefab = viewPrefab.GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = spriteRendPrefab.sprite;
        }
        */
#endif
    }
}