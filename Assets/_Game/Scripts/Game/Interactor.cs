using System.Linq;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Interactor : MonoBehaviour
    {
        public Vector2Int InteractOffset { get; private set; }
        public IInteractable InteractingWith { get; private set; }

        [SerializeField] private Vector2Int _baseInteractOffset;
        
        private IActorInputProvider _actorInputProvider;
        private IFacing _facing;
        private ICoordProvider _coordProvider;
        private ECardinalDirection4 _prevFacing;
        private Vector2Int _defaultInteractOffset;

        private void Awake()
        {
            var components = GetComponents<MonoBehaviour>();
            _actorInputProvider = components.OfType<IActorInputProvider>().FirstOrDefault();
            _facing = components.OfType<IFacing>().FirstOrDefault();
            _coordProvider = components.OfType<ICoordProvider>().FirstOrDefault();
            _prevFacing = _facing?.Facing ?? ECardinalDirection4.East;
            
            UpdateInteractOffset();
        }

        private void Update()
        {
            if(_facing != null && _facing.Facing != _prevFacing)
                UpdateInteractOffset();

            var actorInput = _actorInputProvider.ActorInput;
            if (actorInput.Interact.WasPressed)
                TryInteract();
            else if(actorInput.Interact.WasReleased)
                TryEndInteract();
        }

        private void TryInteract()
        {
            InteractingWith = null;
            foreach (var gc in GridCollisionManager.GetCollidersAt(_coordProvider.Coord + InteractOffset))
            {
                // TODO: what's a clever way of doing this?
                InteractingWith = gc.gameObject.GetComponent<IInteractable>();
                if (InteractingWith != null)
                    break;
            }

            InteractingWith?.BeginInteraction(this);
        }

        private void TryEndInteract()
        {
            InteractingWith?.EndInteraction(this);
        }

        private void UpdateInteractOffset()
        {
            var baseInteractOffsetFloat = _baseInteractOffset.ToVector2();
            var facingVec = _facing.Facing.ToVector2();
            var angle = Vector2.SignedAngle(Vector2.right, facingVec);
            baseInteractOffsetFloat = baseInteractOffsetFloat.Rotate(angle);
            InteractOffset = baseInteractOffsetFloat.RoundToVector2Int();

            _prevFacing = _facing.Facing;
        }
    }
}