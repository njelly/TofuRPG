using System;
using Tofunaut.TofuRPG.Game.Interfaces;
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

        private void OnDestroy()
        {
            _moveAction.started -= MoveAction;
            _moveAction.performed -= MoveAction;
            _moveAction.canceled -= MoveAction;
            
            _interactAction.started -= MoveAction;
            _interactAction.performed -= MoveAction;
            _interactAction.canceled -= MoveAction;
        }

        private void MoveAction(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
                ActorInput.direction = context.ReadValue<Vector2>();
            if (context.canceled)
                ActorInput.direction = Vector2.zero;
        }

        private void InteractAction(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
                ActorInput.interact = true;
            if (context.canceled)
                ActorInput.interact = false;
        }
    }
}