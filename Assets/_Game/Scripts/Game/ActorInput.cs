﻿using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{    
    public class ActorInput
    {
        public readonly DirectionButton Direction;
        public readonly Button Interact;

        public ActorInput()
        {
            Direction = new DirectionButton();
            Interact = new Button();
        }

        public class Button
        {
            public float TimePressed;
            public float TimeReleased;

            public float TimeHeld => !Held ? 0f : Time.time - TimePressed;

            public bool Pressed => Time.time - TimePressed <= float.Epsilon;
            public bool Held => TimePressed > TimeReleased;
            public bool Released => Time.time - TimeReleased < float.Epsilon;
        }

        public class DirectionButton : Button
        {
            public Vector2 Direction { get; private set; } = Vector2.zero;

            public void SetDirection(Vector2 direction)
            {
                var prevDirSqrMagnitude = Direction.sqrMagnitude;
                var newDirSqrMagnitude = direction.sqrMagnitude;
                if (prevDirSqrMagnitude > float.Epsilon && newDirSqrMagnitude <= float.Epsilon)
                    TimeReleased = Time.time;
                else if(prevDirSqrMagnitude <= float.Epsilon && newDirSqrMagnitude > float.Epsilon)
                    TimePressed = Time.time;

                Direction = direction;
            }

            public static implicit operator Vector2(DirectionButton button)
            {
                return button.Held ? button.Direction : Vector2.zero;
            }
        }
    }
}