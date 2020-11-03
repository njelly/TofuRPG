using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class GameCameraManager : SingletonBehaviour<GameCameraManager>
    {
        public Camera gameCamera;
        public Vector3 offset;
        public Transform target;

        public void LateUpdate()
        {
            if (!target || !gameCamera)
                return;

            gameCamera.transform.position = target.position + offset;
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
        }
    }
}