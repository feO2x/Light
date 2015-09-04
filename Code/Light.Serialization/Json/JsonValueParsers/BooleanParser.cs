using System;

namespace Light.Serialization.Json.JsonValueParsers
{
    public sealed class BooleanParser : IJsonValueParser
    {
        private readonly Type _booleanType = typeof (bool);

        private const string False = "false";
        private const string True = "true";

        public bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer.JsonType == JsonType.Boolean && requestedType == _booleanType;
        }

        public object DeserializeValue(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer[0] == 'f' ? DeserializeInternal(buffer, False, false) : DeserializeInternal(buffer, True, true);
        }

        private static bool DeserializeInternal(JsonCharacterBuffer buffer, string booleanStringToCompare, bool returnValue)
        {
            if (buffer.Count != booleanStringToCompare.Length)
                throw new DeserializationException($"Cannot deserialize value {buffer} to a boolean value.");
            for (var i = 1; i < buffer.Count; i++)
            {
                if (buffer[i] != booleanStringToCompare[i])
                    throw new DeserializationException($"Cannot deserialize value {buffer} to a boolean value.");
            }
            return returnValue;
        }
    }
}