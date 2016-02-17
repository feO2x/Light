using System;

namespace Light.Serialization.Json.TokenParsers
{
	public class IntParser : IJsonTokenParser
	{
		private readonly Type _integerType = typeof (int);
		
        public const char DecimalPointCharacter = '.';
        public const char NegativeSign = '-';

        public const string MaxAsString = "2147483647";
        public const string MinAsString = "-2147483648";

		public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return (token.JsonType == JsonTokenType.IntegerNumber || token.JsonType == JsonTokenType.FloatingPointNumber) && requestedType == _integerType;
        }

		public object ParseValue(JsonDeserializationContext context)
        {
            var token = context.Token;
            var decimalPointInfo = GetIndexOfDecimalPoint(token);
            var positionsBeforeDecimalPoint = token.Length;

            if (decimalPointInfo.IndexOfDecimalPoint != null)
            {
                if (decimalPointInfo.AreTrailingDigitsOnlyZeros == false)
                    throw new DeserializationException($"Could not deserialize value {token} because it is no integer, but a real number");

                positionsBeforeDecimalPoint = decimalPointInfo.IndexOfDecimalPoint.Value;
            }

            var currentIndex = 0;
            var isResultNegative = false;
            string overflowCompareString = null;

			if (NegativeSign != '+' && token[0] == NegativeSign)
            {
                if (token.Length > MinAsString.Length)
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");
                if (token.Length == MinAsString.Length)
                    overflowCompareString = MinAsString;
                isResultNegative = true;

                positionsBeforeDecimalPoint--;
                currentIndex++;
            }
			else if (token.Length > MaxAsString.Length)
                throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");
            else if (token.Length == MaxAsString.Length)
                overflowCompareString = MaxAsString;

			var result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                var digit = token[currentIndex] - '0';

                if (digit > overflowCompareString?[currentIndex] - '0')
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");

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

        private static DecimalPointInfo GetIndexOfDecimalPoint(JsonToken buffer)
        {
            var areTrailingDigitsOnlyZeros = true;
            int? indexOfDecimalPoint = null;
            int i;

            for (i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != DecimalPointCharacter) continue;

                indexOfDecimalPoint = i;
                break;
            }

            for (i++; i < buffer.Length; i++)
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
				public class ShortParser : IJsonTokenParser
	{
		private readonly Type _integerType = typeof (short);
		
        public const char DecimalPointCharacter = '.';
        public const char NegativeSign = '-';

        public const string MaxAsString = "1 ";
        public const string MinAsString = "1 ";

		public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return (token.JsonType == JsonTokenType.IntegerNumber || token.JsonType == JsonTokenType.FloatingPointNumber) && requestedType == _integerType;
        }

		public object ParseValue(JsonDeserializationContext context)
        {
            var token = context.Token;
            var decimalPointInfo = GetIndexOfDecimalPoint(token);
            var positionsBeforeDecimalPoint = token.Length;

            if (decimalPointInfo.IndexOfDecimalPoint != null)
            {
                if (decimalPointInfo.AreTrailingDigitsOnlyZeros == false)
                    throw new DeserializationException($"Could not deserialize value {token} because it is no integer, but a real number");

                positionsBeforeDecimalPoint = decimalPointInfo.IndexOfDecimalPoint.Value;
            }

            var currentIndex = 0;
            var isResultNegative = false;
            string overflowCompareString = null;

			if (NegativeSign != '+' && token[0] == NegativeSign)
            {
                if (token.Length > MinAsString.Length)
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");
                if (token.Length == MinAsString.Length)
                    overflowCompareString = MinAsString;
                isResultNegative = true;

                positionsBeforeDecimalPoint--;
                currentIndex++;
            }
			else if (token.Length > MaxAsString.Length)
                throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");
            else if (token.Length == MaxAsString.Length)
                overflowCompareString = MaxAsString;

			var result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                var digit = token[currentIndex] - '0';

                if (digit > overflowCompareString?[currentIndex] - '0')
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");

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

        private static DecimalPointInfo GetIndexOfDecimalPoint(JsonToken buffer)
        {
            var areTrailingDigitsOnlyZeros = true;
            int? indexOfDecimalPoint = null;
            int i;

            for (i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != DecimalPointCharacter) continue;

                indexOfDecimalPoint = i;
                break;
            }

            for (i++; i < buffer.Length; i++)
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
				public class UIntParser : IJsonTokenParser
	{
		private readonly Type _integerType = typeof (uint);
		
        public const char DecimalPointCharacter = '.';
        public const char NegativeSign = '+';

        public const string MaxAsString = "1";
        public const string MinAsString = "1";

		public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return (token.JsonType == JsonTokenType.IntegerNumber || token.JsonType == JsonTokenType.FloatingPointNumber) && requestedType == _integerType;
        }

		public object ParseValue(JsonDeserializationContext context)
        {
            var token = context.Token;
            var decimalPointInfo = GetIndexOfDecimalPoint(token);
            var positionsBeforeDecimalPoint = token.Length;

            if (decimalPointInfo.IndexOfDecimalPoint != null)
            {
                if (decimalPointInfo.AreTrailingDigitsOnlyZeros == false)
                    throw new DeserializationException($"Could not deserialize value {token} because it is no integer, but a real number");

                positionsBeforeDecimalPoint = decimalPointInfo.IndexOfDecimalPoint.Value;
            }

            var currentIndex = 0;
            var isResultNegative = false;
            string overflowCompareString = null;

			if (NegativeSign != '+' && token[0] == NegativeSign)
            {
                if (token.Length > MinAsString.Length)
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");
                if (token.Length == MinAsString.Length)
                    overflowCompareString = MinAsString;
                isResultNegative = true;

                positionsBeforeDecimalPoint--;
                currentIndex++;
            }
			else if (token.Length > MaxAsString.Length)
                throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");
            else if (token.Length == MaxAsString.Length)
                overflowCompareString = MaxAsString;

			var result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                var digit = token[currentIndex] - '0';

                if (digit > overflowCompareString?[currentIndex] - '0')
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");

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

        private static DecimalPointInfo GetIndexOfDecimalPoint(JsonToken buffer)
        {
            var areTrailingDigitsOnlyZeros = true;
            int? indexOfDecimalPoint = null;
            int i;

            for (i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != DecimalPointCharacter) continue;

                indexOfDecimalPoint = i;
                break;
            }

            for (i++; i < buffer.Length; i++)
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