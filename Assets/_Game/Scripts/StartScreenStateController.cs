using Tofunaut.TofuRPG.UI;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.Events;

namespace Tofunaut.TofuRPG
{
    public class StartScreenStateController : AppStateController<StartScreenStateController>
    {
        [Header("View Controllers")] 
        public StartScreenRootView startScreenRootView;
        
        private void Start()
        {
            IsReady = true;
            ViewControllerStack.Push(startScreenRootView);
        }
    }
}