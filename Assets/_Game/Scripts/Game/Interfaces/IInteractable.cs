using System;

namespace Tofunaut.TofuRPG.Game.Interfaces
{
    public interface IInteractable
    {
        event EventHandler<InteractableEventArgs> InteractionBegan;
        event EventHandler<InteractableEventArgs> InteractionEnded;
        void BeginInteraction(Interactor interactor);
        void EndInteraction(Interactor interactor);
    }

    public class InteractableEventArgs : EventArgs
    {
        public readonly Interactor Interactor;

        public InteractableEventArgs(Interactor interactor)
        {
            Interactor = interactor;
        }
    }
}