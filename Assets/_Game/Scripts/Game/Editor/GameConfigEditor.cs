using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tofunaut.TofuUnity.Editor;
using UnityEditor;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game.Editor
{
    [CustomEditor(typeof(GameConfig))]
    public class GameConfigEditor : UnityEditor.Editor 
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Import JSON"))
                ShowTextImportWindow();
            
            DrawDefaultInspector();
        }

        private void ShowTextImportWindow()
        {
            var actorConfig = (GameConfig) target;
            var a = new object[]
            {
                actorConfig.actorModels,
                actorConfig.attackModels,
            };
            var initialText = JsonConvert.SerializeObject(a, Formatting.Indented,
                new Vector2IntConverter(),
                new StringConverter(),
                new FloatConverter());
            TextImportWindow.Init("Import ActorConfig JSON", initialText, Deserialize);
        }

        private void Deserialize(string s)
        {
            var modelArrayArray= JsonConvert.DeserializeObject<object[]>(s, 
                new Vector2IntConverter(),
                new StringConverter(),
                new FloatConverter());
            if (modelArrayArray == null)
            {
                Debug.LogError("could not deserialize GameConfig JSON");
                return;
            }

            var serializer = new JsonSerializer();
            serializer.Converters.Add(new Vector2IntConverter());
            serializer.Converters.Add(new FloatConverter());
            serializer.Converters.Add(new StringConverter());
            var gameConfig = (GameConfig) target;
            var actorModels = ((JArray)modelArrayArray[0])
                .Select(x => x.ToObject<ActorModel>(serializer))
                .Where(x => !string.IsNullOrEmpty(x.Name)).ToArray();
            var attackModels = ((JArray)modelArrayArray[1])
                .Select(x => x.ToObject<AttackModel>(serializer))
                .Where(x => !string.IsNullOrEmpty(x.Name)).ToArray();

            var duplicates = actorModels.GroupBy(x => x.Name)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .Concat(attackModels.GroupBy(x => x.Name)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key));

            var duplicateArray = duplicates as string[] ?? duplicates.ToArray();
            if (duplicateArray.Any())
            {
                foreach (var duplicateName in duplicateArray)
                    Debug.LogError($"multiple models with name {duplicateName}");
                return;
            }
            
            gameConfig.actorModels = actorModels;
            gameConfig.attackModels = attackModels;
            
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
    }

    public class Vector2IntConverter : JsonConverter<Vector2Int>
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

    public class FloatConverter : JsonConverter<float>
    {
        public override void WriteJson(JsonWriter writer, float value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }

        public override float ReadJson(JsonReader reader, Type objectType, float existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var s = reader.Value == null ? "0" : reader.Value.ToString();
            return float.TryParse(s, out var v) ? v : 0f;
        }
    }

    public class StringConverter : JsonConverter<string>
    {
        public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }

        public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return reader.Value == null ? string.Empty : (string)reader.Value;
        }
    }
}