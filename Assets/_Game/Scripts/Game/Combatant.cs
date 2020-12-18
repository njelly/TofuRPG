using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Combatant : MonoBehaviour
    {
        public Attack[] attacks;

        public void DoAttack(int attackIndex, Damageable target)
        {
            var attack = Instantiate(attacks[attackIndex]);
            attack.Initialize(this);
            target.TakeDamage(attack);
        }
    }
}