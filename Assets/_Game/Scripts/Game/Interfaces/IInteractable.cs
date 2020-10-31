namespace Tofunaut.TofuRPG.Game.Interfaces
{
    public interface IInteractable
    {
        void BeginInteraction(Interactor interactor);
        void EndInteraction(Interactor interactor);
    }
}