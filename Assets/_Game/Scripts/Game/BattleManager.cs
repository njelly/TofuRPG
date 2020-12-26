using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class BattleManager : SingletonBehaviour<BattleManager>
    {
        public static Battle Battle => _instance._battle;
        
        private Battle _battle;
    }
}