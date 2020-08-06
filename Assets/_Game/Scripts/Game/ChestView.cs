using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class ChestView : MonoBehaviour
    {
        [SerializeField] private Chest _chest;
        [SerializeField] private Animator _animator;

        private bool _isOpen;

        private void Update()
        {
            if (_chest.Open != _isOpen)
            {
                _isOpen = _chest.Open;
                _animator.SetBool("Open", _isOpen);
            }
        }
    }
}