using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Weapon : MonoBehaviour
    {
        public bool IsAttacking { get; private set; }

        public float attackCooldown;
        public Attack attack;

        private float _lastAttackTime;

        public void StartAttack(Combatant combatant, Vector2Int position, Vector2 direction)
        {
            if (!attack)
            {
                return;
            }

            if (Time.time - _lastAttackTime < attackCooldown)
            {
                return;
            }

            _lastAttackTime = Time.time;

            attack.DoAttack(combatant, position, direction);
        }
    }
}