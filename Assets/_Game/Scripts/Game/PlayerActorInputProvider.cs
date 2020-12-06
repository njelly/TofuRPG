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
        public PlayerInput playerInput;

        private InputActionAsset _inputActionAsset;

        private void Awake()
        {
            ActorInput = new ActorInput();
        }

        public async void Start()
        {
            _inputActionAsset = await Addressables.LoadAssetAsync<InputActionAsset>(inputAssetReference).Task;
            _inputActionAsset.Enable();

            if (!playerInput)
                playerInput = FindObjectOfType<PlayerInput>();
            
            playerInput.actions["Player/Move"].started += OnMove;
            playerInput.actions["Player/Move"].performed += OnMove;
            playerInput.actions["Player/Move"].canceled += OnMove;
            playerInput.actions["Player/Interact"].started += OnInteract;
            playerInput.actions["Player/Interact"].performed += OnInteract;
            playerInput.actions["Player/Interact"].canceled += OnInteract;
        }

        private void Update()
        {
            if (playerInput && !playerInput.currentActionMap.name.Equals("Player"))
                ActorInput.Reset();
        }

        private void OnDestroy()
        {
            if (!playerInput)
                return;
            
            playerInput.actions["Player/Move"].started -= OnMove;
            playerInput.actions["Player/Move"].performed -= OnMove;
            playerInput.actions["Player/Move"].canceled -= OnMove;
            playerInput.actions["Player/Interact"].started -= OnInteract;
            playerInput.actions["Player/Interact"].performed -= OnInteract;
            playerInput.actions["Player/Interact"].canceled -= OnInteract;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
                ActorInput.Direction.SetAxis(context.ReadValue<Vector2>());
            if (context.canceled)
                ActorInput.Direction.SetAxis(Vector2.zero);
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
                ActorInput.Interact.Press();
            if (context.canceled)
                ActorInput.Interact.Release();
        }
    }
}