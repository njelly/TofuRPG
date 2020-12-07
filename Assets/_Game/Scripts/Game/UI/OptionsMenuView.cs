using System;
using System.Threading.Tasks;
using DG.Tweening;
using Tofunaut.TofuRPG.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Tofunaut.TofuRPG.Game.UI
{
    public class OptionsMenuView : ViewController
    {
        [Header("OptionsMenu")]
        public Slider sfxVolumeSlider;
        public Slider musicVolumeSlider;

        private InputAction _showOptionsAction;

        private void Start()
        {
            _showOptionsAction = ViewControllerStack.PlayerInput.actions["Player/ShowOptions"];
            _showOptionsAction.performed += OnShowOptions;
        }

        private void OnDestroy()
        {
            _showOptionsAction.performed -= OnShowOptions;
        }

        public override async Task OnShow()
        {
            sfxVolumeSlider.value = UserSettings.SFXVolume;
            musicVolumeSlider.value = UserSettings.MusicVolume;
            
            await base.OnShow();
        }

        public override async Task OnHide()
        {
            await base.OnHide();
            await canvasGroup.DOFade(0f, canvasFadeInTime).AsyncWaitForCompletion();
        }
        
        public void SetSFXPercent(float percent)
        {
            UserSettings.SFXVolume = percent;
        }

        public void SetMusicPercent(float percent)
        {
            UserSettings.MusicVolume = percent;
        }

        public void OnCloseClicked()
        {
            ViewControllerStack.Pop(this);
        }

        protected override void OnCancel(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnCloseClicked();
        }

        private void OnShowOptions(InputAction.CallbackContext context)
        {
            if (context.performed)
                ViewControllerStack.Push(this);
        }
    }
}