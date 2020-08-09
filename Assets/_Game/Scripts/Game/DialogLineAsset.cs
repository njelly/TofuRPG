using System.Collections.Generic;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{

    [CreateAssetMenu(fileName = "New DialogLineAsset", menuName = "TofuRPG/DialogLineAsset")]
    public class DialogLineAsset : ScriptableObject
    {
        [SerializeField] private Sprite _avatarSprite;
        [SerializeField] private AudioCue _typewriterCue;
        [SerializeField] private float _typewriterSpeed; // characters per second
        [SerializeField] private float _minCueInterval;
        [TextArea, SerializeField] private List<string> _pages;

        public DialogLine GetDialogLine()
        {
            return new DialogLine()
            {
                avatarSprite = _avatarSprite,
                typewriterCue = _typewriterCue,
                typewriterSpeed = _typewriterSpeed,
                minCueInterval = _minCueInterval,
                pages = _pages,
            };
        }
    }

    public class DialogLine
    {
        public GameObject source;
        public Sprite avatarSprite;
        public AudioCue typewriterCue;
        public float typewriterSpeed;
        public float minCueInterval;
        public List<string> pages;
    }
}