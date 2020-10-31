using System.Linq;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Interactor : MonoBehaviour
    {
        public Vector2Int InteractOffset;
        public IInteractable InteractingWith { get; private set; }
        
        private IActorInputProvider _actorInputProvider;
        private IFacing _facing;
        private ICoordProvider _coordProvider;
        private ECardinalDirection4 _prevFacing;

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
            if (actorInput.Interact.Pressed)
            {
                InteractingWith = null;
                foreach (var gc in GridCollisionManager.GetCollidersAt(_coordProvider.Coord + InteractOffset))
                {
                    // TODO: what's a clever way of doing this?
                    InteractingWith = gc.gameObject.GetComponent<IInteractable>();
                    if (InteractingWith != null)
                        break;
                }
                if(InteractingWith != null)
                    InteractingWith.BeginInteraction(this);
            }
            else if(actorInput.Interact.Released)
                InteractingWith?.EndInteraction(this);
        }

        private void UpdateInteractOffset()
        {
            var facingVectorInt = _facing.Facing.ToVector2Int();
            var facingVector = new Vector2(facingVectorInt.x, facingVectorInt.y);
            var offsetVector = ((Vector2) InteractOffset).Rotate(Vector2.SignedAngle(Vector2.right, facingVector));
            InteractOffset = new Vector2Int(Mathf.RoundToInt(offsetVector.x), Mathf.RoundToInt(offsetVector.y));
        }
    }
}