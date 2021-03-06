﻿using System;
using Tofunaut.TofuRPG.Game.Interfaces;
using Tofunaut.TofuRPG.Game.UI;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class NPCDialogInteractable : ActorComponent, IInteractable
    {
        public Actor Actor { get; private set; }
        public bool IsBeingInteractedWith => _interactor;

        public event EventHandler<InteractableEventArgs> InteractionBegan;
        
        [TextArea] public string dialog;

        private Interactor _interactor;

        public void BeginInteraction(Interactor interactor)
        {
            _interactor = interactor;
            InGameStateController.Blackboard?.Invoke(new EnqueueDialogEvent(new Dialog
            {
                Text = dialog,
                OnDialogComplete = () =>
                {
                    _interactor = null;
                },
            }));
            InteractionBegan?.Invoke(this, new InteractableEventArgs(interactor));
        }

        public override void Initialize(Actor actor, ActorModel model)
        {
            Actor = actor;
        }
    }
}