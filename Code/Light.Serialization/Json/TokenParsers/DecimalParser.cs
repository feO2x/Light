using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Serialization.Json.TokenParsers
{
    public class DecimalParser : IJsonTokenParser
    {
        private readonly Type _decimalType = typeof(decimal);

        public bool CanBeCached => true;

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return (token.JsonType == JsonTokenType.FloatingPointNumber || token.JsonType == JsonTokenType.IntegerNumber) && requestedType == _decimalType;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var decimalString = context.Token.ToString();
            decimal result;
            if (decimal.TryParse(decimalString, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
                return result;

            throw new DeserializationException($"Cannot deserialize value {decimalString} to a decimal value.");
        }
    }
}
