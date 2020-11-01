using Tofunaut.TofuRPG.Game.Interfaces;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class SignPost : MonoBehaviour, IInteractable
    {
        [TextArea] public string[] pages;
        
        public void BeginInteraction(Interactor interactor)
        {
            foreach(var s in pages)
                Debug.Log(s);
        }

        public void EndInteraction(Interactor interactor)
        {
            
        }
    }
}