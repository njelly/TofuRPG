using System;
using System.Collections.Generic;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class DamageMatrix : SingletonBehaviour<DamageMatrix>
    {
        public enum EDamageTarget
        {
            Invalid = 0,
            Flesh = 1,
        }

        public enum EDamageAttribute
        {
            None = 0,
            Blunt = 1 << 0,
            Slash = 1 << 2,
            Pierce = 1 << 3,
            Fire = 1 << 4,
            Ice = 1 << 5,
            Poison = 1 << 6,
            Acid = 1 << 7,
        }

        [Serializable]
        public struct DamagePairing
        {
            public EDamageTarget target;
            public EDamageAttribute attribute;
            public float multiplier;
        }

        [SerializeField] private List<DamagePairing> _damagePairings = new List<DamagePairing>();

        private float[,] _targetToAttributeToMultiplier;

        protected override void Awake()
        {
            base.Awake();

            _targetToAttributeToMultiplier = new float[Enum.GetValues(typeof(EDamageTarget)).Length, Enum.GetValues(typeof(EDamageAttribute)).Length];
            for (int i = 0; i < _targetToAttributeToMultiplier.GetLength(0); i++)
            {
                for (int j = 0; j < _targetToAttributeToMultiplier.GetLength(1); j++)
                {
                    _targetToAttributeToMultiplier[i, j] = 1f;
                }
            }

            foreach (DamagePairing pairing in _damagePairings)
            {
                int targetTypeIndex = (int)pairing.target;
                int attributeTypeIndex = (int)pairing.attribute;

                _targetToAttributeToMultiplier[targetTypeIndex, attributeTypeIndex] = pairing.multiplier;
            }
        }

        public static float GetDamageAmount(float baseAmount, EDamageTarget target, EDamageAttribute damageAttribute)
        {
            if (!_instance)
            {
                return baseAmount;
            }

            return baseAmount * _instance._targetToAttributeToMultiplier[(int)target, (int)damageAttribute];
        }
    }
}