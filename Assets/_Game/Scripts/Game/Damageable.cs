using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Damageable : MonoBehaviour
    {
        public const float MAX_HEALTH = 100f;

        public float health = MAX_HEALTH;
        public bool destroyAtZeroHP = true;


    }
}