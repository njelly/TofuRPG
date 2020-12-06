using System;
using System.Linq;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class NPCActorInputProvider : MonoBehaviour , IActorInputProvider
    {
        [Flags]
        protected enum NPCFlags
        {
            Nothing = 0,
            IsBeingInteractedWith = 1 << 0,
        }
        
        public ActorInput ActorInput { get; private set; }
        public bool IsBeingInteractedWith => (_flags & NPCFlags.IsBeingInteractedWith) != 0;

        private NPCFlags _flags;

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
        }

        private void Update()
        {
            if(IsBeingInteractedWith)
                ActorInput.Reset();
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