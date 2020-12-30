using System;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuRPG.Game.UI;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class SignPost : MonoBehaviour, IInteractable
    {
        public Actor Actor => null;
        public bool IsBeingInteractedWith => false;

        public event EventHandler<InteractableEventArgs> InteractionBegan;
        
        [TextArea] public string dialog;

        public void BeginInteraction(Interactor interactor)
        {
            InGameStateController.Blackboard?.Invoke(new EnqueueDialogEvent(new Dialog
            {
               Text = dialog,
               OnDialogComplete = null,
            }));
            
            InteractionBegan?.Invoke(this, new InteractableEventArgs(interactor));
        }
    }
}