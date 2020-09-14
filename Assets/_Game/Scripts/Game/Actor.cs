using System.Linq;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class ActorInput
    {
        public PlayerInput.DirectionButton direction;
        public PlayerInput.Button interact;
        public PlayerInput.Button aim;
    }

    public class Actor : MonoBehaviour, PlayerInputManager.IPlayerInputReceiver
    {
        public interface IInteractable
        {
            void BeginInteraction(Actor actor);
            void EndInteraction(Actor actor);
        }

        public interface IActorInputReceiver
        {
            void ReceiveActorInput(ActorInput actorInput);
        }

        private ActorInput _input;
        private IActorInputReceiver[] _actorInputReceivers;

        private void Start()
        {
            _input = new ActorInput();

            // Get all IActorInputReceivers on this GameObject
            _actorInputReceivers = GetComponents<MonoBehaviour>().OfType<IActorInputReceiver>().ToArray();
        }

        public void ReceivePlayerInput(PlayerInput playerInput)
        {
            _input.direction.SetDirection(playerInput.direction);

            _input.aim.timePressed = playerInput.shift.timePressed;
            _input.aim.timeReleased = playerInput.shift.timeReleased;

            _input.interact.timePressed = playerInput.select.timePressed;
            _input.interact.timeReleased = playerInput.select.timeReleased;

            foreach (IActorInputReceiver actorInputReceiver in _actorInputReceivers)
            {
                actorInputReceiver.ReceiveActorInput(_input);
            }
        }
    }
}