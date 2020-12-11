using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuRPG.UI
{
    public abstract class ViewController : MonoBehaviour
    {
        public bool IsShowing { get; private set; }

        protected virtual float CanvasFadeInTime => 0.1f;
        
        [Header("ViewController")]
        public CanvasGroup canvasGroup;

        public async Task Show()
        {
            IsShowing = true;
            canvasGroup.interactable = true;
            
            OnShow();
            
            await canvasGroup.DOFade(1f, CanvasFadeInTime).AsyncWaitForCompletion();
        }

        public async Task Hide()
        {
            IsShowing = false;
            canvasGroup.interactable = false;
            
            OnHide();
            
            await canvasGroup.DOFade(0f, CanvasFadeInTime).AsyncWaitForCompletion();
        }

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }

        public void StartListeningForInput(PlayerInput playerInput)
        {
            playerInput.actions["UI/Submit"].started += OnSubmit;
            playerInput.actions["UI/Submit"].performed += OnSubmit;
            playerInput.actions["UI/Submit"].canceled += OnSubmit;
            playerInput.actions["UI/Cancel"].started += OnCancel;
            playerInput.actions["UI/Cancel"].performed += OnCancel;
            playerInput.actions["UI/Cancel"].canceled += OnCancel;
        }

        public void StopListeningForInput(PlayerInput playerInput)
        {
            playerInput.actions["UI/Submit"].started -= OnSubmit;
            playerInput.actions["UI/Submit"].performed -= OnSubmit;
            playerInput.actions["UI/Submit"].canceled -= OnSubmit;
            playerInput.actions["UI/Cancel"].started -= OnCancel;
            playerInput.actions["UI/Cancel"].performed -= OnCancel;
            playerInput.actions["UI/Cancel"].canceled -= OnCancel;
        }

        protected virtual void OnSubmit(InputAction.CallbackContext context) { }
        
        protected virtual void OnCancel(InputAction.CallbackContext context) { }
    }
}