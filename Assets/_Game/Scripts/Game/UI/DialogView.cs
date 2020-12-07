using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Tofunaut.TofuRPG.UI;
using Tofunaut.TofuUnity;
using Tofunaut.TofuUnity.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuRPG.Game.UI
{
    public class EnqueueDialogEvent : IBlackboardEvent
    {
        public readonly string Dialog;

        public EnqueueDialogEvent(string dialog)
        {
            Dialog = dialog;
        }
    }

    public class DialogView : ViewController
    {
        [Header("Dialog")]
        public TextMeshProUGUI text;

        private Queue<string> _dialogs;
        private int _currentPageIndex;
        private int _currentCharIndex;

        private void Awake()
        {
            _dialogs = new Queue<string>();
        }

        private void Start()
        {
            if (InGameStateController.Blackboard != null)
                InGameStateController.Blackboard.Subscribe<EnqueueDialogEvent>(OnEnqueueDialog);
        }

        private void OnDestroy()
        {
            if (InGameStateController.Blackboard != null)
                InGameStateController.Blackboard.Unsubscribe<EnqueueDialogEvent>(OnEnqueueDialog);
        }

        private void Next()
        {
            if (_dialogs.Count <= 0)
            {
                ViewControllerStack.Pop(this);
                return;
            }

            text.text = _dialogs.Dequeue();
            _currentPageIndex = 0;
            _currentCharIndex = 0;
        }

        protected override void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.performed)
                Next();
        }

        private void OnEnqueueDialog(EnqueueDialogEvent e)
        {
            if (e.Dialog == null)
                return;

            if (_dialogs.Count <= 0)
                ViewControllerStack.Push(this);

            _dialogs.Enqueue(e.Dialog);
            Next();
        }
    }
}