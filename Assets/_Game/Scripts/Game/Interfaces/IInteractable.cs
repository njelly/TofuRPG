namespace Tofunaut.TofuRPG.Game
{
    public interface IInteractable
    {
        void BeginInteraction(Actor actor);
        void EndInteraction(Actor actor);
    }
}