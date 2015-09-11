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

        public object ParseValue(JsonDeserializationContext context)
        {
            if (context.Buffer.Count != Null.Length)
                throw new DeserializationException($"Cannot deserialize value {context.Buffer} to null");

            for (var i = 1; i < context.Buffer.Count; i++)
            {
                if (context.Buffer[i] != Null[i])
                    throw new DeserializationException($"Cannot deserialize value {context.Buffer} to null");
            }

            return null;
        }
    }
}
