using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.IntegerMetadata;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class UnsignedIntegerParser : IJsonTokenParser
    {
        private Dictionary<Type, UnsignedIntegerTypeInfo> _unsignedIntegerTypes = UnsignedIntegerTypeInfo.CreateDefaultUnsignedIntegerTypes();

        public Dictionary<Type, UnsignedIntegerTypeInfo> UnsignedIntegerTypes
        {
            get { return _unsignedIntegerTypes; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _unsignedIntegerTypes = value;
            }
        }

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return (token.JsonType == JsonTokenType.IntegerNumber || token.JsonType == JsonTokenType.FloatingPointNumber) && _unsignedIntegerTypes.ContainsKey(requestedType);
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var token = context.Token;
            if (token[0] == JsonSymbols.Minus && (token[1] != 0 || token.Length > 2))
                throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type {context.RequestedType}.");

            var digitsLeftToRead = token.Length;

            if (token.JsonType == JsonTokenType.FloatingPointNumber)
            {
                var decimalPartInfo = DecimalPartInfo.FromNumericJsonToken(token);
                if (decimalPartInfo.AreTrailingDigitsOnlyZeros == false)
                    throw new DeserializationException($"Could not deserialize value {token} because it is no integer, but a real number.");

                digitsLeftToRead = decimalPartInfo.IndexOfDecimalPoint;
            }

            string overflowCompareString = null;
            var integerInfo = _unsignedIntegerTypes[context.RequestedType];
            if (digitsLeftToRead > integerInfo.MaximumAsString.Length)
                throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type {integerInfo.Type}.");
            if (digitsLeftToRead == integerInfo.MaximumAsString.Length)
                overflowCompareString = integerInfo.MaximumAsString;

            var result = 0ul;
            var isDefinitelyInRange = false;
            var currentIndex = 0;
            while (digitsLeftToRead > 0)
            {
                var digit = token[currentIndex] - '0';
                if (isDefinitelyInRange == false && overflowCompareString != null)
                {
                    var overflowCompareDigit = overflowCompareString[currentIndex] - '0';
                    if (digit < overflowCompareDigit)
                        isDefinitelyInRange = true;
                    else if (digit > overflowCompareDigit)
                        throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type {integerInfo.Type}.");
                }
                currentIndex++;
                digitsLeftToRead--;

                result += (ulong) digit * CalculateBase(digitsLeftToRead);
            }

            return integerInfo.Type == typeof (ulong) ? result : integerInfo.DowncastValue(result);
        }

        private static ulong CalculateBase(int digitsLeftToRead)
        {
            var result = 1ul;
            for (var i = 0; i < digitsLeftToRead; i++)
            {
                result *= 10;
            }
            return result;
        }
    }
}