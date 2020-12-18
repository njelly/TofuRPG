using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [CreateAssetMenu(fileName = "new ActorConfig", menuName = "TofuRPG/ActorConfig")]
    public class ActorConfig : ScriptableObject
    {
        public ActorModel[] actorModels;
        
        private Dictionary<string, ActorModel> _lookUpDictionary;

        public string[] GetKeys()
        {
            if (_lookUpDictionary == null)
                _lookUpDictionary = BuildLookUpDictionary();

            return _lookUpDictionary.Keys.ToArray();
        }

        public bool TryGetActorModel(string key, out ActorModel actorModel)
        {
            if (_lookUpDictionary == null)
                _lookUpDictionary = BuildLookUpDictionary();

            return _lookUpDictionary.TryGetValue(key, out actorModel);
        }
        
        private Dictionary<string, ActorModel> BuildLookUpDictionary() 
            => actorModels.ToDictionary(actorModel => actorModel.Name);
    }

    [Serializable]
    public struct ActorModel
    {
        public enum EActorInputSource
        {
            None,
            Player,
            AI,
        }
        
        public string Name;
        public string ViewAsset;
        public EActorInputSource ActorInputSource;
        public string AIAsset;
        public float BaseHealth;
        public float MoveSpeed;
        public Vector2Int ColliderSize;
        public Vector2Int ColliderOffset;
        public float MoveHesitationTime;
        public Vector2Int BaseInteractOffset;
    }
}