using System.Collections.Generic;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuRPG.Game
{
    public class PlayerActorInputManager : SingletonBehaviour<PlayerActorInputManager>
    {
        public AssetReference inputAssetReference;

        private ActorInput _actorInput;
        private HashSet<IActorInputReceiver> _receivers;
        private HashSet<IActorInputReceiver> _toAdd;
        private HashSet<IActorInputReceiver> _toRemove;
        private InputActionAsset _inputActionAsset;
        private InputAction _moveAction;
        private InputAction _interactAction;

        protected override void Awake()
        {
            base.Awake();
            
            _actorInput = new ActorInput();
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

        private void Update()
        {
            foreach (var receiver in _toAdd)
                _receivers.Add(receiver);
            _toAdd.Clear();

            foreach (var receiver in _toRemove)
                _receivers.Remove(receiver);
            _toRemove.Clear();

            foreach (var receiver in _receivers)
                receiver.ReceiveActorInput(_actorInput);
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
                _actorInput.direction.SetAxis(context.ReadValue<Vector2>());
            if (context.canceled)
                _actorInput.direction.SetAxis(Vector2.zero);
        }

        private void InteractAction(InputAction.CallbackContext context)
        {
            if (context.started)
                _actorInput.interact.Press();
            if (context.canceled)
                _actorInput.interact.Release();
        }

        public static void Add(IActorInputReceiver receiver)
        {
            _instance._toAdd.Add(receiver);
        }

        public static void Remove(IActorInputReceiver receiver)
        {
            _instance._toRemove.Add(receiver);
        }
    }
}