using System;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuRPG.Game.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuRPG.Game
{
    public class PlayerActorInputProvider : MonoBehaviour, IActorInputProvider
    {
        public ActorInput ActorInput { get; private set; }

        public AssetReference inputAssetReference;
        
        private InputActionAsset _inputActionAsset;
        private InputAction _moveAction;
        private InputAction _interactAction;

        private void Awake()
        {
            ActorInput = new ActorInput();
        }

        public async void Start()
        {
            _inputActionAsset = await Addressables.LoadAssetAsync<InputActionAsset>(inputAssetReference).Task;
            _moveAction = _inputActionAsset.FindAction("Move");
            _interactAction = _inputActionAsset.FindAction("Interact");
            _inputActionAsset.Enable();
            
            SubscribeToInputActions();
        }

        private void OnDestroy()
        {
            UnsubscribeToInputActions();
        }

        private void SubscribeToInputActions()
        {
            _moveAction.started += MoveAction;
            _moveAction.performed += MoveAction;
            _moveAction.canceled += MoveAction;
            
            _interactAction.started += InteractAction;
            _interactAction.performed += InteractAction;
            _interactAction.canceled += InteractAction;
        }

        private void UnsubscribeToInputActions()
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
                ActorInput.Direction.SetAxis(context.ReadValue<Vector2>());
            if (context.canceled)
                ActorInput.Direction.SetAxis(Vector2.zero);
        }

        private void InteractAction(InputAction.CallbackContext context)
        {
            if (context.started)
                ActorInput.Interact.Press();
            if (context.canceled)
                ActorInput.Interact.Release();
        }
    }
}