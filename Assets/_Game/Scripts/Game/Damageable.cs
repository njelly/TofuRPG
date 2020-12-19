using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tofunaut.TofuRPG.Game
{
    public class Damageable : ActorComponent
    {
        public class TookDamageEventArgs : EventArgs
        {
            public readonly float PrevHealth;
            public readonly float CurrentHealth;

            public TookDamageEventArgs(float prevHealth, float currentHealth)
            {
                PrevHealth = prevHealth;
                CurrentHealth = currentHealth;
            }
        }

        public event EventHandler<TookDamageEventArgs> TookDamage;
        
        public float Health { get; private set; }
        public float ImpactResistance => _baseImpactResistance;
        public float CrushResistance => _baseCrushResistance;
        public float SlashResistance => _baseSlashResistance;
        public float PierceResistance => _basePierceResistance;
        public float BurnResistance => _baseBurnResistance;
        public float FreezeResistance => _baseFreezeResistance;
        public float ShockResistance => _baseShockResistance;
        public float PoisonResistance => _basePoisonResistance;
        
        public bool IsDead => Health <= 0;

        private float _baseHealth;
        private float _baseImpactResistance;
        private float _baseCrushResistance;
        private float _baseSlashResistance;
        private float _basePierceResistance;
        private float _baseBurnResistance;
        private float _baseFreezeResistance;
        private float _baseShockResistance;
        private float _basePoisonResistance;

        public void TakeDamage(Attack attack)
        {
            var prevHealth = Health;
            Health -= attack.Damage;
            Health = Mathf.Clamp(Health, 0f, float.MaxValue);
            
            TookDamage?.Invoke(this, new TookDamageEventArgs(prevHealth, Health));
        }

        public override void Initialize(Actor actor, ActorModel model)
        {
            _baseHealth = model.Health;
            _baseImpactResistance = model.ImpactResistance;
            _baseCrushResistance = model.CrushResistance;
            _baseSlashResistance = model.SlashResistance;
            _basePierceResistance = model.PierceResistance;
            _baseBurnResistance = model.BurnResistance;
            _baseFreezeResistance = model.FreezeResistance;
            _baseShockResistance = model.ShockResistance;
            _basePoisonResistance = model.PoisonResitance;
            
            Health = _baseHealth;
        }
    }
}