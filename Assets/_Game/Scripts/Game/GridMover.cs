using Tofunaut.TofuUnity;
using UnityEngine;
using static Tofunaut.TofuUnity.TofuAnimator;

namespace Tofunaut.TofuRPG.Game
{
    public class GridMover : MonoBehaviour
    {
        public Vector2Int Coord { get; private set; }

        public float moveSpeed;

        private Actor _actor;
        private TofuAnimator.Sequence _moveSequence;

        private void Awake()
        {
            _actor = gameObject.RequireComponent<Actor>();
        }

        private void Update()
        {
            if (_moveSequence == null && _actor.Input.direction.sqrMagnitude <= float.Epsilon)
            {
            }

        }

        private void MoveTo(Vector2Int newCoord)
        {
            Vector2Int prevCoord = Coord;
            Coord = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));


            _moveSequence = gameObject.Sequence()
                .Curve(EEaseType.Linear, moveSpeed, (float newValue) =>
                {
                    //transform.position = Vector2.LerpUnclamped(prevCoord, )

                });
            _moveSequence.Play();
        }
    }
}