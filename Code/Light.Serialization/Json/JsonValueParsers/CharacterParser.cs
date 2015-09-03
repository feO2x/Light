using System;

namespace Light.Serialization.Json.JsonValueParsers
{
    public sealed class CharacterParser : IJsonValueParser
    {
        private readonly Type _charType = typeof (char);

        public bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer.JsonType == JsonType.String && requestedType == _charType;
        }

        public object DeserializeValue(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer.Count == 4 ? buffer[2] : buffer[1];
        }
    }
}
