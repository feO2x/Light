using System;

namespace Light.Serialization.Json.JsonValueParsers
{
    public sealed class BooleanParser : IJsonValueParser
    {
        private readonly Type _booleanType = typeof (bool);

        public bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType)
        {
            return (buffer.JsonType == JsonType.True || buffer.JsonType == JsonType.False) && requestedType == _booleanType;
        }

        public object ParseValue(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer.JsonType == JsonType.True;
        }
    }
}