using System;

namespace Light.Serialization.Json.JsonValueParsers
{
    public sealed class NullParser : IJsonValueParser
    {
        private const string Null = "null";

        public bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer.JsonType == JsonType.Null && (requestedType.IsClass || requestedType.IsInterface);
        }

        public object DeserializeValue(JsonCharacterBuffer buffer, Type requestedType)
        {
            if (buffer.Count != Null.Length)
                throw new DeserializationException($"Cannot deserialize value {buffer} to null");

            for (var i = 1; i < buffer.Count; i++)
            {
                if (buffer[i] != Null[i])
                    throw new DeserializationException($"Cannot deserialize value {buffer} to null");
            }

            return null;
        }
    }
}
