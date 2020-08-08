using UnityEngine;
using UnityEngine.Events;

namespace Tofunaut.TofuRPG.Game
{
    // an IInteractable that triggers some arbitray event *poke*
    public class Pokeable : MonoBehaviour, Interactor.IInteractable
    {
        public UnityEvent OnBeginInteraction;
        public UnityEvent OnEndInteraction;

        public void BeginInteraction(Interactor interactor)
        {
            OnBeginInteraction?.Invoke();
        }

        public void EndInteraction(Interactor interactor)
        {
            OnEndInteraction?.Invoke();
        }
    }
}