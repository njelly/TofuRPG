using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class ActorView : MonoBehaviour
    {
        public Actor Actor { get; private set; }

        private Dictionary<Type, ActorViewComponent> _typeToActorViewComponent;
        
        public void Initialize(Actor actor)
        {
            Actor = actor;
            _typeToActorViewComponent = new Dictionary<Type, ActorViewComponent>();
            var actorViewComponents = GetComponentsInChildren<ActorViewComponent>();
            foreach (var actorViewComponent in actorViewComponents)
                _typeToActorViewComponent.Add(actorViewComponent.GetType(), actorViewComponent);
            foreach (var actorViewComponent in actorViewComponents)
                actorViewComponent.Initialize(this);
        }
    }
}