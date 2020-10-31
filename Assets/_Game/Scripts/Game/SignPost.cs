using Tofunaut.TofuRPG.Game.Interfaces;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class SignPost : MonoBehaviour, IInteractable
    {
        public void BeginInteraction(Interactor interactor)
        {
            GameCameraController.SetTarget(transform);
            Debug.Log("begin interaction");
        }

        public void EndInteraction(Interactor interactor)
        {
            GameCameraController.SetTarget(interactor.transform);
            Debug.Log("end interaction");
        }
    }
}