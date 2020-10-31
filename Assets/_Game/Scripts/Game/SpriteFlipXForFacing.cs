using System;
using System.Linq;
using Tofunaut.TofuRPG.Game.Interfaces;
using UnityEngine;
using Tofunaut.TofuUnity;

namespace Tofunaut.TofuRPG.Game
{
    public class SpriteFlipXForFacing : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public GameObject facingRoot;

        private IFacing _facing;

        private void Awake()
        {
            var components = facingRoot.GetComponents<MonoBehaviour>();
            _facing = components.OfType<IFacing>().FirstOrDefault();
        }

        private void LateUpdate()
        {
            if (!spriteRenderer || _facing == null)
                return;
            
            switch (_facing.Facing)
            {
                case ECardinalDirection4.East when spriteRenderer.flipX:
                    spriteRenderer.flipX = false;
                    break;
                case ECardinalDirection4.West when !spriteRenderer.flipX:
                    spriteRenderer.flipX = true;
                    break;
            }
        }
    }
}