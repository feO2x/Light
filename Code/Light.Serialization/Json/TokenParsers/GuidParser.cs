using System;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class GuidParser : BaseJsonStringToPrimitiveParser<Guid>, IJsonStringToPrimitiveParser
    {
        public bool CanBeCached => true;

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.String && requestedType == typeof (Guid);
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var token = context.Token;
            var guidString = token.ToStringWithoutQuotationMarks();

            Guid parsedGuid;
            if (Guid.TryParse(guidString, out parsedGuid) == false)
                throw new JsonDocumentException($"Could not deserialize token {token} to a valid GUID.", token);

            return parsedGuid;
        }

        public ParseResult TryParse(JsonToken token)
        {
            var guidString = token.ToStringWithoutQuotationMarks();

            Guid parsedGuid;
            return Guid.TryParse(guidString, out parsedGuid) == false ? new ParseResult(false) : new ParseResult(true, parsedGuid);
        }
    }
}