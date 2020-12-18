using System.Collections.Generic;
using Tofunaut.TofuRPG.Game.Interfaces;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game.AI
{
    public class EnemyBrain : NPCBrain
    {
        public enum State
        {
            Wandering,
            Pursuing,
        }
        
        [Header("Enemy Brain")]
        public WanderBrain wanderBrainAsset;

        private State _state;
        private WanderBrain _wanderBrain;

        public override void Initialize(GameObject actor)
        {
            base.Initialize(actor);

            _state = State.Wandering;
            _wanderBrain = Instantiate(wanderBrainAsset);
            _wanderBrain.Initialize(actor);
        }

        public override void Update()
        {
            base.Update();
            switch (_state)
            {
                case State.Wandering:
                    _wanderBrain.Update();
                    break;
                case State.Pursuing:
                    UpdatePursuing();
                    break;
            }
        }

        private void UpdateWandering()
        {
            _wanderBrain.Update();
        }

        private void UpdatePursuing()
        {
            
        }

        protected override Queue<Vector2Int> ChoosePath()
        {
            throw new System.NotImplementedException();
        }

        protected override IInteractable ChooseTargetInteractable()
        {
            throw new System.NotImplementedException();
        }
    }
}