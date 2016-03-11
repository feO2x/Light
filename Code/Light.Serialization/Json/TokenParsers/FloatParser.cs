using System;
using System.Globalization;

namespace Light.Serialization.Json.TokenParsers
{
    public class FloatParser : IJsonTokenParser
    {
        private readonly Type _floatType = typeof(float);

        public bool CanBeCached => true;

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return (token.JsonType == JsonTokenType.FloatingPointNumber || token.JsonType == JsonTokenType.IntegerNumber) && requestedType == _floatType;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var floatString = context.Token.ToString();
            float result;
            if (float.TryParse(floatString, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out result))
                return result;

            throw new DeserializationException($"Cannot deserialize value {floatString} to a float value.");
        }
    }
}
