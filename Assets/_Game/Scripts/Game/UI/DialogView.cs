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
    
    public class ShowDialogEvent : IBlackboardEvent { }
    public class HideDialogEvent : IBlackboardEvent { }

    public class DialogView : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public CanvasGroup canvasGroup;
        public Image nextPageIcon;
        public float fadeInTime;

        private ActorInput _actorInput;
        private bool _isShowing;
        private Queue<string> _dialogs;
        private int _currentCharIndex;
        private TofuAnimator.Sequence _typewriterSequence;
        private float _timeShown;

        private void Start()
        {
            _dialogs = new Queue<string>();
            _actorInput = new ActorInput();

            InGameStateController.Blackboard?.Subscribe<EnqueueDialogEvent>(OnEnqueueDialog);
        }

        private void Update()
        {
            ProcessActorInput();
        }

        private void OnDestroy()
        {
            InGameStateController.Blackboard?.Unsubscribe<EnqueueDialogEvent>(OnEnqueueDialog);
        }

        private void ProcessActorInput()
        {
            if (Time.time.IsApproximately(_timeShown))
                return;
            
            if(_actorInput.interact.WasPressed)
                CompletePage();
        }

        private void Show()
        {
            _isShowing = true;
            _timeShown = Time.time;
            canvasGroup.DOFade(1f, fadeInTime);

            StartNextDialog();
            
            InGameStateController.Blackboard?.Invoke(new ShowDialogEvent());
            //_actorInput = PlayerActorInputManager.InstanceActorInput;
        }

        private void Hide()
        {
            _isShowing = false;
            canvasGroup.DOFade(0f, fadeInTime);
            
            InGameStateController.Blackboard?.Invoke(new HideDialogEvent());
            _actorInput = new ActorInput();
        }

        private void StartNextDialog()
        {
            if (_dialogs.Count <= 0)
            {
                Hide();
                return;
            }
            
            text.text = _dialogs.Dequeue();
            text.ForceMeshUpdate();
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
    }
}