using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public abstract class ActorBehaviour : MonoBehaviour
    {
        public enum EState
        {
            Idle,
            InProgress,
            Complete,
            Failed,
        }

        public EState State { get; private set; }

        protected Actor _actor;

        private ActorInput _input;

        protected virtual void OnEnable()
        {
            State = EState.InProgress;
            _input = new ActorInput();
        }

        protected virtual void OnDisable()
        {
            State = EState.Idle;
        }

        protected virtual void Update()
        {
            if (!_actor)
            {
                return;
            }

            if (State != EState.InProgress)
            {
                return;
            }

            if (CheckFailed())
            {
                State = EState.Failed;
                return;
            }

            if (CheckComplete())
            {
                State = EState.Complete;
                return;
            }

            PollInput(ref _input);
            _actor.ReceiveActorInput(_input);
        }

        public virtual void Initialize(Actor actor)
        {
            _actor = actor;
        }

        public abstract bool CheckFailed();
        public abstract bool CheckComplete();
        public abstract void PollInput(ref ActorInput input);

    }
}