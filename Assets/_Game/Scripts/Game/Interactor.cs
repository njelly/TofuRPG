using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [RequireComponent(typeof(Actor))]
    public class Interactor : MonoBehaviour, Actor.IActorInputReceiver
    {
        public ECardinalDirection4 Facing { get; private set; }

        private Actor _actor;
        private ECardinalDirection4 _prevFacing;

        private void Awake()
        {
            _actor = gameObject.GetComponent<Actor>();
        }

        private void OnEnable()
        {
            _actor.AddReceiver(this);
        }

        private void OnDisable()
        {
            if (_actor)
            {
                _actor.RemoveReceiver(this);
            }
        }

        public void ReceiveActorInput(ActorInput input)
        {
            if (input.direction.sqrMagnitude > float.Epsilon)
            {
                Facing = input.direction.ToCardinalDirection4();
            }

            if (input.interact.WasPressed)
            {
                //GridCollisionManager.
                Debug.Log("interact!");
            }
        }
    }
}