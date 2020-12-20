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
        public int Xp { get; private set; }

        public float Strength => _baseStrength;
        public float Intelligence => _baseIntelligence;
        public float Charisma => _baseCharisma;

        private float _baseStrength;
        private float _baseIntelligence;
        private float _baseCharisma;
        private BoxCollider2D _unityCollider;
        
        public async Task Initialize(ActorModel model, int xp)
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
                    gameObject.AddComponent<PlayerActorInputProvider>().Initialize(this, model);
                    break;
                case ActorModel.EActorInputSource.AI:
                    gameObject.AddComponent<NPCActorInputProvider>().Initialize(this, model);
                    break;
            }

            if (model.ColliderSize.magnitude > 1)
            {
                if(model.MoveSpeed > 0)
                    gameObject.AddComponent<ActorGridMover>().Initialize(this, model);
                else
                    gameObject.AddComponent<ActorGridCollider>().Initialize(this, model);
            }

            gameObject.AddComponent<Interactor>().Initialize(this, model);

            if (model.Health > 0)
                gameObject.AddComponent<Damageable>().Initialize(this, model);

            if (model.AggroRange > 0)
                gameObject.AddComponent<Combatant>().Initialize(this, model);
            
            // instantiate the view last, return if there is no view
            if (string.IsNullOrEmpty(model.ViewAsset))
                return;
            
            var viewPrefab = await Addressables.LoadAssetAsync<GameObject>(model.ViewAsset).Task;
            Instantiate(viewPrefab, transform, false);
        }
    }
}