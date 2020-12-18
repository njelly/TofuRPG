using System;
using System.Threading.Tasks;
using Tofunaut.TofuRPG.UI;
using Tofunaut.TofuUnity.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuRPG.Game.UI
{
    public class InGameMenuView : ViewController
    {
        public TabManager tabManager;

        private InputAction _showInventoryAction;
        private InputAction _showPartyStatsAction;

        private async void Start()
        {
            while (ViewControllerStack.PlayerInput == null)
                await Task.Yield();
            
            _showInventoryAction = ViewControllerStack.PlayerInput.actions["Player/ShowInventory"];
            _showPartyStatsAction = ViewControllerStack.PlayerInput.actions["Player/ShowPartyStats"];
            _showInventoryAction.performed += OnShowInventory;
            _showPartyStatsAction.performed += OnShowPartyStats;
        }

        private void OnDestroy()
        {
            _showInventoryAction.performed -= OnShowInventory;
            _showPartyStatsAction.performed -= OnShowPartyStats;
        }

        protected override void OnHide()
        {
            UserSettings.Save();
        }

        private void OnShowInventory(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ViewControllerStack.Push(this);
                tabManager.OpenTab(1);
            }
        }

        private void OnShowPartyStats(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ViewControllerStack.Push(this);
                tabManager.OpenTab(0);
            }
        }

        protected override void OnCancel(InputAction.CallbackContext context)
        {
            base.OnCancel(context);
            if (context.performed)
                ViewControllerStack.Pop(this);
        }
    }
}