using System;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuRPG.Game.UI;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class NPCDialogInteractable : MonoBehaviour, IInteractable
    {
        public event EventHandler<InteractableEventArgs> InteractionBegan;
        public event EventHandler<InteractableEventArgs> InteractionEnded;
        
        [TextArea] public string dialog;

        public void BeginInteraction(Interactor interactor)
        {
            InGameStateController.Blackboard?.Invoke(new EnqueueDialogEvent(dialog));
            InteractionBegan?.Invoke(this, new InteractableEventArgs(interactor));
        }

        public void EndInteraction(Interactor interactor)
        {
            InteractionEnded?.Invoke(this, new InteractableEventArgs(interactor));
        }
    }
}