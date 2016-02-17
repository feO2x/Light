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

			int  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                int digit = (int)(token[currentIndex] - '0');

                if (digit > overflowCompareString?[currentIndex] - '0')
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");

                result += (int)(digit * CalculateBase(currentPositionBeforeDecimalPoint));

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

            int result = 10;
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

        public const string MaxAsString = "4294967295";
        public const string MinAsString = "0";

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

			uint  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                uint digit = (uint)(token[currentIndex] - '0');

                if (digit > overflowCompareString?[currentIndex] - '0')
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");

                result += (uint)(digit * CalculateBase(currentPositionBeforeDecimalPoint));

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

            if (isResultNegative)
                return -result;

            return result;
        }

		private static uint CalculateBase(int positionsBeforeDecimalPoint)
        {
            if (positionsBeforeDecimalPoint == 1)
                return 1;

            uint result = 10;
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

        public const string MaxAsString = "32767";
        public const string MinAsString = "-32768";

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

			short  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                short digit = (short)(token[currentIndex] - '0');

                if (digit > overflowCompareString?[currentIndex] - '0')
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");

                result += (short)(digit * CalculateBase(currentPositionBeforeDecimalPoint));

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

            if (isResultNegative)
                return -result;

            return result;
        }

		private static short CalculateBase(int positionsBeforeDecimalPoint)
        {
            if (positionsBeforeDecimalPoint == 1)
                return 1;

            short result = 10;
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
				public class UShortParser : IJsonTokenParser
	{
		private readonly Type _integerType = typeof (ushort);
		
        public const char DecimalPointCharacter = '.';
        public const char NegativeSign = '-';

        public const string MaxAsString = "65535";
        public const string MinAsString = "0";

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

			ushort  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                ushort digit = (ushort)(token[currentIndex] - '0');

                if (digit > overflowCompareString?[currentIndex] - '0')
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");

                result += (ushort)(digit * CalculateBase(currentPositionBeforeDecimalPoint));

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

            if (isResultNegative)
                return -result;

            return result;
        }

		private static ushort CalculateBase(int positionsBeforeDecimalPoint)
        {
            if (positionsBeforeDecimalPoint == 1)
                return 1;

            ushort result = 10;
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
				public class ByteParser : IJsonTokenParser
	{
		private readonly Type _integerType = typeof (byte);
		
        public const char DecimalPointCharacter = '.';
        public const char NegativeSign = '-';

        public const string MaxAsString = "255";
        public const string MinAsString = "0";

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
                if (positionsBeforeDecimalPoint > MinAsString.Length)
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");
                if (token.Length == MinAsString.Length)
                    overflowCompareString = MinAsString;
                isResultNegative = true;

                positionsBeforeDecimalPoint--;
                currentIndex++;
            }
			else if (positionsBeforeDecimalPoint > MaxAsString.Length)
                throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");
            else if (positionsBeforeDecimalPoint == MaxAsString.Length)
                overflowCompareString = MaxAsString;

			byte  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                byte digit = (byte)(token[currentIndex] - '0');

                if (digit > overflowCompareString?[currentIndex] - '0')
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");

                result += (byte)(digit * CalculateBase(currentPositionBeforeDecimalPoint));

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

            if (isResultNegative)
                return -result;

            return result;
        }

		private static byte CalculateBase(int positionsBeforeDecimalPoint)
        {
            if (positionsBeforeDecimalPoint == 1)
                return 1;

            byte result = 10;
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
				public class SByteParser : IJsonTokenParser
	{
		private readonly Type _integerType = typeof (sbyte);
		
        public const char DecimalPointCharacter = '.';
        public const char NegativeSign = '-';

        public const string MaxAsString = "127";
        public const string MinAsString = "-128";

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

			sbyte  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                sbyte digit = (sbyte)(token[currentIndex] - '0');

                if (digit > overflowCompareString?[currentIndex] - '0')
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");

                result += (sbyte)(digit * CalculateBase(currentPositionBeforeDecimalPoint));

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

            if (isResultNegative)
                return -result;

            return result;
        }

		private static sbyte CalculateBase(int positionsBeforeDecimalPoint)
        {
            if (positionsBeforeDecimalPoint == 1)
                return 1;

            sbyte result = 10;
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
				public class LongParser : IJsonTokenParser
	{
		private readonly Type _integerType = typeof (long);
		
        public const char DecimalPointCharacter = '.';
        public const char NegativeSign = '-';

        public const string MaxAsString = "9223372036854775807";
        public const string MinAsString = "–9223372036854775808";

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

			long  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                long digit = (long)(token[currentIndex] - '0');

                if (digit > overflowCompareString?[currentIndex] - '0')
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");

                result += (long)(digit * CalculateBase(currentPositionBeforeDecimalPoint));

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

            if (isResultNegative)
                return -result;

            return result;
        }

		private static long CalculateBase(int positionsBeforeDecimalPoint)
        {
            if (positionsBeforeDecimalPoint == 1)
                return 1;

            long result = 10;
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