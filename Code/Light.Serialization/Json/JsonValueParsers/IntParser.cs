using System;

namespace Light.Serialization.Json.JsonValueParsers
{
    public sealed class IntParser : IJsonValueParser
    {
        private readonly Type _intType = typeof (int);

        public const char DecimalPointCharacter = '.';
        public const char NegativeSign = '-';

        public const string MaxIntAsString = "2147483647";
        public const string MinIntAsString = "-2147483648";

        public bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer.JsonType == JsonType.Number && requestedType == _intType;
        }

        public object ParseValue(JsonCharacterBuffer buffer, Type requestedType)
        {
            var decimalPointInfo = GetIndexOfDecimalPoint(buffer);
            var positionsBeforeDecimalPoint = buffer.Count;

            if (decimalPointInfo.IndexOfDecimalPoint != null)
            {
                if (decimalPointInfo.AreTrailingDigitsOnlyZeros == false)
                    throw new DeserializationException($"Could not deserialize value {buffer} because it is no integer, but a real number");

                positionsBeforeDecimalPoint = decimalPointInfo.IndexOfDecimalPoint.Value;
            }

            var currentIndex = 0;
            var isResultNegative = false;
            string overflowCompareString = null;
            if (buffer[0] == NegativeSign)
            {
                if (buffer.Count > MinIntAsString.Length)
                    throw new DeserializationException($"Could not deserialize value {buffer} because it produces an overflow for type int.");
                if (buffer.Count == MinIntAsString.Length)
                    overflowCompareString = MinIntAsString;
                isResultNegative = true;

                positionsBeforeDecimalPoint--;
                currentIndex++;
            }
            else if (buffer.Count > MaxIntAsString.Length)
                throw new DeserializationException($"Could not deserialize value {buffer} because it produces an overflow for type int.");
            else if (buffer.Count == MaxIntAsString.Length)
                overflowCompareString = MaxIntAsString;

            var result = 0;
            var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                var digit = buffer[currentIndex] - '0';

                if (digit > overflowCompareString?[currentIndex] - '0')
                    throw new DeserializationException($"Could not deserialize value {buffer} because it produces an overflow for type int.");

                result += digit * CalculateBase(currentPositionBeforeDecimalPoint);

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

            if (isResultNegative)
                return -result;

            return result;
        }

        private static int CalculateBase(int positionsBeforeDecimalPoint)
        {
            if (positionsBeforeDecimalPoint == 1)
                return 1;

            var result = 10;
            for (var i = 2; i < positionsBeforeDecimalPoint; i++)
            {
                result *= 10;
            }
            return result;
        }

        private static DecimalPointInfo GetIndexOfDecimalPoint(JsonCharacterBuffer buffer)
        {
            var areTrailingDigitsOnlyZeros = true;
            int? indexOfDecimalPoint = null;
            int i;

            for (i = 0; i < buffer.Count; i++)
            {
                if (buffer[i] != DecimalPointCharacter) continue;

                indexOfDecimalPoint = i;
                break;
            }

            for (i++; i < buffer.Count; i++)
            {
                if (buffer[i] == '0') continue;
                areTrailingDigitsOnlyZeros = false;
                break;
            }

            return new DecimalPointInfo(indexOfDecimalPoint, areTrailingDigitsOnlyZeros);
        }

        private struct DecimalPointInfo
        {
            public readonly int? IndexOfDecimalPoint;
            public readonly bool AreTrailingDigitsOnlyZeros;

            public DecimalPointInfo(int? indexOfDecimalPoint, bool areTrailingDigitsOnlyZeros)
            {
                AreTrailingDigitsOnlyZeros = areTrailingDigitsOnlyZeros;
                IndexOfDecimalPoint = indexOfDecimalPoint;
            }
        }
    }
}