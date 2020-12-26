using System;
using DG.Tweening;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Combatant : ActorComponent
    {
        public Actor Actor { get; private set; }
        public string DefaultAttack { get; private set; }
        public Damageable Target { get; private set; }
        public float ChaseRange => _baseChaseRange;
        public float AggroRange => _baseAggroRange;

        private float _baseAggroRange;
        private float _baseChaseRange;

        private void Update()
        {
            if(Target)
                UpdateTarget();
            
            Target = GetBestTarget();
        }

        private void UpdateTarget()
        {
            var toTarget = (Target.transform.position - transform.position).sqrMagnitude;
            if (toTarget > Mathf.Pow(ChaseRange, 2f))
                Target = null;
        }

        private Damageable GetBestTarget()
        {
            var hits = new Collider2D[8];
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _baseAggroRange, hits, LayerMask.GetMask("Actor"));
            for (var i = 0; i < size; i++)
            {
                var hitDamageable = hits[i].GetComponent<Damageable>();
                if (hitDamageable == null)
                    continue;
                
                
            }

            // TODO: THIS FUNCTION
            return null;
        }

        public void DoAttack(string attackKey, Damageable target)
        {
            if (InGameStateController.Config == null)
            {
                Debug.LogError("cant attack, config is null");
                return;
            }

            if (!InGameStateController.Config.TryGetAttackModel(attackKey, out var attackModel))
            {
                Debug.LogError($"could not find attack with key {attackKey}");
                return;
            }
            
            var attack = new Attack(this, attackModel);
            target.TakeDamage(attack);
        }

        public override void Initialize(Actor actor, ActorModel model)
        {
            Actor = actor;
            DefaultAttack = model.DefaultAttack;
            _baseAggroRange = model.AggroRange;
        }
    }
}