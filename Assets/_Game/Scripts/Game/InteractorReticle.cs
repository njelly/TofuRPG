using System;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class InteractorReticle : ActorViewComponent
    {
        public float unPressedAlpha;
        public float pressedAlpha;
        public float lerpPositionTime;
        public float lerpPressColorTime;
        public Color neutralColor;
        public Color alliedColor;
        public Color enemyColor;

        private Interactor _interactor;
        private IActorInputProvider _actorInputProvider;
        private SpriteRenderer _spriteRenderer;
        private Vector2Int _prevInteractOffset;
        private float _lerpAngle;
        private Transform _t;
        private bool _wasPressed;
        private TweenerCore<float, float, FloatOptions> _flashTween; 
        private TweenerCore<Color, Color, ColorOptions> _colorTween;
        private Color _currentColor;

        private void Start()
        {
            _t = transform;
        }

        private void Update()
        {
            if (!IsInitialized)
                return;
            
            if (_prevInteractOffset != _interactor.InteractOffset)
                UpdatePosition();

            var actorInput = _actorInputProvider.ActorInput;
            if(actorInput.Interact.WasPressed)
                UpdatePressedColor(true);
            else if(!actorInput.Interact.Held)
                UpdatePressedColor(false);
        }

        public override void Initialize(ActorView actorView)
        {
            base.Initialize(actorView);
            
            var actorComponents = actorView.Actor.GetComponents<MonoBehaviour>();
            _interactor = actorComponents.OfType<Interactor>().FirstOrDefault();
            _actorInputProvider = actorComponents.OfType<IActorInputProvider>().FirstOrDefault();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.color = neutralColor.WithAlpha(unPressedAlpha);
            _prevInteractOffset = _interactor.InteractOffset;
            _lerpAngle = Vector2.SignedAngle(Vector2.right, _prevInteractOffset.ToVector2()) * Mathf.Deg2Rad;
            _currentColor = neutralColor;
        }

        private void UpdatePosition()
        {
            var magnitude = _interactor.InteractOffset.magnitude;
            var from = _lerpAngle * Mathf.Rad2Deg;
            var absoluteTo = Vector2.SignedAngle(Vector2.right, _interactor.InteractOffset.ToVector2());
            var shortestDiff = MathfUtils.SmallestAngleDifferenceDeg(from, absoluteTo);
            var to = from + shortestDiff;

            DOTween.To(() => from, newValue =>
            {
                from = newValue;
                _lerpAngle = from * Mathf.Deg2Rad;
                _t.localPosition = new Vector2(Mathf.Cos(_lerpAngle), Mathf.Sin(_lerpAngle) * magnitude);
            }, to, lerpPositionTime);

            _prevInteractOffset = _interactor.InteractOffset;
        }

        private async void UpdatePressedColor(bool wasPressed)
        {
            if (_wasPressed == wasPressed)
                return;
            
            _wasPressed = wasPressed;

            if (_flashTween != null && !_flashTween.IsComplete())
                await _flashTween.AsyncWaitForCompletion();

            var to = wasPressed ? pressedAlpha : unPressedAlpha;
            _flashTween = DOTween.To(
                () => _spriteRenderer.color.a, 
                x => _spriteRenderer.color = _spriteRenderer.color.WithAlpha(x), 
                to, 
                lerpPressColorTime);
        }
    }
}