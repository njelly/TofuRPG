using System;
using System.Collections.Generic;
using TMPro;
using Tofunaut.TofuUnity;
using Tofunaut.TofuUnity.Interfaces;
using UnityEngine;

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
    
    [Serializable]
    public class Dialog
    {
        [TextArea] public string[] pages;
    }
    
    public class DialogView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private GameObject _rootView;
        [SerializeField] private float _showNextCharDelay;

        private Queue<Dialog> _dialogs;
        private Dialog _currentDialog;
        private int _currentPageIndex;
        private int _currentCharIndex;

        private void Start()
        {
            if(InGameStateController.Blackboard != null)
                InGameStateController.Blackboard.Subscribe<EnqueueDialogEvent>(OnEnqueueDialog);
        }

        private void OnDestroy()
        {
            if(InGameStateController.Blackboard != null)
                InGameStateController.Blackboard.Unsubscribe<EnqueueDialogEvent>(OnEnqueueDialog);
        }

        public bool Next()
        {
            if (_dialogs.Count <= 0)
            {
                _rootView.SetActive(false);
                return false;
            }

            _currentDialog = _dialogs.Dequeue();
            _currentPageIndex = 0;
            _currentCharIndex = 0;
            
            if(!_rootView.activeInHierarchy)
                _rootView.SetActive(true);

            return true;
        }

        private void OnEnqueueDialog(EnqueueDialogEvent e)
        {
            if (e.Dialog == null)
                return;
            
            _dialogs.Enqueue(e.Dialog);
        }
    }
}