using System;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class TimeSpanParser : BaseJsonStringToPrimitiveParser<TimeSpan>, IJsonStringToPrimitiveParser
    {
        private readonly Type _timeSpanType = typeof (TimeSpan);

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.String && requestedType == _timeSpanType;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            return ParseValue(context.Token);
        }

        private static object ParseValue(JsonToken token)
        {
            var parser = new Iso8601DurationParser();
            return parser.ParseToken(ref token);
        }

        public ParseResult TryParse(JsonToken token)
        {
            try
            {
                return new ParseResult(true, ParseValue(token));
            }
            catch (JsonDocumentException)
            {
                return new ParseResult(false);
            }
        }
    }
}