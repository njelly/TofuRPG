using System;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class ActorInput
    {
        public readonly InputDoubleAxis direction;
        public readonly InputButton interact;

        public ActorInput()
        {
            direction = new InputDoubleAxis();
            interact = new InputButton();
        }
    }
}