using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class Actor : MonoBehaviour
    {
        public ActorInput Input { get; private set; }
        public bool IsReceivingInput { get; private set; }

        [SerializeField] private bool _recieveOnStart;

        private void Start()
        {
            if (_recieveOnStart)
            {
                BeginReceivingInput();
            }
        }

        private void OnDestroy()
        {
            StopReceivingInput();
        }

        public void BeginReceivingInput()
        {
            ActorInputManager.AddTarget(this);
            IsReceivingInput = true;
        }

        public void StopReceivingInput()
        {
            ActorInputManager.RemoveTarget(this);
            IsReceivingInput = false;
            Input = null;
        }

        public void ReceiveInput(ActorInput input)
        {
            Input = input;
        }
    }
}