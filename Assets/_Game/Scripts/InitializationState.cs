using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class InitializationState : MonoBehaviour, IInitializationState
    {
        public bool IsComplete { get; private set; }
    }
}