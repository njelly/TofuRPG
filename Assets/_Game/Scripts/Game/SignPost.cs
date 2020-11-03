using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuRPG.Game.UI;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class SignPost : MonoBehaviour, IInteractable
    {
        [SerializeField] private Dialog _dialog;
        
        public void BeginInteraction(Interactor interactor)
        {
            InGameStateController.Blackboard?.Invoke(new EnqueueDialogEvent(_dialog));
        }

        public void EndInteraction(Interactor interactor) { }
    }
}