using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuRPG.UI;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tofunaut.TofuRPG.Game.AI
{
    public class NPCActorInputProvider : ActorComponent , IActorInputProvider
    {
        public ActorInput ActorInput { get; private set; }

        private ActorGridCollider _collider;
        private NPCBrain _brain;
        private IInteractable _interactable;

        private void Awake()
        {
            ActorInput = new ActorInput();
        }

        private async void Start()
        {
            var components = GetComponents<MonoBehaviour>();
            
            _interactable = components.OfType<IInteractable>().FirstOrDefault();
            if(_interactable != null)
                _interactable.InteractionBegan += Interactable_InteractionBegan;

            _collider = components.OfType<ActorGridCollider>().FirstOrDefault();

            while (!_brain)
                await Task.Yield();
            
            _brain.Initialize(gameObject);
        }

        private void Update()
        {
            if (_interactable != null && _interactable.IsBeingInteractedWith)
            {
                ActorInput.Reset();
                return;
            }
            
            if(_brain)
                UpdateBrain();
        }

        private void UpdateBrain()
        {
            _brain.Update();
            CheckBrainPath();
        }

        public override async void Initialize(Actor actor, ActorModel model)
        {
            if (!string.IsNullOrEmpty(model.AIAsset))
            {
                var npcBrain = await Addressables.LoadAssetAsync<NPCBrain>(model.AIAsset).Task;
                _brain = Instantiate(npcBrain);
                _brain.Initialize(gameObject);
            }
        }

        private void CheckBrainPath()
        {
            if (_brain.Path.Count <= 0)
            {
                ActorInput.Direction.SetAxis(Vector2.zero);
                return;
            }
            
            var toNextCoord = _brain.Path.Peek() - _collider.Coord;
            ActorInput.Direction.SetAxis(toNextCoord);
        }

        private void Interactable_InteractionBegan(object sender, InteractableEventArgs e)
        {
            var toInteractor = ((Vector2) (e.Interactor.transform.position - transform.localPosition)).RoundToVector2Int().ToVector2();
            ActorInput.Direction.SetAxis(toInteractor);
        }
        
        
    }

}