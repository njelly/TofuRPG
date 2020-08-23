using UnityEngine;
using UnityEngine.Events;

namespace Tofunaut.TofuRPG.Game
{
    // an IInteractable that triggers some arbitray event *poke*
    public class Pokeable : MonoBehaviour, Actor.IInteractable
    {
        public UnityEvent OnBeginInteraction;
        public UnityEvent OnEndInteraction;

        public void BeginInteraction(Actor actor)
        {
            OnBeginInteraction?.Invoke();
        }

        public void EndInteraction(Actor actor)
        {
            OnEndInteraction?.Invoke();
        }
    }
}