using System;
using Newtonsoft.Json;

namespace Bridge.Models.Common.Files
{
    public class ResolutionConverter : JsonConverter<Resolution?>
    {
        public override void WriteJson(JsonWriter writer, Resolution? value, JsonSerializer serializer)
        {
            var strValue = value.ToString();
            var withoutUnderscore = strValue.TrimStart('_');
            writer.WriteValue(withoutUnderscore);
        }

        public override Resolution? ReadJson(JsonReader reader, Type objectType, Resolution? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (hasExistingValue)
                return existingValue;

            var val = reader.Value;
            if (val == null)
                return null;

            if (val is long index)
                return (Resolution) index;

            var name = val as string;

            if (char.IsNumber(name[0]) && !name.StartsWith("_"))
                name = "_" + name;

            return (Resolution)Enum.Parse(typeof(Resolution), name);
        }

        public override bool CanRead => true;

        public override bool CanWrite => true;
    }
}