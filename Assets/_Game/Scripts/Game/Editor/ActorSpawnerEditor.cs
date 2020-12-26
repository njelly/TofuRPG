using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace Tofunaut.TofuRPG.Game.Editor
{
    [CustomEditor(typeof(ActorSpawner))]
    public class ActorSpawnerEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var actorSpawner = target as ActorSpawner;
            if (actorSpawner == null)
                return;

            var prevColor = Handles.color;
            Handles.color = Color.red;
            Handles.Label(actorSpawner.transform.position, actorSpawner.actorModelKey);
            Handles.color = prevColor;
        }
    }
}