using Tofunaut.TofuRPG.UI;
using Tofunaut.TofuUnity;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using Task = System.Threading.Tasks.Task;

namespace Tofunaut.TofuRPG
{
    public class StartScreenStateController : AppStateController<StartScreenStateController>
    {
        [Header("View Controllers")] 
        public LoadingScreenView loadingScreenView;
        public StartScreenRootView startScreenRootView;
        
        private async void Start()
        {
            ViewControllerStack.Push(loadingScreenView);

            await Task.Delay(3000);
            
            ViewControllerStack.Pop(loadingScreenView);
            IsReady = true;
            ViewControllerStack.Push(startScreenRootView);
        }
    }
}