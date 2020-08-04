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
            Vector3 startPos = transform.localPosition;
            if (startPos.sqrMagnitude <= float.Epsilon)
            {
                startPos = Vector3.right;
            }
            Quaternion startRot = Quaternion.LookRotation(startPos, Vector3.back);
            Quaternion endRot = Quaternion.LookRotation(interactor.Facing.ToVector2(), Vector3.back);

            float timer = 0f;
            while (timer < 0.5f)
            {
                float ratio = timer / 0.5f;
                Quaternion rot = Quaternion.SlerpUnclamped(startRot, endRot, ratio);
                transform.localPosition = rot * Vector3.right;

                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}