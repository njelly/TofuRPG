using System;
using System.Linq;
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
            if(_prevInteractOffset != interactor.InteractOffset)
                UpdatePosition();
            
            var actorInput = _actorInputProvider.ActorInput;
            if(actorInput.interact.WasPressed)
                UpdateColor(true);
            else if(actorInput.interact.WasReleased)
                UpdateColor(false);
        }

        private void UpdatePosition()
        {
            var magnitude = interactor.InteractOffset.magnitude;
            var from = _lerpAngle * Mathf.Rad2Deg;
            var to = Vector2.SignedAngle(Vector2.right, interactor.InteractOffset.ToVector2());
            if (Mathf.Abs(to - from) > 180)
                if(from > 0)
                    from *= -1;
                else if (to > 0)
                    to *= -1;
            
            gameObject.Sequence()
                .Curve(TofuAnimator.EEaseType.Linear, lerpPositionTime, newValue =>
                {
                    _lerpAngle = Mathf.LerpUnclamped(from, to, newValue) * Mathf.Deg2Rad;
                    _t.localPosition = new Vector2(Mathf.Cos(_lerpAngle), Mathf.Sin(_lerpAngle) * magnitude);
                })
                .Play();
            
            _prevInteractOffset = interactor.InteractOffset;
        }

        private void UpdateColor(bool wasPressed)
        {
            var from = _lerpColor;
            var to = wasPressed ? pressedColor : unPressedColor;
            gameObject.Sequence()
                .Curve(TofuAnimator.EEaseType.Linear, lerpPressColorTime, newValue =>
                {
                    _lerpColor = Color.LerpUnclamped(from, to, newValue);
                    _spriteRenderer.color = _lerpColor;
                })
                .Play();
        }
    }
}