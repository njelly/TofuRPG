using System;
using System.Threading.Tasks;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuRPG.Game.UI;
using Tofunaut.TofuRPG.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuRPG.Game
{
    public class PlayerActorInputProvider : ActorComponent, IActorInputProvider
    {
        public ActorInput ActorInput { get; private set; }

        private InputActionAsset _inputActionAsset;

        private void Awake()
        {
            ActorInput = new ActorInput();
        }

        public async void Start()
        {
            while (!ViewControllerStack.PlayerInput)
                await Task.Yield();
            
            GameCameraManager.SetTarget(transform);
            
            ViewControllerStack.PlayerInput.actions["Player/Move"].started += OnMove;
            ViewControllerStack.PlayerInput.actions["Player/Move"].performed += OnMove;
            ViewControllerStack.PlayerInput.actions["Player/Move"].canceled += OnMove;
            ViewControllerStack.PlayerInput.actions["Player/Interact"].started += OnInteract;
            ViewControllerStack.PlayerInput.actions["Player/Interact"].performed += OnInteract;
            ViewControllerStack.PlayerInput.actions["Player/Interact"].canceled += OnInteract;
        }

        private void Update()
        {
            if (ViewControllerStack.PlayerInput && !ViewControllerStack.PlayerInput.currentActionMap.name.Equals("Player"))
                ActorInput.Reset();
        }

        private void OnDestroy()
        {
            if (ViewControllerStack.PlayerInput == null)
                return;
            
            ViewControllerStack.PlayerInput.actions["Player/Move"].started -= OnMove;
            ViewControllerStack.PlayerInput.actions["Player/Move"].performed -= OnMove;
            ViewControllerStack.PlayerInput.actions["Player/Move"].canceled -= OnMove;
            ViewControllerStack.PlayerInput.actions["Player/Interact"].started -= OnInteract;
            ViewControllerStack.PlayerInput.actions["Player/Interact"].performed -= OnInteract;
            ViewControllerStack.PlayerInput.actions["Player/Interact"].canceled -= OnInteract;
        }

        public override void Initialize(Actor actor, ActorModel model) { }

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