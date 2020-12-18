using System;
using System.Linq;
using Tofunaut.TofuRPG.Game.Interfaces;
using UnityEngine;
using Tofunaut.TofuUnity;

namespace Tofunaut.TofuRPG.Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteFlipXForFacing : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private IFacing _facing;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            var components = transform.parent.GetComponents<MonoBehaviour>();
            _facing = components.OfType<IFacing>().FirstOrDefault();
        }

        private void LateUpdate()
        {
            if (!_spriteRenderer || _facing == null)
                return;
            
            switch (_facing.Facing)
            {
                case ECardinalDirection4.East when _spriteRenderer.flipX:
                    _spriteRenderer.flipX = false;
                    break;
                case ECardinalDirection4.West when !_spriteRenderer.flipX:
                    _spriteRenderer.flipX = true;
                    break;
            }
        }
    }
}