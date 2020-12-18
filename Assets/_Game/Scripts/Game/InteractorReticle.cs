﻿using System;
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
    public class InteractorReticle : MonoBehaviour
    {
        public Color unPressedColor;
        public Color pressedColor;
        public float lerpPositionTime;
        public float lerpPressColorTime;

        private Interactor _interactor;
        private IActorInputProvider _actorInputProvider;
        private SpriteRenderer _spriteRenderer;
        private Vector2Int _prevInteractOffset;
        private float _lerpAngle;
        private Transform _t;
        private bool _wasPressed;
        private TweenerCore<Color, Color, ColorOptions> _flashTween; 

        private void Start()
        {
            var actor = transform.GetComponentInParent<Actor>();
            var actorComponents = actor.GetComponents<MonoBehaviour>();
            _interactor = actorComponents.OfType<Interactor>().FirstOrDefault();
            _actorInputProvider = actorComponents.OfType<IActorInputProvider>().FirstOrDefault();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.color = unPressedColor;
            _prevInteractOffset = _interactor.InteractOffset;
            _lerpAngle = Vector2.SignedAngle(Vector2.right, _prevInteractOffset.ToVector2()) * Mathf.Deg2Rad;
            _t = transform;
        }

        private void Update()
        {
            if (_prevInteractOffset != _interactor.InteractOffset)
                UpdatePosition();

            var actorInput = _actorInputProvider.ActorInput;
            if(actorInput.Interact.WasPressed)
                UpdateColor(true);
            else if(!actorInput.Interact.Held)
                UpdateColor(false);
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
                _lerpAngle = newValue * Mathf.Deg2Rad;
                _t.localPosition = new Vector2(Mathf.Cos(_lerpAngle), Mathf.Sin(_lerpAngle) * magnitude);
            }, to, lerpPositionTime);

            _prevInteractOffset = _interactor.InteractOffset;
        }

        private async void UpdateColor(bool wasPressed)
        {
            if (_wasPressed == wasPressed)
                return;
            
            _wasPressed = wasPressed;

            if (_flashTween != null && !_flashTween.IsComplete())
                await _flashTween.AsyncWaitForCompletion();
            
            var to = wasPressed ? pressedColor : unPressedColor;
            _flashTween = _spriteRenderer.DOColor(to, lerpPressColorTime);
        }
    }
}