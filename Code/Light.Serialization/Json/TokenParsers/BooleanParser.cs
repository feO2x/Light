using System;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class BooleanParser : IJsonTokenParser
    {
        private readonly Type _booleanType = typeof (bool);

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return (token.JsonType == JsonTokenType.True || token.JsonType == JsonTokenType.False) && requestedType == _booleanType;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            return context.Token.JsonType == JsonTokenType.True;
        }
    }
}