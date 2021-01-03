using System.Linq;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Interactor : ActorComponent
    {
        public Vector2Int InteractOffset { get; private set; }
        public IInteractable InteractingWith { get; private set; }
        public IGridCollider CurrentlyFacing { get; private set; }
        
        private Vector2Int _baseInteractOffset;
        private IActorInputProvider _actorInputProvider;
        private IFacing _facing;
        private ICoordProvider _coordProvider;
        private ECardinalDirection4 _prevFacing;
        private Vector2Int _defaultInteractOffset;

        private void Start()
        {
            var components = GetComponents<MonoBehaviour>();
            _actorInputProvider = components.OfType<IActorInputProvider>().FirstOrDefault();
            _facing = components.OfType<IFacing>().FirstOrDefault();
            if (_facing == null)
            {
                Debug.LogError("parent does not contain a component of type IFacing");
                return;
            }
            
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
        }

        public override void Initialize(Actor actor, ActorModel model)
        {
            _baseInteractOffset = model.BaseInteractOffset;
        }

        private void TryInteract()
        {
            InteractingWith?.BeginInteraction(this);
        }

        private void UpdateInteractOffset()
        {
            var baseInteractOffsetFloat = _baseInteractOffset.ToVector2();
            var facingVec = _facing.Facing.ToVector2();
            var angle = Vector2.SignedAngle(Vector2.right, facingVec);
            baseInteractOffsetFloat = baseInteractOffsetFloat.Rotate(angle);
            InteractOffset = baseInteractOffsetFloat.RoundToVector2Int();

            _prevFacing = _facing.Facing;
            
            InteractingWith = null;
            foreach (var gc in GridCollisionManager.GetCollidersAt(_coordProvider.Coord + InteractOffset))
            {
                var gcGameObject = gc as MonoBehaviour;
                if (gcGameObject == null)
                    continue;
                
                // TODO: what's a clever way of doing this?
                InteractingWith = gcGameObject.GetComponent<IInteractable>();
                if (InteractingWith != null)
                    break;
            }
        }
    }
}