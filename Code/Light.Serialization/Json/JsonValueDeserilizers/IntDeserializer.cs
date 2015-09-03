using System;

namespace Light.Serialization.Json.JsonValueDeserilizers
{
    public sealed class IntDeserializer : IJsonValueDeserializer
    {
        private readonly Type _intType = typeof (int);

        public char DecimalPointCharacter = '.';
        public char NegativeSign = '-';

        public bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer.JsonType == JsonType.Number && requestedType == _intType;

        }

        public object DeserializeValue(JsonCharacterBuffer buffer, Type requestedType)
        {
            var positionsBeforeDecimalPoint = buffer.Count;

            var currentIndex = 0;
            var isResultNegative = false;
            if (buffer[currentIndex] == NegativeSign)
            {
                isResultNegative = true;
                positionsBeforeDecimalPoint--;
                currentIndex++;
            }

            var result = 0;
            while (currentIndex < buffer.Count)
            {
                var digit = buffer[currentIndex] - '0';
                result += digit * CalculatePosition(positionsBeforeDecimalPoint);

                currentIndex++;
                positionsBeforeDecimalPoint--;
            }

            if (isResultNegative)
                return -result;

            return result;
        }

        private static int CalculatePosition(int positionsBeforeDecimalPoint)
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

   }
}