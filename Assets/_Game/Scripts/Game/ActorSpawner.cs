using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tofunaut.TofuRPG.Game.AI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tofunaut.TofuRPG.Game
{
    public class ActorModelNameAttribute : Attribute
    {
    
    }
    
    public class ActorSpawner : MonoBehaviour
    {
        [ActorModelName] public string actorModelKey;

        private async void Start()
        {
            while (InGameStateController.ActorConfig == null)
                await Task.Yield();
            
            if(!InGameStateController.ActorConfig.TryGetActorModel(actorModelKey, out var model))
                Debug.LogError($"no ActorModel found for {actorModelKey}");
            else
            {
                var t = transform;
                await Spawn(model, t.position, t.parent);
            }
            
            Destroy(gameObject);
        }

        public static async Task Spawn(ActorModel model, Vector3 position, Transform parent)
        {
            var gameObject = new GameObject();
            var t = gameObject.transform;
            t.position = position;
            t.SetParent(parent, true);

            await gameObject.AddComponent<Actor>().Initialize(model);
        }
    }
}