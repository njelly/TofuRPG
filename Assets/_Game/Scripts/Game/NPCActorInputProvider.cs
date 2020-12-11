using System;
using System.Collections.Generic;
using System.Linq;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuRPG.UI;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game.AI
{
    public class NPCActorInputProvider : MonoBehaviour , IActorInputProvider
    {
        
        public ActorInput ActorInput { get; private set; }
        
        public NPCBrain brainAsset;

        private GridCollider _collider;
        private NPCBrain _brain;
        private IInteractable _interactable;

        private void Awake()
        {
            ActorInput = new ActorInput();
        }

        private void Start()
        {
            var components = GetComponents<MonoBehaviour>();
            
            _interactable = components.OfType<IInteractable>().FirstOrDefault();
            if(_interactable != null)
                _interactable.InteractionBegan += Interactable_InteractionBegan;

            _collider = components.OfType<GridCollider>().FirstOrDefault();

            _brain = Instantiate(brainAsset);
            _brain.Initialize(gameObject);
        }

        private void Update()
        {
            if (_interactable != null && _interactable.IsBeingInteractedWith)
            {
                ActorInput.Reset();
                return;
            }
            
            _brain.Update();
            
            CheckBrainPath();
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