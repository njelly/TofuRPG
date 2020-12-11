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
        public readonly Dialog Dialog;

        public EnqueueDialogEvent(Dialog dialog)
        {
            Dialog = dialog;
        }
    }

    public class Dialog
    {
        public string Text;
        public Action OnDialogComplete;
    }

    public class DialogView : ViewController
    {
        [Header("Dialog")]
        public TextMeshProUGUI text;

        private Queue<Dialog> _queuedDialogs;
        private int _currentPageIndex;
        private int _currentCharIndex;
        private Dialog _currentDialog;

        private void Awake()
        {
            _queuedDialogs = new Queue<Dialog>();
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
            _currentDialog?.OnDialogComplete?.Invoke();
            if (_queuedDialogs.Count <= 0)
            {
                _currentDialog = null;
                ViewControllerStack.Pop(this);
                return;
            }

            _currentDialog = _queuedDialogs.Dequeue();
            text.text = _currentDialog.Text;
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

            if (_queuedDialogs.Count <= 0)
                ViewControllerStack.Push(this);

            _queuedDialogs.Enqueue(e.Dialog);
            Next();
        }
    }
}