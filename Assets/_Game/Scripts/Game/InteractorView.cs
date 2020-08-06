using System.Collections;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class InteractorView : MonoBehaviour
    {
        public Interactor interactor;
        public SpriteRenderer flipXFacing;
        public SpriteRenderer reticle;
        public float reticleMoveAnimTime;
        public float reticleFlashAnimTime;

        private ECardinalDirection4 _prevFacing;

        private void Start()
        {
            if (!interactor)
            {
                return;
            }

            _prevFacing = interactor.Facing;
        }

        private void Update()
        {
            if (!interactor)
            {
                return;
            }

            if (flipXFacing)
            {
                flipXFacing.enabled = interactor.enabled;
            }

            ECardinalDirection4 facing = interactor.Facing;
            if (_prevFacing != facing)
            {
                switch (facing)
                {
                    case ECardinalDirection4.East:
                        flipXFacing.flipX = false;
                        break;
                    case ECardinalDirection4.West:
                        flipXFacing.flipX = true;
                        break;
                }

                StartCoroutine(AnimateReticleCoroutine());

                _prevFacing = facing;
            }
        }

        private IEnumerator AnimateReticleCoroutine()
        {
            float startRot = Vector2.Angle(Vector2.right, transform.localPosition);
            if (transform.localPosition.y < 0)
            {
                startRot *= -1;
            }

            Vector2 endPos = interactor.Facing.ToVector2();
            float endRot = Vector2.Angle(Vector2.right, endPos);
            if (endPos.y < 0)
            {
                endRot *= -1;
            }

            float timer = 0f;
            while (timer < reticleMoveAnimTime)
            {
                float ratio = timer / reticleMoveAnimTime;
                Quaternion rot = Quaternion.SlerpUnclamped(Quaternion.Euler(0f, 0f, startRot), Quaternion.Euler(0f, 0f, endRot), ratio);
                transform.localPosition = rot * Vector2.right;
                timer += Time.deltaTime;

                yield return null;
            }

            transform.localPosition = endPos;
        }
    }
}