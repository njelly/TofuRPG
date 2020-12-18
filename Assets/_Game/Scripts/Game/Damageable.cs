using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tofunaut.TofuRPG.Game
{
    public class Damageable : MonoBehaviour
    {
        [Serializable]
        public class TookDamageEvent : UnityEvent<Damageable, Attack, float>
        {
        }
        
        public float Health { get; private set; }
        public bool IsDead => Health <= 0;

        public float baseHealth;
        public TookDamageEvent onTookDamage;

        private void Awake()
        {
            Health = baseHealth;
        }

        public void TakeDamage(Attack attack)
        {
            var prevHealth = Health;
            Health -= attack.BaseDamage;

            onTookDamage?.Invoke(this, attack, prevHealth);
        }
    }
}