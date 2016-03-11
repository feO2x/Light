using System;
using System.Reflection;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class NullParser : IJsonTokenParser
    {
        public bool CanBeCached => true;

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            var typeInfo = requestedType.GetTypeInfo();
            return token.JsonType == JsonTokenType.Null && (typeInfo.IsClass || typeInfo.IsInterface);
        }

        public object ParseValue(JsonDeserializationContext context)
        { 
            return null;
        }
    }
}
