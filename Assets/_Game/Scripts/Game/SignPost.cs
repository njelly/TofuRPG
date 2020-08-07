using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class SignPost : MonoBehaviour, Interactor.IInteractable
    {
        [TextArea] public string _dialog;

        public void BeginInteraction(Interactor interactor)
        {
            Debug.Log(_dialog);
        }

        public void EndInteraction(Interactor interactor)
        {
        }
    }
}