using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Chest : MonoBehaviour, Actor.IInteractable
    {
        public bool Open { get; private set; }

        public void BeginInteraction(Actor actor)
        {
            Open = !Open;
        }

        public void EndInteraction(Actor actor)
        {

        }
    }
}