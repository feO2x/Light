using System;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class NullParser : IJsonTokenParser
    {
        private const string Null = "null";

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.Null && (requestedType.IsClass || requestedType.IsInterface);
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            if (context.Token.Length != Null.Length)
                throw new DeserializationException($"Cannot deserialize value {context.Token} to null");

            for (var i = 1; i < context.Token.Length; i++)
            {
                if (context.Token[i] != Null[i])
                    throw new DeserializationException($"Cannot deserialize value {context.Token} to null");
            }

            return null;
        }
    }
}
