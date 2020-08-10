using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [RequireComponent(typeof(Actor))]
    public class Combatant : MonoBehaviour, Actor.IActorInputReceiver
    {
        public Weapon weapon;

        private Actor _actor;
        private Aimer _aimer;
        private GridCollider _gridCollider;

        private void Awake()
        {
            _actor = gameObject.GetComponent<Actor>();
            _aimer = gameObject.GetComponent<Aimer>();
            _gridCollider = gameObject.GetComponent<GridCollider>();
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
            if (input.interact.Pressed)
            {
                weapon.StartAttack(this, GridCollisionManager.ConvertToGridPosition(_gridCollider.Coord + _aimer.AimVector), _aimer.AimVector);
            }
        }
    }
}