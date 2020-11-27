using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuRPG.UI
{
    public class UIManager : MonoBehaviour
    {
        public const string PlayerActionMap = "Player";
        public const string UIActionMap = "UI";
        
        public ViewController Current => _viewControllers.Peek();

        public PlayerInput playerInput;
        public Canvas rootCanvas;
        
        private Stack<ViewController> _viewControllers;

        private void Awake()
        {
            _viewControllers = new Stack<ViewController>();
        }

        public async void Push(ViewController prefab)
        {
            var viewController = Instantiate(prefab, rootCanvas.transform);
            playerInput.SwitchCurrentActionMap(UIActionMap);
            
            await viewController.Show();
            
            _viewControllers.Push(viewController);
        }

        public async void Pop()
        {
            await Current.Hide();
            Destroy(_viewControllers.Pop().gameObject);
            
            if(_viewControllers.Count <= 0)
                playerInput.SwitchCurrentActionMap(PlayerActionMap);
        }
    }
}