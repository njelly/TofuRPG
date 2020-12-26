using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [CreateAssetMenu(fileName = "new GameConfig", menuName = "TofuRPG/Game/Config")]
    public class GameConfig : ScriptableObject
    {
        public ActorModel[] actorModels;
        public AttackModel[] attackModels;
        
        private Dictionary<string, ActorModel> _actorLookUp;
        private Dictionary<string, AttackModel> _attackLookUp;

        private void OnEnable()
        {
            if (actorModels == null)
                actorModels = new ActorModel[0];
            if (attackModels == null)
                attackModels = new AttackModel[0];
            
            _actorLookUp = actorModels.ToDictionary(x => x.Name);
            _attackLookUp = attackModels.ToDictionary(x => x.Name);
        }

        public string[] GetActorKeys() => _actorLookUp.Keys.ToArray();
        public string[] GetAttackKeys() => _attackLookUp.Keys.ToArray();
        
        public bool TryGetActorModel(string key, out ActorModel actorModel) => _actorLookUp.TryGetValue(key, out actorModel);
        public bool TryGetAttackModel(string key, out AttackModel actorModel) => _attackLookUp.TryGetValue(key, out actorModel);

    }

    [Serializable]
    public struct ActorModel
    {
        public enum EActorInputSource
        {
            None,
            Player,
            AI,
        }
        
        public string Name;
        public string ViewAsset;
        public EActorInputSource ActorInputSource;
        public string AIAsset;
        public float MoveSpeed;
        public int Team;
        public Vector2Int ColliderSize;
        public Vector2Int ColliderOffset;
        public float MoveHesitationTime;
        public Vector2Int BaseInteractOffset;
        public float Health;
        public float ImpactResistance;
        public float CrushResistance;
        public float SlashResistance;
        public float PierceResistance;
        public float BurnResistance;
        public float FreezeResistance;
        public float ShockResistance;
        public float PoisonResitance;
        public float BaseAgility;
        public float BaseCharisma;
        public float BaseInteligence;
        public float BaseStrength;
        public float AggroRange;
        public float ChaseRange;
        public string DefaultAttack;
    }

    [Serializable]
    public struct AttackModel
    {
        public enum EDamageType
        {
            Impact,
            Crush,
            Slash,
            Pierce,
            Burn,
            Freeze,
            Shock,
            Toxic,
        }

        public enum EStatusAilment
        {
            Bloodied,
            Burned,
            Frozen,
            Stunned,
            Poisoned,
        }

        public string Name;
        public EDamageType DamageType;
        public float Damage;
        public float BloodiedChance;
        public float BurnedChance;
        public float FrozenChance;
        public float StunnedChance;
        public float PoisonedChance;
        public float Difficulty;
    }
}