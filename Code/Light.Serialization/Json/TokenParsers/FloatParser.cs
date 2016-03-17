using System;
using System.Globalization;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class FloatParser : IJsonTokenParser
    {
        public bool CanBeCached => true;

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return (token.JsonType == JsonTokenType.FloatingPointNumber || token.JsonType == JsonTokenType.IntegerNumber || token.JsonType == JsonTokenType.String) && requestedType == typeof(float);
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var token = context.Token;
            if (token.JsonType == JsonTokenType.String)
                token = token.RemoveOuterQuotationMarks();

            var floatString = token.ToString();
            float result;
            if (float.TryParse(floatString, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out result))
                return result;

            throw new DeserializationException($"Cannot deserialize value {floatString} to a float value.");
        }
    }
}
