using System;

namespace Tofunaut.TofuRPG.Game.Interfaces
{
    public interface IInteractable
    {
        bool IsBeingInteractedWith { get; }
        event EventHandler<InteractableEventArgs> InteractionBegan;
        void BeginInteraction(Interactor interactor);
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