using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.IntegerMetadata;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class UnsignedIntegerParser : IJsonTokenParser
    {
        private Dictionary<Type, UnsignedIntegerTypeInfo> _unsignedIntegerTypes = UnsignedIntegerTypeInfo.CreateDefaultUnsignedIntegerTypes();

        public bool CanBeCached => true;

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
            return (token.JsonType == JsonTokenType.IntegerNumber || token.JsonType == JsonTokenType.FloatingPointNumber || token.JsonType == JsonTokenType.String) && 
                _unsignedIntegerTypes.ContainsKey(requestedType);
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var token = context.Token;
            if (token.JsonType == JsonTokenType.String)
                token = token.RemoveOuterQuotationMarks();

            var currentIndex = 0;
            var digitsLeftToRead = token.Length;

            if (token[0] == JsonSymbols.Minus)
            {
                if (token[1] != '0')
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type {context.RequestedType}.");

                if (token.JsonType == JsonTokenType.FloatingPointNumber)
                {
                    var decimalPartInfo = DecimalPartInfo.FromNumericJsonToken(token);
                    if (decimalPartInfo.AreTrailingDigitsOnlyZeros == false)
                        throw new DeserializationException($"Could not deserialize value {token} because it is no integer, but a real number.");
                }

                var info = _unsignedIntegerTypes[context.RequestedType];
                return info.Type == typeof(ulong) ? 0UL : info.DowncastValue(0UL);
            }

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