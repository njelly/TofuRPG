using Tofunaut.TofuRPG.Game;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    /// <summary>
    /// Provides ActorInput based on the player's input.
    /// </summary>
    public class PlayerActorInput : MonoBehaviour, IActorInputProvider
    {
        private ActorInput _actorInput;

        private void Awake()
        {
            _actorInput = new ActorInput
            {
                direction = new ActorInput.DirectionButton(),
                interact = new ActorInput.Button(),
                aim = new ActorInput.Button(),
            };
        }

        public ActorInput GetActorInput()
        {
            _actorInput.direction.SetDirection(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

            return _actorInput;
        }
    }
}