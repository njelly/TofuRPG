using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuRPG.UI
{
    public abstract class ViewController : MonoBehaviour
    {
        public bool IsShowing { get; private set; }

        public virtual async Task OnShow()
        {
            IsShowing = true;
            await Task.CompletedTask;
        }

        public virtual async Task OnHide()
        {
            IsShowing = false;
            await Task.CompletedTask;
        }

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