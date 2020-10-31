using UnityEngine;
using Tofunaut.TofuUnity;

namespace Tofunaut.TofuRPG.Game
{
    public class SpriteFlipXForGridMover : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public GridMover gridMover;

        private void LateUpdate()
        {
            if (!spriteRenderer || !gridMover)
                return;
            
            switch (gridMover.MoveDirection)
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