using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Combatant : ActorComponent
    {
        public Attack[] attacks;

        private Actor _actor;
        private string _defaultAttack;
        private float _baseAggroRange;

        public void DoAttack(string attackKey, Damageable target)
        {
            if (InGameStateController.Config == null)
            {
                Debug.LogError("cant attack, config is null");
                return;
            }

            if (!InGameStateController.Config.TryGetAttackModel(attackKey, out var attackModel))
            {
                Debug.LogError($"could not find attack with key {attackKey}");
                return;
            }
            
            var attack = new Attack(this, attackModel);
            target.TakeDamage(attack);
        }

        public override void Initialize(Actor actor, ActorModel model)
        {
            _actor = actor;
            _defaultAttack = model.DefaultAttack;
            _baseAggroRange = model.AggroRange;
        }
    }
}