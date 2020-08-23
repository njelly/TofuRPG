﻿using System.Collections;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class ActorView : MonoBehaviour
    {
        public Actor actor;
        public SpriteRenderer flipXFacing;
        public SpriteRenderer interactReticle;
        public float reticleMoveAnimTime;
        public float reticleFlashAnimTime;
        [Range(0f, 1f)] public float reticleFlashAlpha;
        public Color reticleColor = Color.white;

        private ECardinalDirection4 _prevFacing;
        private bool _isInteracting;
        private float _baseAlpha;

        private void Start()
        {
            if (!actor)
            {
                return;
            }

            _prevFacing = actor.Facing;

            if (interactReticle)
            {
                _baseAlpha = interactReticle.color.a;
            }
        }

        private void Update()
        {
            if (!actor)
            {
                return;
            }

            if (flipXFacing)
            {
                flipXFacing.enabled = actor.enabled;
            }

            bool interacting = actor.InteractingWith != null;
            if (interacting != _isInteracting)
            {
                _isInteracting = interacting;
                StartCoroutine(AnimateReticleFlashCoroutine(_isInteracting ? 0.8f : _baseAlpha));
            }

            ECardinalDirection4 facing = actor.Facing;
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

                StartCoroutine(AnimateReticlePositionCoroutine());

                _prevFacing = facing;
            }
        }

        private IEnumerator AnimateReticlePositionCoroutine()
        {
            float startRot = Vector2.Angle(Vector2.right, transform.localPosition);
            if (transform.localPosition.y < 0)
            {
                startRot *= -1;
            }

            Vector2 endPos = actor.Facing.ToVector2();
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

        private IEnumerator AnimateReticleFlashCoroutine(float alpha)
        {
            if (!interactReticle)
            {
                yield break;
            }

            float startAlpha = interactReticle.color.a;
            float endAlpha = alpha;

            float timer = 0f;
            while (timer < reticleFlashAnimTime)
            {
                timer += Time.deltaTime;

                // set these every frame so reticleColor RGB can be updated independently
                Color from = new Color(reticleColor.r, reticleColor.g, reticleColor.b, startAlpha);
                Color to = new Color(reticleColor.r, reticleColor.g, reticleColor.b, endAlpha);
                interactReticle.color = Color.Lerp(from, to, timer / reticleFlashAnimTime);

                yield return null;
            }

            interactReticle.color = new Color(reticleColor.r, reticleColor.g, reticleColor.b, endAlpha);
        }
    }
}