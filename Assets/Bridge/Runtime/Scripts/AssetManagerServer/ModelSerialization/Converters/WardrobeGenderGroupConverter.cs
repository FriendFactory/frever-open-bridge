using System;
using Bridge.Models.AsseManager;
using Bridge.Models.Common;
using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bridge.AssetManagerServer.ModelSerialization.Converters
{
    internal class WardrobeGenderGroupConverter: JsonConverter<WardrobeGenderGroup>
    {
        public override void WriteJson(JsonWriter writer, WardrobeGenderGroup value, JsonSerializer serializer)
        {
            var jToken = JToken.FromObject(value);

            var idPropName = nameof(IEntity.Id);
            var idPropNameCamelCase = idPropName.FirstCharToLower();

            var result = new JObject
            {
                {idPropNameCamelCase, jToken[idPropName]}
            };
            var wardrobeSource = jToken[nameof(WardrobeGenderGroup.Wardrobe)] as JArray;
            var wardrobeResults = new JArray();

            if (wardrobeSource != null)
            {
                foreach (var wardrobe in wardrobeSource)
                {
                    var obj = wardrobe as JObject;
                    var wardrobeRes = new JObject
                    {
                        {idPropNameCamelCase, obj[idPropName]}
                    };

                    wardrobeResults.Add(wardrobeRes);
                }
            }
          
            result.Add(nameof(WardrobeGenderGroup.Wardrobe),wardrobeResults);
            result.WriteTo(writer);
        }

        public override WardrobeGenderGroup ReadJson(JsonReader reader, Type objectType, WardrobeGenderGroup existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => false;
    }
}