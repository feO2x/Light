using System;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class BooleanParser : IJsonTokenParser
    {
        public bool CanBeCached => true;

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.True || token.JsonType == JsonTokenType.False;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            return context.Token.JsonType == JsonTokenType.True;
        }
    }
}