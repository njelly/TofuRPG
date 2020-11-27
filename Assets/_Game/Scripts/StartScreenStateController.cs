using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.Events;

namespace Tofunaut.TofuRPG
{
    public class StartScreenStateController : AppStateController<StartScreenStateController>
    {
        private void Start()
        {
            IsReady = true;
        }
    }
}