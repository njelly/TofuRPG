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
        public PlayerInput playerInput;
        public CanvasGroup canvasGroup;
        public float canvasFadeInTime;
        public Slider sfxVolumeSlider;
        public Slider musicVolumeSlider;

        public override async Task OnShow()
        {
            sfxVolumeSlider.value = UserSettings.SFXVolume;
            musicVolumeSlider.value = UserSettings.MusicVolume;
            
            await base.OnShow();
            await canvasGroup.DOFade(1f, canvasFadeInTime).AsyncWaitForCompletion();
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
            OnCloseClicked();
        }
    }
}