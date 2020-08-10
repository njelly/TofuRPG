using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class WaitBehaviour : ActorBehaviour
    {
        public float waitTime;

        [Header("Random Reset")]
        public float minWaitTime = 0f;
        public float maxWaitTime = 0f;

        private float _startWaitTime;

        protected override void OnEnable()
        {
            base.OnEnable();

            _startWaitTime = Time.time;

            if (minWaitTime >= 0 && maxWaitTime > 0)
            {
                _startWaitTime = Random.Range(minWaitTime, maxWaitTime);
            }
        }

        public override bool CheckFailed()
        {
            // can't fail "just wait around"
            return false;
        }

        public override bool CheckComplete()
        {
            return Time.time - _startWaitTime > waitTime;
        }

        public override void PollInput(ref ActorInput input)
        {
            input.direction.SetDirection(Vector2Int.zero);
        }
    }
}