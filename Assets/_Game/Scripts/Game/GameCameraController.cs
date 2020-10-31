using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [RequireComponent(typeof(Camera))]
    public class GameCameraController : SingletonBehaviour<GameCameraController>
    {
        public Vector3 offset;
        public Transform target;
        public float lerpToTargetTime;

        private Vector3 _defaultOffset;

        protected override void Awake()
        {
            base.Awake();
            _defaultOffset = offset;
        }

        public void LateUpdate()
        {
            if (!target)
                return;

            transform.position = target.position + offset;
        }

        public static void SetOffset(Vector3 offset)
        {
            if (!_instance)
                return;
            
            _instance.offset = offset;
        }

        public static void SetTarget(Transform target)
        {
            if (!_instance)
                return;

            _instance.target = target;
            var startOffset = _instance.target.position - _instance.transform.position;
            var endOffset = _instance._defaultOffset;
            _instance.offset = startOffset;
            Debug.Log(startOffset);
            _instance.gameObject.Sequence()
                .Curve(TofuAnimator.EEaseType.EaseOutExpo, _instance.lerpToTargetTime, newValue =>
                {
                    _instance.offset = Vector3.Lerp(startOffset, endOffset, newValue);
                })
                .Play();
        }
    }
}