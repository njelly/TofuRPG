using System.Collections.Generic;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuRPG.Game
{
    public class PlayerActorInputManager : SingletonBehaviour<PlayerActorInputManager>, IActorInputProvider
    {
        public static ActorInput InstanceActorInput => _instance.ActorInput;

        public ActorInput ActorInput { get; private set; }

        public AssetReference inputAssetReference;

        private HashSet<IActorInputReceiver> _receivers;
        private HashSet<IActorInputReceiver> _toAdd;
        private HashSet<IActorInputReceiver> _toRemove;
        private InputActionAsset _inputActionAsset;
        private InputAction _moveAction;
        private InputAction _interactAction;

        protected override void Awake()
        {
            base.Awake();
            
            ActorInput = new ActorInput();
            _receivers = new HashSet<IActorInputReceiver>();
            _toAdd = new HashSet<IActorInputReceiver>();
            _toRemove = new HashSet<IActorInputReceiver>();
        }

        private async void Start()
        {
            _inputActionAsset = await Addressables.LoadAssetAsync<InputActionAsset>(inputAssetReference).Task;
            _inputActionAsset.Enable();

            _moveAction = _inputActionAsset.FindAction("Move");
            _moveAction.started += MoveAction;
            _moveAction.performed += MoveAction;
            _moveAction.canceled += MoveAction;

            _interactAction = _inputActionAsset.FindAction("Interact");
            _interactAction.started += InteractAction;
            _interactAction.performed += InteractAction;
            _interactAction.canceled += InteractAction;
        }

        protected override void OnDestroy()
        {
            _moveAction.started -= MoveAction;
            _moveAction.performed -= MoveAction;
            _moveAction.canceled -= MoveAction;
            
            _interactAction.started -= InteractAction;
            _interactAction.performed -= InteractAction;
            _interactAction.canceled -= InteractAction;
        }

        private void MoveAction(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
                ActorInput.direction.SetAxis(context.ReadValue<Vector2>());
            if (context.canceled)
                ActorInput.direction.SetAxis(Vector2.zero);
        }

        private void InteractAction(InputAction.CallbackContext context)
        {
            if (context.started)
                ActorInput.interact.Press();
            if (context.canceled)
                ActorInput.interact.Release();
        }
    }
}