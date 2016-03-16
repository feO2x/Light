using System;
using System.Globalization;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class DecimalParser : IJsonTokenParser
    {
        public bool CanBeCached => true;

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return (token.JsonType == JsonTokenType.FloatingPointNumber || token.JsonType == JsonTokenType.IntegerNumber || token.JsonType == JsonTokenType.String) && requestedType == typeof(decimal);
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var token = context.Token;
            if (token.JsonType == JsonTokenType.String)
                token = token.RemoveOuterQuotationMarks();

            var decimalString = token.ToString();
            decimal result;
            if (decimal.TryParse(decimalString, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
                return result;

            throw new DeserializationException($"Cannot deserialize value {decimalString} to a decimal value.");
        }
    }
}