using System.Collections.Generic;
using TMPro;
using Tofunaut.TofuUnity;
using Tofunaut.TofuUnity.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Tofunaut.TofuRPG.Game.Interfaces;

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

    public class DialogView : MonoBehaviour, IActorInputReceiver
    {
        public TextMeshProUGUI text;
        public CanvasGroup canvasGroup;
        public Image nextPageIcon;
        public float fadeInTime;

        private bool _isShowing;
        private Queue<string> _dialogs;
        private int _currentCharIndex;
        private TofuAnimator.Sequence _typewriterSequence;

        private void Start()
        {
            _dialogs = new Queue<string>();

            if (InGameStateController.Blackboard != null)
                InGameStateController.Blackboard.Subscribe<EnqueueDialogEvent>(OnEnqueueDialog);
        }

        private void OnDestroy()
        {
            if (InGameStateController.Blackboard != null)
                InGameStateController.Blackboard.Unsubscribe<EnqueueDialogEvent>(OnEnqueueDialog);
        }

        private void Show()
        {
            _isShowing = true;
            canvasGroup.DOFade(1f, fadeInTime);
            
            PlayerActorInputManager.Add(this);

            StartNextDialog();
        }

        private void Hide()
        {
            _isShowing = false;
            canvasGroup.DOFade(0f, fadeInTime);
            
            PlayerActorInputManager.Remove(this);
        }

        private void StartNextDialog()
        {
            if (_dialogs.Count <= 0)
            {
                Hide();
                return;
            }
            
            text.text = _dialogs.Dequeue();
            text.pageToDisplay = 0; // start at 0 because pages start at 1, and next page will increment to 1
            StartNextPage();
        }

        private void StartNextPage()
        {
            text.pageToDisplay++;
            if (text.pageToDisplay > text.textInfo.pageCount)
            {
                StartNextDialog();
                return;
            }
            
            nextPageIcon.gameObject.SetActive(text.pageToDisplay <= text.textInfo.pageCount - 1);
        }

        private void CompletePage()
        {
            StartNextPage();
        }

        private void OnEnqueueDialog(EnqueueDialogEvent e)
        {
            if (e.Dialog == null)
                return;
            
            _dialogs.Enqueue(e.Dialog);
            
            if (!_isShowing)
                Show();
        }

        public void ReceiveActorInput(ActorInput actorInput)
        {
            if(actorInput.interact.WasPressed)
                CompletePage();
        }
    }
}