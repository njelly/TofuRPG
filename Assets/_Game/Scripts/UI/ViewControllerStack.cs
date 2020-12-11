using System.Collections.Generic;
using System.Linq;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuRPG.UI
{
    public class ViewControllerStack : SingletonBehaviour<ViewControllerStack>
    {
        public static int Count => _instance._stack.Count;
        public static PlayerInput PlayerInput => _instance.playerInput;
        
        protected override bool SetDontDestroyOnLoad => false;

        public Canvas canvas;
        public PlayerInput playerInput;
        
        private Stack<ViewController> _stack;
        private string _prevActionMap;

        protected override void Awake()
        {
            base.Awake();
            
            _stack = new Stack<ViewController>();
        }

        public static async void Push(ViewController vc)
        {
            if (!vc)
            {
                Debug.LogError("tried to push null ViewController");
                return;
            }

            if (_instance._stack.Count <= 0)
            {
                _instance._prevActionMap = _instance.playerInput.currentActionMap.name;
                _instance.playerInput.SwitchCurrentActionMap("UI");
            }
            else
                _instance._stack.Peek().StopListeningForInput(_instance.playerInput);

            vc.transform.SetParent(_instance.canvas.transform);
            vc.transform.SetAsLastSibling();
            vc.StartListeningForInput(_instance.playerInput);
            _instance._stack.Push(vc);
            
            await vc.Show();
        }

        public static async void Pop(ViewController vc)
        {
            if (Count <= 0)
                return;
            
            if (vc == _instance._stack.Peek())
            {
                _instance._stack.Pop();
                vc.StopListeningForInput(_instance.playerInput);
                await vc.Hide();
            }
            else
            {
                var stackList = _instance._stack.ToList();
                stackList.Remove(vc);
                stackList.Reverse();
                _instance._stack.Clear();
                foreach (var viewController in stackList)
                    _instance._stack.Push(viewController);
            }
            
            if(_instance._stack.Count <= 0)
                _instance.playerInput.SwitchCurrentActionMap(_instance._prevActionMap);
            else
            {
                _instance._stack.Peek().transform.SetAsLastSibling();
                _instance._stack.Peek().StartListeningForInput(_instance.playerInput);
            }
        }
    }
}