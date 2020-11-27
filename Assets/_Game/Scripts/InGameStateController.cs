using System;
using Tofunaut.TofuUnity;

namespace Tofunaut.TofuRPG
{
    public class InGameStateController : AppStateController<InGameStateController>
    {
        public static Blackboard Blackboard => _instance ? _instance._blackboard : null;

        private Blackboard _blackboard;

        protected override void Awake()
        {
            base.Awake();
            
            _blackboard = new Blackboard();
        }

        private void Start()
        {
            IsReady = true;
        }
    }
}