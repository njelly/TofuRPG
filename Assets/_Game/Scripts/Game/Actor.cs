using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tofunaut.TofuRPG.Game.AI;
using Tofunaut.TofuUnity;
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
        public int Xp { get; private set; }

        public float Agility => _baseAgility;
        public float Charisma => _baseCharisma;
        public float Intelligence => _baseIntelligence;
        public float Strength => _baseStrength;

        private Dictionary<Type, ActorComponent> _typeToActorComponent;
        private float _baseAgility;
        private float _baseCharisma;
        private float _baseIntelligence;
        private float _baseStrength;
        private BoxCollider2D _unityCollider;

        public async Task Initialize(ActorModel model, int xp, AssetReference viewOverride)
        {
            Id = ++ _idCounter;
            Xp = xp;

            _baseStrength = model.BaseStrength;
            _baseIntelligence = model.BaseInteligence;
            _baseCharisma = model.BaseCharisma;
            
            gameObject.name = $"{model.Name}_{Id}";
            gameObject.layer = LayerMask.NameToLayer("Actor");
            
            // now add the components and initialize them

            switch (model.ActorInputSource)
            {
                case ActorModel.EActorInputSource.Player:
                    gameObject.AddComponent<PlayerActorInputProvider>();
                    break;
                case ActorModel.EActorInputSource.AI:
                    gameObject.AddComponent<NPCActorInputProvider>();
                    break;
                case ActorModel.EActorInputSource.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            gameObject.AddComponent<Interactor>();

            if (model.ColliderSize.magnitude > 1)
            {
                if(model.MoveSpeed > 0)
                    gameObject.AddComponent<ActorGridMover>();
                else
                    gameObject.AddComponent<ActorGridCollider>();
            }

            if (model.Health > 0)
                gameObject.AddComponent<Damageable>();

            if (model.AggroRange > 0)
                gameObject.AddComponent<Combatant>();

            // register actor components, then initialize them
            _typeToActorComponent = new Dictionary<Type, ActorComponent>();
            var actorComponents = gameObject.GetComponents<ActorComponent>();
            foreach (var actorComponent in actorComponents)
                _typeToActorComponent.Add(actorComponent.GetType(), actorComponent);
            foreach (var actorComponent in actorComponents)
                actorComponent.Initialize(this, model);
            
            // instantiate the view last
            var prefab = default(GameObject);
            if (!string.IsNullOrEmpty(viewOverride.AssetGUID))
                prefab = await Addressables.LoadAssetAsync<GameObject>(viewOverride).Task;
            else if (!string.IsNullOrEmpty(model.ViewAsset))
                prefab = await Addressables.LoadAssetAsync<GameObject>(model.ViewAsset).Task;
            else
                return;
            
            Instantiate(prefab, transform, false).RequireComponent<ActorView>().Initialize(this);
        }

        public bool TryGet<T>(out T actorComponent) where T : ActorComponent
        {
            actorComponent = default;
            if (!_typeToActorComponent.TryGetValue(typeof(T), out var ac))
                return false;

            actorComponent = (T) ac;
            return true;
        }
    }
}