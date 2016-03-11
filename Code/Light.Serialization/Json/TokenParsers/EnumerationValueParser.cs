using System;
using System.Reflection;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class EnumerationValueParser : IJsonTokenParser
    {
        public bool CanBeCached => true;

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.String && requestedType.GetTypeInfo().IsEnum;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var @string = context.DeserializeToken<string>(context.Token);

            return Enum.Parse(context.RequestedType, @string, true);
        }
    }
}