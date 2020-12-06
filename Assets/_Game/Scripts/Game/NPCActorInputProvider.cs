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
        [Flags]
        private enum NPCFlags
        {
            Nothing = 0,
            IsBeingInteractedWith = 1 << 0,
        }
        
        public ActorInput ActorInput { get; private set; }
        public bool IsBeingInteractedWith => (_flags & NPCFlags.IsBeingInteractedWith) != 0;
        
        public NPCBrain brainAsset;

        private NPCFlags _flags;
        private GridCollider _collider;
        private NPCBrain _brain;

        private void Awake()
        {
            ActorInput = new ActorInput();

            var components = GetComponents<MonoBehaviour>();
            
            var interactables = components.OfType<IInteractable>();
            foreach (var interactable in interactables)
            {
                interactable.InteractionBegan += Interactable_InteractionBegan;
                interactable.InteractionEnded += Interactable_InteractionEnded;
            }

            _collider = components.OfType<GridCollider>().FirstOrDefault();

            _brain = Instantiate(brainAsset);
            _brain.Initialize(gameObject);
        }

        private void Update()
        {
            if (IsBeingInteractedWith)
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
            _flags |= NPCFlags.IsBeingInteractedWith;
            var toInteractor = ((Vector2) (e.Interactor.transform.position - transform.localPosition)).RoundToVector2Int().ToVector2();
            ActorInput.Direction.SetAxis(toInteractor);
        }

        private void Interactable_InteractionEnded(object sender, InteractableEventArgs e)
        {
            _flags &= ~NPCFlags.IsBeingInteractedWith;
        }
        
        
    }

}