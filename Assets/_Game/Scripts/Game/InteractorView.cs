using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class InteractorView : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public Interactor interactor;
        public Color interactingColor;
        public Color notInteractingColor;

        private Vector2Int _prevOffset;
        private bool _wasInteracting;

        private void Start()
        {
            SetOffset(interactor.InteractOffset);
            SetInteracting(interactor.InteractingWith != null);
        }

        private void Update()
        {
            if(interactor.InteractOffset != _prevOffset)
            {
                SetOffset(interactor.InteractOffset);
            }

            bool isInteracting = interactor.InteractingWith != null;
            if(isInteracting != _wasInteracting)
            {
                SetInteracting(isInteracting);
            }
        }

        private void SetOffset(Vector2Int newOffset)
        {
            transform.localPosition = (Vector2)newOffset;
            _prevOffset = newOffset;
        }

        private void SetInteracting(bool isInteracting)
        {
            if(isInteracting)
            {
                spriteRenderer.color = interactingColor;
            }
            else
            {
                spriteRenderer.color = notInteractingColor;
            }

            _wasInteracting = isInteracting;
        }
    }
}