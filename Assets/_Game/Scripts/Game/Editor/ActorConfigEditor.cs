using System;
using System.Linq;
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
                ShowTextImportWindow();
            
            DrawDefaultInspector();
        }

        private void ShowTextImportWindow()
        {
            var actorConfig = (ActorConfig) target;
            var initialText = JsonConvert.SerializeObject(actorConfig.actorModels, Formatting.Indented,new Vector2IntConverter());
            TextImportWindow.Init("Import ActorConfig JSON", initialText, Deserialize);
        }

        private void Deserialize(string s)
        {
            var actorModels = JsonConvert.DeserializeObject<ActorModel[]>(s, new Vector2IntConverter());
            if (actorModels == null)
            {
                Debug.LogError("could not deserialize ActorModel array JSON");
                return;
            }
            
            var actorConfig = (ActorConfig) target;
            var duplicates = actorModels.GroupBy(x => x.Name)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);
            
            var duplicateArray = duplicates as string[] ?? duplicates.ToArray();
            if (duplicateArray.Any())
            {
                foreach (var duplicateName in duplicateArray)
                    Debug.LogError($"multiple ActorModels with name {duplicateName}");

                return;
            }
            
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