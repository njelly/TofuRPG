using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [CreateAssetMenu(fileName = "new Attack", menuName = "TofuRPG/Attack")]
    public class Attack : ScriptableObject
    {
        public Combatant Instigator { get; private set; }
        public string DisplayName;
        public float BaseDamage;

        public void Initialize(Combatant instigator)
        {
            Instigator = instigator;
        }
    }
}