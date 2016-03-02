using System;
using Light.GuardClauses;

namespace Light.Serialization.Json.IntegerMetadata
{
    public struct DecimalPartInfo
    {
        public readonly int IndexOfDecimalPoint;
        public readonly bool AreTrailingDigitsOnlyZeros;

        public DecimalPartInfo(int indexOfDecimalPoint, bool areTrailingDigitsOnlyZeros)
        {
            IndexOfDecimalPoint = indexOfDecimalPoint;
            AreTrailingDigitsOnlyZeros = areTrailingDigitsOnlyZeros;
        }

        public static DecimalPartInfo FromNumericJsonToken(JsonToken token)
        {
            Guard.Against(token.JsonType != JsonTokenType.FloatingPointNumber,
                          () => new ArgumentException($"The token must be a floating point number, but you specified {token}."));

            var areTrailingDigitsOnlyZeros = true;
            var indexOfDecimalPoint = -1;
            int i;

            for (i = 0; i < token.Length; i++)
            {
                if (token[i] != JsonSymbols.DecimalPoint) continue;

                indexOfDecimalPoint = i;
                break;
            }

            for (i++; i < token.Length; i++)
            {
                if (token[i] == '0') continue;
                areTrailingDigitsOnlyZeros = false;
                break;
            }

            return new DecimalPartInfo(indexOfDecimalPoint, areTrailingDigitsOnlyZeros);
        }
    }
}