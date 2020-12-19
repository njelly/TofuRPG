using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Attack
    {
        public float Damage => _model.Damage;
        public readonly Combatant instigator;

        private readonly AttackModel _model;
        
        public Attack(Combatant instigator, AttackModel model)
        {
            this.instigator = instigator;
            _model = model;
        }
    }
}