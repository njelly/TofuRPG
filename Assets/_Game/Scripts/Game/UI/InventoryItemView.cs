using System;
using System.Threading.Tasks;
using TMPro;
using Tofunaut.TofuRPG.Game;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Tofunaut.TofuRPG.Scripts.UI
{
    public class InventoryItemView : MonoBehaviour
    {
        public string ItemSpecPath { get; private set; }
        
        public TextMeshProUGUI itemNameLabel;
        public TextMeshProUGUI quantityLabel;
        public Image itemSprite;

        private void OnEnable()
        {
            itemNameLabel.text = string.Empty;
            quantityLabel.text = string.Empty;
            itemSprite.sprite = null;
        }

        public async Task Initialize(string assetPath, int quantity)
        {
            itemNameLabel.text = string.Empty;
            quantityLabel.text = string.Empty;
            itemSprite.sprite = null;
            
            var itemSpec = await Addressables.LoadAssetAsync<ItemSpec>(assetPath).Task;
            
            ItemSpecPath = assetPath;
            itemNameLabel.text = itemSpec.displayName;
            quantityLabel.text = quantity > 1 ? quantity.ToString() : string.Empty;
            itemSprite.sprite = itemSpec.itemSprite;
        }
    }
}