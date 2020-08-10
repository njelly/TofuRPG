using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [CreateAssetMenu(fileName = "New Attack", menuName = "TofuRPG/Attack")]
    public class Attack : ScriptableObject
    {
        public float damage;
        public DamageMatrix.EDamageAttribute damageAttribute;
        public float hitBoxStartTime;
        public float hitBoxDuration;
        public float sameHitCooldown;

        public void DoAttack(Combatant combatant, Vector2Int coordinate, Vector2 direction)
        {
            Debug.Log($"do attack: {name} at {coordinate} facing {direction}");
        }
    }
}