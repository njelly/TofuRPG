﻿using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuRPG.Game.UI;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class SignPost : MonoBehaviour, IInteractable
    {
        [TextArea] public string dialog;

        public void BeginInteraction(Interactor interactor)
        {
            InGameStateController.Blackboard?.Invoke(new EnqueueDialogEvent(new Dialog()
            {
                pages = new string[] { dialog },
            }));
        }

        public void EndInteraction(Interactor interactor) { }
    }
}