using System;
using Tofunaut.TofuRPG.Game.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuRPG.Game
{
    public class PlayerActorInputProvider : MonoBehaviour, IActorInputProvider, IActorInputReceiver
    {
        public ActorInput ActorInput { get; private set; }

        public bool registerWithManagerOnStart;

        private void Awake()
        {
            ActorInput = new ActorInput();
        }

        public void Start()
        {
            if (registerWithManagerOnStart)
                PlayerActorInputManager.Add(this);
        }

        public void ReceiveActorInput(ActorInput actorInput)
        {
            ActorInput = actorInput;
        }
    }
}