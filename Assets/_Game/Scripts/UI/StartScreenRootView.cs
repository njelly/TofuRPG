using TMPro;
using UnityEngine;

namespace Tofunaut.TofuRPG.UI
{
    public class StartScreenRootView : ViewController
    {
        protected override float CanvasFadeInTime => 0f;
        
        [Header("Start Screen Root View")]
        public AppVersionProviderAsset appVersionProviderAsset;
        public TextMeshProUGUI appVersionLabel;

        private void Start()
        {
            appVersionLabel.text = appVersionProviderAsset.Version;
        }
    }
}