using System;
using System.Collections.Generic;
using TMPro;
using Tofunaut.TofuUnity;
using Tofunaut.TofuUnity.Interfaces;
using UnityEngine;
using UnityEngine.UI;

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
    
    public class DialogView : MonoBehaviour
    {
        public bool IsShowing => rootView.gameObject.activeInHierarchy;
        
        public TextMeshProUGUI text;
        public GameObject rootView;
        public CanvasGroup canvasGroup;
        public Image nextPageIcon;
        public float fadeInTime;
        public float showNextCharDelay;

        private bool _isShowing;
        private Queue<string> _dialogs;
        private string _currentDialog;
        private int _currentDialogPageIndex;
        private int _currentTMPPageCharIndex;
        private int _currentTMPPageIndex;
        private int _currentCharIndex;
        private TofuAnimator.Sequence _fadeSequence;
        private TofuAnimator.Sequence _typewriterSequence;

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

        private void Update()
        {
            if (!_isShowing)
                return;

            //nextPageIcon.gameObject.SetActive(_currentTMPPageCharIndex == text.textInfo.pageInfo[_currentTMPPageIndex + 1]);
        }

        private void Show()
        {
            _isShowing = true;
            _fadeSequence?.Stop();
            _fadeSequence = gameObject.Sequence()
                .Curve(TofuAnimator.EEaseType.Linear, fadeInTime, newValue =>
                {
                    canvasGroup.alpha = Mathf.LerpUnclamped(0f, 1f, newValue);
                })
                .Then()
                .Execute(() =>
                {
                    _fadeSequence = null;
                });
            _fadeSequence.Play();
        }

        private void Hide()
        {
            _isShowing = false;
            _fadeSequence?.Stop();
            _fadeSequence = gameObject.Sequence()
                .Curve(TofuAnimator.EEaseType.Linear, fadeInTime, newValue =>
                {
                    canvasGroup.alpha = Mathf.LerpUnclamped(1f, 0f, newValue);
                })
                .Then()
                .Execute(() =>
                {
                    _fadeSequence = null;
                });
            _fadeSequence.Play();
        }

        private bool NextDialog()
        {
            if (_dialogs.Count <= 0 && IsShowing)
            {
                Hide();
                return false;
            }

            _currentDialog = _dialogs.Dequeue();
            _currentTMPPageCharIndex = 0;
            _currentTMPPageIndex = 0;

            if (!IsShowing)
                Show();

            return true;
        }

        private void OnEnqueueDialog(EnqueueDialogEvent e)
        {
            if (e.Dialog == null)
                return;
            
            _dialogs.Enqueue(e.Dialog);
        }

        //private string GetStringForIndex(int index)
        //{
        //    
        //}
    }
}