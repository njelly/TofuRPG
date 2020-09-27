using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class ActorView : MonoBehaviour
    {
        [SerializeField] private Actor _actor;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private ActorFacing _actorFacing;

        private void Awake()
        {
            _actorFacing = _actor.GetComponent<ActorFacing>();
        }

        private void Update()
        {
            UpdateFacing();
        }

        private void UpdateFacing()
        {
            if(!_actorFacing)
            {
                return;
            }

            switch (_actorFacing.Direction)
            {
                case ECardinalDirection4.East:
                    _spriteRenderer.flipX = false;
                    break;
                case ECardinalDirection4.West:
                    _spriteRenderer.flipX = true;
                    break;
            }
        }
    }
}