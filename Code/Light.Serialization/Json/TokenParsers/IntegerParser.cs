﻿using System;

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

			if (token[0] == NegativeSign)
            {
                if (positionsBeforeDecimalPoint > MinAsString.Length)
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");
                if (positionsBeforeDecimalPoint == MinAsString.Length)
                    overflowCompareString = MinAsString;
                isResultNegative = true;

                positionsBeforeDecimalPoint--;
                currentIndex++;
            }
			else if (positionsBeforeDecimalPoint > MaxAsString.Length)
                throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");
            else if (positionsBeforeDecimalPoint == MaxAsString.Length)
                overflowCompareString = MaxAsString;
			
			
			int  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
			bool isDefinitelyInRange = false;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                int digit = token[currentIndex] - '0';

				if (isDefinitelyInRange == false)
                {
                    var overflowCompareDigit = overflowCompareString?[currentIndex] - '0';
                    if (digit < overflowCompareDigit)
                        isDefinitelyInRange = true;
                    else if (digit > overflowCompareDigit)
                        throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");
                }

                try
                { 
                    result += checked((int)(digit * CalculateBase(currentPositionBeforeDecimalPoint)));
                }
                catch (OverflowException e)
                {
                    // The following line displays information about the error.
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type uint.");
                }

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

			if(result > int.MaxValue || result < int.MinValue)
				throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type int.");

			if (isResultNegative)
                return (int) -result;
			
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
            string overflowCompareString = null;

			
						if (positionsBeforeDecimalPoint > MaxAsString.Length)
                throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type uint.");
            if (positionsBeforeDecimalPoint == MaxAsString.Length)
                overflowCompareString = MaxAsString;
			
			uint  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
			bool isDefinitelyInRange = false;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                int digit = token[currentIndex] - '0';

				if (isDefinitelyInRange == false)
                {
                    var overflowCompareDigit = overflowCompareString?[currentIndex] - '0';
                    if (digit < overflowCompareDigit)
                        isDefinitelyInRange = true;
                    else if (digit > overflowCompareDigit)
                        throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type uint.");
                }

                try
                { 
                    result += checked((uint)(digit * CalculateBase(currentPositionBeforeDecimalPoint)));
                }
                catch (OverflowException e)
                {
                    // The following line displays information about the error.
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type uint.");
                }

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

			if(result > uint.MaxValue || result < uint.MinValue)
				throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type uint.");

			
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

			if (token[0] == NegativeSign)
            {
                if (positionsBeforeDecimalPoint > MinAsString.Length)
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type short.");
                if (positionsBeforeDecimalPoint == MinAsString.Length)
                    overflowCompareString = MinAsString;
                isResultNegative = true;

                positionsBeforeDecimalPoint--;
                currentIndex++;
            }
			else if (positionsBeforeDecimalPoint > MaxAsString.Length)
                throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type short.");
            else if (positionsBeforeDecimalPoint == MaxAsString.Length)
                overflowCompareString = MaxAsString;
			
			
			short  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
			bool isDefinitelyInRange = false;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                int digit = token[currentIndex] - '0';

				if (isDefinitelyInRange == false)
                {
                    var overflowCompareDigit = overflowCompareString?[currentIndex] - '0';
                    if (digit < overflowCompareDigit)
                        isDefinitelyInRange = true;
                    else if (digit > overflowCompareDigit)
                        throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type short.");
                }

                try
                { 
                    result += checked((short)(digit * CalculateBase(currentPositionBeforeDecimalPoint)));
                }
                catch (OverflowException e)
                {
                    // The following line displays information about the error.
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type uint.");
                }

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

			if(result > short.MaxValue || result < short.MinValue)
				throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type short.");

			if (isResultNegative)
                return (short) -result;
			
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

			if (token[0] == NegativeSign)
            {
                if (positionsBeforeDecimalPoint > MinAsString.Length)
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type ushort.");
                if (positionsBeforeDecimalPoint == MinAsString.Length)
                    overflowCompareString = MinAsString;
                isResultNegative = true;

                positionsBeforeDecimalPoint--;
                currentIndex++;
            }
			else if (positionsBeforeDecimalPoint > MaxAsString.Length)
                throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type ushort.");
            else if (positionsBeforeDecimalPoint == MaxAsString.Length)
                overflowCompareString = MaxAsString;
			
			
			ushort  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
			bool isDefinitelyInRange = false;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                int digit = token[currentIndex] - '0';

				if (isDefinitelyInRange == false)
                {
                    var overflowCompareDigit = overflowCompareString?[currentIndex] - '0';
                    if (digit < overflowCompareDigit)
                        isDefinitelyInRange = true;
                    else if (digit > overflowCompareDigit)
                        throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type ushort.");
                }

                try
                { 
                    result += checked((ushort)(digit * CalculateBase(currentPositionBeforeDecimalPoint)));
                }
                catch (OverflowException e)
                {
                    // The following line displays information about the error.
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type uint.");
                }

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

			if(result > ushort.MaxValue || result < ushort.MinValue)
				throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type ushort.");

			if (isResultNegative)
                return (ushort) -result;
			
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

			if (token[0] == NegativeSign)
            {
                if (positionsBeforeDecimalPoint > MinAsString.Length)
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type byte.");
                if (positionsBeforeDecimalPoint == MinAsString.Length)
                    overflowCompareString = MinAsString;
                isResultNegative = true;

                positionsBeforeDecimalPoint--;
                currentIndex++;
            }
			else if (positionsBeforeDecimalPoint > MaxAsString.Length)
                throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type byte.");
            else if (positionsBeforeDecimalPoint == MaxAsString.Length)
                overflowCompareString = MaxAsString;
			
			
			byte  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
			bool isDefinitelyInRange = false;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                int digit = token[currentIndex] - '0';

				if (isDefinitelyInRange == false)
                {
                    var overflowCompareDigit = overflowCompareString?[currentIndex] - '0';
                    if (digit < overflowCompareDigit)
                        isDefinitelyInRange = true;
                    else if (digit > overflowCompareDigit)
                        throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type byte.");
                }

                try
                { 
                    result += checked((byte)(digit * CalculateBase(currentPositionBeforeDecimalPoint)));
                }
                catch (OverflowException e)
                {
                    // The following line displays information about the error.
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type uint.");
                }

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

			if(result > byte.MaxValue || result < byte.MinValue)
				throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type byte.");

			if (isResultNegative)
                return (byte) -result;
			
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

			if (token[0] == NegativeSign)
            {
                if (positionsBeforeDecimalPoint > MinAsString.Length)
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type sbyte.");
                if (positionsBeforeDecimalPoint == MinAsString.Length)
                    overflowCompareString = MinAsString;
                isResultNegative = true;

                positionsBeforeDecimalPoint--;
                currentIndex++;
            }
			else if (positionsBeforeDecimalPoint > MaxAsString.Length)
                throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type sbyte.");
            else if (positionsBeforeDecimalPoint == MaxAsString.Length)
                overflowCompareString = MaxAsString;
			
			
			sbyte  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
			bool isDefinitelyInRange = false;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                int digit = token[currentIndex] - '0';

				if (isDefinitelyInRange == false)
                {
                    var overflowCompareDigit = overflowCompareString?[currentIndex] - '0';
                    if (digit < overflowCompareDigit)
                        isDefinitelyInRange = true;
                    else if (digit > overflowCompareDigit)
                        throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type sbyte.");
                }

                try
                { 
                    result += checked((sbyte)(digit * CalculateBase(currentPositionBeforeDecimalPoint)));
                }
                catch (OverflowException e)
                {
                    // The following line displays information about the error.
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type uint.");
                }

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

			if(result > sbyte.MaxValue || result < sbyte.MinValue)
				throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type sbyte.");

			if (isResultNegative)
                return (sbyte) -result;
			
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
        public const string MinAsString = "-9223372036854775808";

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

			if (token[0] == NegativeSign)
            {
                if (positionsBeforeDecimalPoint > MinAsString.Length)
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type long.");
                if (positionsBeforeDecimalPoint == MinAsString.Length)
                    overflowCompareString = MinAsString;
                isResultNegative = true;

                positionsBeforeDecimalPoint--;
                currentIndex++;
            }
			else if (positionsBeforeDecimalPoint > MaxAsString.Length)
                throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type long.");
            else if (positionsBeforeDecimalPoint == MaxAsString.Length)
                overflowCompareString = MaxAsString;
			
			
			long  result = 0;
			var currentPositionBeforeDecimalPoint = positionsBeforeDecimalPoint;
			bool isDefinitelyInRange = false;
            while (currentPositionBeforeDecimalPoint > 0)
            {
                int digit = token[currentIndex] - '0';

				if (isDefinitelyInRange == false)
                {
                    var overflowCompareDigit = overflowCompareString?[currentIndex] - '0';
                    if (digit < overflowCompareDigit)
                        isDefinitelyInRange = true;
                    else if (digit > overflowCompareDigit)
                        throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type long.");
                }

                try
                { 
                    result += checked((long)(digit * CalculateBase(currentPositionBeforeDecimalPoint)));
                }
                catch (OverflowException e)
                {
                    // The following line displays information about the error.
                    throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type uint.");
                }

                currentIndex++;
                currentPositionBeforeDecimalPoint--;
            }

			if(result > long.MaxValue || result < long.MinValue)
				throw new DeserializationException($"Could not deserialize value {token} because it produces an overflow for type long.");

			if (isResultNegative)
                return (long) -result;
			
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