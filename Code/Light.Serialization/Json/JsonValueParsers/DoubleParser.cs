using System;
using System.Globalization;

namespace Light.Serialization.Json.JsonValueParsers
{
    public sealed class DoubleParser : IJsonValueParser
    {
        private readonly Type _doubleType = typeof (double);

        public bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer.JsonType == JsonType.Number && requestedType == _doubleType;
        }

        public object DeserializeValue(JsonCharacterBuffer buffer, Type requestedType)
        {
            var doubleString = buffer.ToString();
            double result;
            if (double.TryParse(doubleString, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out result))
                return result;

            throw new DeserializationException($"Cannot deserialize value {doubleString} into a double value.");
        }
    }
}
