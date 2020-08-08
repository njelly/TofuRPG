using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class PlayAudioCue : MonoBehaviour
    {
        [SerializeField] private AudioCue _cue;
        [SerializeField] private bool _playOnEnable;
        [SerializeField] private bool _attachToThis = true;

        private void OnEnable()
        {
            if (_playOnEnable)
            {
                Play();
            }
        }

        public void Play()
        {
            _cue.Play(_attachToThis ? transform : null);
        }
    }
}