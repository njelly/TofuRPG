using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class GameCameraController : MonoBehaviour, IGameCameraController
    {
        [SerializeField] private GameObject _following;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private float _cameraZ = -10;

        private Camera _camera;
        private Vector2 _followPos;

        protected void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void LateUpdate()
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

        public void Follow(GameObject target)
        {
            _following = target;
            _followPos = target.transform.position;
        }
    }
}