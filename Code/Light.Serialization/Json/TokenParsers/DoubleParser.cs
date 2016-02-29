using System;
using System.Globalization;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class DoubleParser : IJsonTokenParser
    {
        private readonly Type _doubleType = typeof (double);

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.FloatingPointNumber || token.JsonType == JsonTokenType.IntegerNumber && requestedType == _doubleType;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var doubleString = context.Token.ToString();
            double result;
            if (double.TryParse(doubleString, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out result))
                return result;

            throw new DeserializationException($"Cannot deserialize value {doubleString} to a double value.");
        }
    }
}
