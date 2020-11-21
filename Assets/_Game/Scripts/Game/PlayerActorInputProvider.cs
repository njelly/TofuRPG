using System;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuRPG.Game.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuRPG.Game
{
    public class PlayerActorInputProvider : MonoBehaviour, IActorInputProvider
    {
        public ActorInput ActorInput { get; private set; }

        public bool registerWithManagerOnStart;

        private void Awake()
        {
            ActorInput = new ActorInput();
        }

        public void Start()
        {
            if (registerWithManagerOnStart)
                RegisterWithPlayerActorInputManager();
        }

        private void RegisterWithPlayerActorInputManager()
        {
            ActorInput = PlayerActorInputManager.InstanceActorInput;
            InGameStateController.Blackboard?.Subscribe<ShowDialogEvent>(OnShowDialog);
            InGameStateController.Blackboard?.Unsubscribe<HideDialogEvent>(OnHideDialog);
        }

        private void UnregisterWithPlayerActorInputManager()
        {
            ActorInput = new ActorInput();
            InGameStateController.Blackboard?.Unsubscribe<ShowDialogEvent>(OnShowDialog);
            InGameStateController.Blackboard?.Subscribe<HideDialogEvent>(OnHideDialog);
        }

        private void OnShowDialog(ShowDialogEvent e)
        {
            UnregisterWithPlayerActorInputManager();
        }

        private void OnHideDialog(HideDialogEvent e)
        {
            RegisterWithPlayerActorInputManager();
        }
    }
}