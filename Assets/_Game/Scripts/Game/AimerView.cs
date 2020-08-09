using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class AimerView : MonoBehaviour
    {
        public Aimer aimer;
        public SpriteRenderer spriteRenderer;
        public float offset;

        private void Update()
        {
            if (!aimer)
            {
                return;
            }

            spriteRenderer.enabled = aimer.IsAiming;

            transform.localPosition = aimer.AimVector * offset;

            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(transform.localPosition.y, transform.localPosition.x));
        }
    }
}
