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
        public Interactor Interactor;
        public Color UnPressedColor;
        public Color PressedColor;
        private IActorInputProvider _actorInputProvider;
        private SpriteRenderer _spriteRenderer;
        private Vector2Int _prevInteractOffset;
        private TofuAnimator.Sequence _pressColorSequence;
        private TofuAnimator.Sequence _moveReticleSequence;

        private void Awake()
        {
            var interactorComponents = Interactor.gameObject.GetComponents<MonoBehaviour>();
            _actorInputProvider = interactorComponents.OfType<IActorInputProvider>().FirstOrDefault();
            _spriteRenderer.color = UnPressedColor;
            _prevInteractOffset = Interactor.InteractOffset;
        }

        private void Update()
        {
            var actorInput = _actorInputProvider.ActorInput;
            
        }
    }
}