﻿using System;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class ActorInput
    {
        public readonly InputDoubleAxis Direction;
        public readonly InputButton Interact;

        public ActorInput()
        {
            Direction = new InputDoubleAxis();
            Interact = new InputButton();
        }
    }
}