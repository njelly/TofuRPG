using System;
using System.Linq;
using DG.Tweening;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class InteractorReticle : MonoBehaviour
    {
        public Interactor interactor;
        public Color unPressedColor;
        public Color pressedColor;
        public float lerpPositionTime;
        public float lerpPressColorTime;

        private IActorInputProvider _actorInputProvider;
        private SpriteRenderer _spriteRenderer;
        private Vector2Int _prevInteractOffset;
        private float _lerpAngle;
        private Color _lerpColor;
        private Transform _t;
        private bool _wasPressed;

        private void Awake()
        {
            var interactorComponents = interactor.gameObject.GetComponents<MonoBehaviour>();
            _actorInputProvider = interactorComponents.OfType<IActorInputProvider>().FirstOrDefault();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.color = unPressedColor;
            _prevInteractOffset = interactor.InteractOffset;
            _lerpAngle = Vector2.SignedAngle(Vector2.right, _prevInteractOffset.ToVector2()) * Mathf.Deg2Rad;
            _lerpColor = unPressedColor;
            _t = transform;
        }

        private void Update()
        {
            if (_prevInteractOffset != interactor.InteractOffset)
                UpdatePosition();

            var actorInput = _actorInputProvider.ActorInput;
            if(actorInput.Interact.WasPressed)
                UpdateColor(true);
            else if(actorInput.Interact.WasReleased)
                UpdateColor(false);
        }

        private void UpdatePosition()
        {
            var magnitude = interactor.InteractOffset.magnitude;
            var from = _lerpAngle * Mathf.Rad2Deg;
            var absoluteTo = Vector2.SignedAngle(Vector2.right, interactor.InteractOffset.ToVector2());
            var shortestDiff = MathfUtils.SmallestAngleDifferenceDeg(from, absoluteTo);
            var to = from + shortestDiff;

            DOTween.To(() => from, newValue =>
            {
                from = newValue;
                _lerpAngle = newValue * Mathf.Deg2Rad;
                _t.localPosition = new Vector2(Mathf.Cos(_lerpAngle), Mathf.Sin(_lerpAngle) * magnitude);
            }, to, lerpPositionTime);

            _prevInteractOffset = interactor.InteractOffset;
        }

        private void UpdateColor(bool wasPressed)
        {
            if (_wasPressed == wasPressed)
                return;
            
            _wasPressed = wasPressed;
            var from = _lerpColor;
            var to = wasPressed ? pressedColor : unPressedColor;
            _spriteRenderer.DOColor(to, lerpPressColorTime);
        }
    }
}