using TMPro;
using UnityEngine;

namespace Tofunaut.TofuRPG.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AppVersionLabel : MonoBehaviour
    {
        public AppVersionProviderAsset appVersionProviderAsset;
        
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _text.text = string.Empty;
        }

        private void Start()
        {
            _text.text = appVersionProviderAsset.Version.ToString();
        }
    }

}