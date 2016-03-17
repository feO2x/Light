using System;
using System.Globalization;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class DoubleParser : IJsonTokenParser
    {
        public bool CanBeCached => true;

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return ((token.JsonType == JsonTokenType.FloatingPointNumber || token.JsonType == JsonTokenType.IntegerNumber || token.JsonType == JsonTokenType.String) && requestedType == typeof (double)) ||
                   (requestedType == typeof (object) || requestedType == typeof (ValueType)) && token.JsonType == JsonTokenType.FloatingPointNumber;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var token = context.Token;
            if (token.JsonType == JsonTokenType.String)
                token = token.RemoveOuterQuotationMarks();

            var doubleString = token.ToString();
            double result;
            if (double.TryParse(doubleString, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out result))
                return result;

            throw new DeserializationException($"Cannot deserialize value {doubleString} to a double value.");
        }
    }
}
