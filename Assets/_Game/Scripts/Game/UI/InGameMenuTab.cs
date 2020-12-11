using DG.Tweening;
using TMPro;
using Tofunaut.TofuUnity.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Tofunaut.TofuRPG.Game.UI
{
    public class InGameMenuTab : Tab
    {
        [Header("InGameMenuTab")]
        public Image backgroundImage;
        public TextMeshProUGUI label;

        protected override void OnOpen()
        {
            backgroundImage.DOColor(Color.white, 0.1f);
            label.DOColor(Color.black, 0.1f);
        }

        protected override void OnClose()
        {
            backgroundImage.DOColor(new Color(1f, 1f, 1f, 0.2f), 0.1f);
            label.DOColor(Color.white, 0.1f);
        }
        
    }
}