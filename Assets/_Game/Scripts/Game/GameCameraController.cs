using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class GameCameraController : SingletonBehaviour<GameCameraController>
    {
        public static Camera Camera
        {
            get
            {
                if (_instance)
                {
                    return _instance._camera;
                }
                return null;
            }
        }

        [SerializeField] private GameObject _following;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private float _cameraZ = -10;

        private Camera _camera;
        private Vector2 _followPos;

        protected override void Awake()
        {
            base.Awake();

            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (_following)
            {
                _followPos = _following.transform.position;
            }
            else
            {
                _followPos = new Vector3(transform.position.x, transform.position.y, _cameraZ);
            }

            gameObject.transform.position = new Vector3(_offset.x, _offset.y, _cameraZ) + new Vector3(_followPos.x, _followPos.y, 0f);
        }

        public static void Follow(GameObject target)
        {
            _instance._following = target;
            _instance._followPos = target.transform.position;
        }
    }
}