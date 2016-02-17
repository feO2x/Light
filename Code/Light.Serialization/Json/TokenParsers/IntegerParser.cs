using System;

namespace Light.Serialization.Json.TokenParsers
{


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
	}
			
	public class UIntParser : IJsonTokenParser
	{
		private readonly Type _integerType = typeof (uint);
		
        public const char DecimalPointCharacter = '.';
        public const char NegativeSign = '';

        public const string MaxAsString = "1 ";
        public const string MinAsString = "1 ";

		public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return (token.JsonType == JsonTokenType.IntegerNumber || token.JsonType == JsonTokenType.FloatingPointNumber) && requestedType == _integerType;
        }
	}
			}