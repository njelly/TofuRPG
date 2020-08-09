using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Tofunaut.TofuRPG.Game
{
    public class UIDialog : MonoBehaviour, PlayerInputManager.IPlayerInputReceiver
    {
        public bool HasPlayed { get; private set; }
        public bool IsComplete { get; private set; }

        [SerializeField] private Image _avatar;
        [SerializeField] private TMPro.TextMeshProUGUI _text;

        private PlayerInput _input;
        private bool _hasReceivedInput;
        private float _lastTypewriterCueTime;

        private void OnEnable()
        {
            _hasReceivedInput = false;
            _text.text = string.Empty;
            PlayerInputManager.AddReceiver(1, this);
        }

        private void OnDisable()
        {
            _hasReceivedInput = false;
            PlayerInputManager.RemoveReceiver(this);
        }

        public void Play(DialogLine line)
        {
            HasPlayed = true;

            _avatar.sprite = line.avatarSprite;
            _avatar.gameObject.SetActive(_avatar.sprite != null);

            _input = new PlayerInput();

            StartCoroutine(ShowPageCoroutine(line, 0));
        }

        private IEnumerator ShowPageCoroutine(DialogLine line, int pageIndex)
        {
            if (pageIndex >= line.pages.Count)
            {
                IsComplete = true;
                yield break;
            }

            yield return new WaitUntil(() =>
            {
                return _hasReceivedInput;
            });

            yield return new WaitUntil(() =>
            {
                return !_input.select.Held;
            });

            float typewriterInterval = 1f / line.typewriterSpeed;
            for (int i = 0; i < line.pages[pageIndex].Length; i++)
            {

                float timer = 0f;
                while (timer < typewriterInterval)
                {
                    // need to be able to skip between ticks
                    if (_input.select.Pressed)
                    {
                        i = line.pages[pageIndex].Length - 1;
                    }

                    timer += Time.deltaTime;
                    yield return null;
                }

                _text.text = line.pages[pageIndex].Substring(0, i + 1);

                // only play typewriter cue during alpha numeric characters (not spaces or punctuation!)
                if (line.typewriterCue
                    && Time.time - _lastTypewriterCueTime > line.minCueInterval
                    && Char.IsLetterOrDigit(_text.text.ToCharArray()[i]))
                {
                    Transform sourceTransform = null;
                    if (line.source)
                    {
                        sourceTransform = line.source.transform;
                    }

                    line.typewriterCue.Play(sourceTransform);
                    _lastTypewriterCueTime = Time.time;
                }
            }

            yield return new WaitUntil(() =>
            {
                return _input.select.Pressed;
            });

            yield return ShowPageCoroutine(line, pageIndex + 1);
        }

        public void ReceivePlayerInput(PlayerInput input)
        {
            _input = input;
            _hasReceivedInput = true;
        }
    }
}