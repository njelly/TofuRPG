using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class SignPost : MonoBehaviour, IInteractable
    {
        public void BeginInteraction(Actor actor)
        {
            Debug.Log("begin interaction");
        }

        public void EndInteraction(Actor actor)
        {
            Debug.Log("end interaction");
        }
    }
}