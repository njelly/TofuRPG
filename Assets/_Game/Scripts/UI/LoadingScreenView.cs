using System;
using DG.Tweening;
using TMPro;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.UI;

namespace Tofunaut.TofuRPG.UI
{
    public class LoadingScreenView : ViewController
    {
        protected override float CanvasFadeInTime => 0f;

        [Header("Loading Screen")]
        public TextMeshProUGUI loadingLabel;
        public float loadingLabelFrameInterval;
        public Image loadingBarFill;

        private string[] _loadingLabelFrames;
        private int _currentLabelFrame;
        private float _labelFrameTimer;

        private void Awake()
        {
            const string original = "Loading...";
            _loadingLabelFrames = new[]
            {
                RichTextUtils.HideAt(original, 7),
                RichTextUtils.HideAt(original, 8),
                RichTextUtils.HideAt(original, 9),
                original,
            };
        }

        private void Update()
        {
            _labelFrameTimer -= Time.deltaTime;
            loadingLabel.text = _loadingLabelFrames[_currentLabelFrame];
            if (_labelFrameTimer <= 0)
            {
                _currentLabelFrame++;
                _currentLabelFrame %= _loadingLabelFrames.Length;
                _labelFrameTimer = loadingLabelFrameInterval;
            }
        }

        protected override void OnShow()
        {
            base.OnShow();
            _currentLabelFrame = 0;
            _labelFrameTimer = loadingLabelFrameInterval;
        }

        public void SetPercentComplete(float percent)
        {
            percent = Mathf.Clamp01(percent);
            loadingBarFill.DOFillAmount(percent, 0.1f);
        }
    }
}