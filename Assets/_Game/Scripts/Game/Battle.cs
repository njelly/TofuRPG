using System.Collections.Generic;
using Tofunaut.TofuUnity.Interfaces;

namespace Tofunaut.TofuRPG.Game
{
    public class NextBattleTurnEvent : IBlackboardEvent
    {
        public readonly Combatant Combatant;

        public NextBattleTurnEvent(Combatant combatant)
        {
            Combatant = combatant;
        }
    }
    
    public class Battle
    {
        public Combatant CurrentCombatant => _combatants[_initiativeIndex];
        
        private List<Combatant> _combatants;

        private int _initiativeIndex;

        public Battle(List<Combatant> combatants)
        {
            // when starting the battle, sort the combatants by their agility first
            combatants.Sort((x, y) => x.Actor.Agility.CompareTo(y.Actor.Agility));

            _initiativeIndex = 0;
        }

        public void Add(Combatant combatant)
        {
            _combatants.Add(combatant);
        }

        public void Next()
        {
            _initiativeIndex++;
            if (_initiativeIndex >= _combatants.Count)
                _initiativeIndex = 0;
            
            InGameStateController.Blackboard.Invoke(new NextBattleTurnEvent(CurrentCombatant));
        }
    }
}