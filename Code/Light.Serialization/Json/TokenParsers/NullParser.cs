using System;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class NullParser : IJsonTokenParser
    {
        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.Null && (requestedType.IsClass || requestedType.IsInterface);
        }

        public object ParseValue(JsonDeserializationContext context)
        { 
            return null;
        }
    }
}
