using System;
using Newtonsoft.Json;
using Tofunaut.TofuUnity.Editor;
using UnityEditor;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game.Editor
{
    [CustomEditor(typeof(ActorConfig))]
    public class ActorConfigEditor : UnityEditor.Editor 
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Import JSON"))
                TextImportWindow.Init("Import ActorConfig JSON", Deserialize);
            
            DrawDefaultInspector();
        }

        private void Deserialize(string s)
        {
            var actorModels = JsonConvert.DeserializeObject<ActorModel[]>(s, new Vector2IntConverter());
            var actorConfig = (ActorConfig) target;
            actorConfig.actorModels = actorModels;
            EditorUtility.SetDirty(target);
        }

        private class Vector2IntConverter : JsonConverter<Vector2Int>
        {
            public override void WriteJson(JsonWriter writer, Vector2Int value, JsonSerializer serializer)
            {
                writer.WriteValue($"({value.x},{value.y})");
            }

            public override Vector2Int ReadJson(JsonReader reader, Type objectType, Vector2Int existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                var s = (string) reader.Value;
                
                if(string.IsNullOrEmpty(s))
                    return Vector2Int.zero;
                
                s = s.Trim('(', ')');
                
                var parts = s.Split(',');
                if(parts.Length != 2 || !int.TryParse(parts[0], out var x) || !int.TryParse(parts[1], out var y))
                    return Vector2Int.zero;
                
                return new Vector2Int(x, y);
            }
        }
    }
}