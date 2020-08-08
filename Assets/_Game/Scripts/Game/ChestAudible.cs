using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class ChestAudible : MonoBehaviour
    {
        [SerializeField] private Chest _chest;
        [SerializeField] private AudioCue _openCue;

        private bool _isOpen;

        private void Update()
        {
            if (_chest.Open != _isOpen)
            {
                _isOpen = _chest.Open;

                if (_isOpen)
                {
                    _openCue.Play(transform);
                }
            }
        }
    }
}