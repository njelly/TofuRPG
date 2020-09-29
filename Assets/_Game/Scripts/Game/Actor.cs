using System.Linq;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public struct ActorInput
    {
        public DirectionButton direction;
        public Button interact;
        public Button aim;

        public class Button
        {
            public float timePressed;
            public float timeReleased;

            public float TimeHeld => !Held ? 0f : Time.time - timePressed;

            public bool Pressed => Time.time - timePressed <= float.Epsilon;
            public bool Held => timePressed > timeReleased;
            public bool Released => Time.time - timeReleased < float.Epsilon;
        }

        public class DirectionButton : Button
        {
            public Vector2 Direction { get; private set; } = Vector2.zero;

            public void SetDirection(Vector2 direction)
            {
                float prevDirSqrMagnitude = Direction.sqrMagnitude;
                float newDirSqrMagnitude = direction.sqrMagnitude;
                if (prevDirSqrMagnitude > float.Epsilon && newDirSqrMagnitude <= float.Epsilon)
                {
                    timeReleased = Time.time;
                }
                else if(prevDirSqrMagnitude <= float.Epsilon && newDirSqrMagnitude > float.Epsilon)
                {
                    timePressed = Time.time;
                }

                Direction = direction;
            }

            public static implicit operator Vector2(DirectionButton button)
            {
                return button.Held ? button.Direction : Vector2.zero;
            }
        }
    }

    public class Actor : MonoBehaviour
    {

        public IActorInputProvider actorInputProvider;

        private IActorInputReceiver[] _actorInputReceivers;

        private void Awake()
        {
            MonoBehaviour[] components = GetComponents<MonoBehaviour>();
            actorInputProvider = components.OfType<IActorInputProvider>().FirstOrDefault();
            _actorInputReceivers = components.OfType<IActorInputReceiver>().ToArray();
        }

        private void Update()
        {
            PollActorInput();
        }

        private void PollActorInput()
        {
            if(actorInputProvider == null)
            {
                return;
            }

            ActorInput actorInput = actorInputProvider.GetActorInput();
            foreach(IActorInputReceiver receiver in _actorInputReceivers)
            {
                receiver.ReceiveActorInput(actorInput);
            }
        }
    }
}