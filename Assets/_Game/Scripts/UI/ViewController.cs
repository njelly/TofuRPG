﻿using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuRPG.UI
{
    public abstract class ViewController : MonoBehaviour
    {
        public bool IsShowing { get; private set; }
        
        [Header("ViewController")]
        public CanvasGroup canvasGroup;
        public float canvasFadeInTime;

        public virtual async Task OnShow()
        {
            IsShowing = true;
            canvasGroup.interactable = true;
            await canvasGroup.DOFade(1f, canvasFadeInTime).AsyncWaitForCompletion();
        }

        public virtual async Task OnHide()
        {
            IsShowing = false;
            canvasGroup.interactable = false;
            await canvasGroup.DOFade(0f, canvasFadeInTime).AsyncWaitForCompletion();
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