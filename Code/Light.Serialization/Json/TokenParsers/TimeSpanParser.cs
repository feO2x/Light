using System;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class TimeSpanParser : IJsonTokenParser
    {
        private readonly Type _timeSpanType = typeof (TimeSpan);

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.String && requestedType == _timeSpanType;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var parser = new Iso8601DurationParser();
            var token = context.Token;
            return parser.ParseToken(ref token);
        }
    }
}