using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Chest : MonoBehaviour, Interactor.IInteractable
    {
        public bool Open { get; private set; }

        public void BeginInteraction(Interactor interactor)
        {
            Open = !Open;
        }

        public void EndInteraction(Interactor interactor)
        {

        }
    }
}