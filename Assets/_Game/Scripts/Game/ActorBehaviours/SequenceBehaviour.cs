using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class SequenceBehaviour : ActorBehaviour
    {
        public List<ActorBehaviour> _sequence;

        private int _sequenceIndex;

        protected override void OnEnable()
        {
            base.OnEnable();

            _sequenceIndex = -1; // set to -1 so Next() goes to 0
            Next();
        }

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            foreach (ActorBehaviour ab in _sequence)
            {
                ab.Initialize(actor);
            }
        }

        public override bool CheckComplete()
        {
            int startIndex = _sequenceIndex;
            for (int i = 0; i < _sequence.Count; i++)
            {
                int index = (startIndex + i) % _sequence.Count;
                if (_sequence[index].CheckComplete())
                {
                    Next();
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public override bool CheckFailed()
        {
            return _sequence[_sequenceIndex].CheckFailed();
        }

        public override void PollInput(ref ActorInput input)
        {
            _sequence[_sequenceIndex].PollInput(ref input);
        }

        public void Next()
        {
            _sequenceIndex++;
            _sequenceIndex %= _sequence.Count;

            for (int i = 0; i < _sequence.Count; i++)
            {
                Debug.Log(i);
                if (!_sequence[i].enabled && _sequenceIndex == i)
                {
                    _sequence[i].enabled = true;
                }
                else if (_sequence[i].enabled && _sequenceIndex != i)
                {
                    _sequence[i].enabled = false;
                }
            }
        }
    }
}