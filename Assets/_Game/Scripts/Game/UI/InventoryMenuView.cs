using System;
using System.Collections.Generic;
using Tofunaut.TofuRPG.Scripts.UI;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game.UI
{
    public class InventoryMenuView : MonoBehaviour
    {
        public InventoryItemView inventoryItemViewPrefab;
        public Transform container;

        private List<InventoryItemView> _instantiatedInventoryItems;

        private void Awake()
        {
            _instantiatedInventoryItems = new List<InventoryItemView>();
        }

        private async void OnEnable()
        {
            UserSettings.AddItem("ItemSpec_Stick", 2);
            UserSettings.AddItem("ItemSpec_Gold", 180);
            UserSettings.AddItem("ItemSpec_Charcoal", 1);
            
            foreach (var inventoryItemView in _instantiatedInventoryItems)
                Destroy(inventoryItemView.gameObject);
            
            _instantiatedInventoryItems.Clear();
            
            var playerItems = UserSettings.GetItems();
            foreach (var kvp in playerItems)
            {
                var inventoryItemView = Instantiate(inventoryItemViewPrefab, container, false);
                await inventoryItemView.Initialize(kvp.Key, kvp.Value);
                _instantiatedInventoryItems.Add(inventoryItemView);
            }
        }
    }
}