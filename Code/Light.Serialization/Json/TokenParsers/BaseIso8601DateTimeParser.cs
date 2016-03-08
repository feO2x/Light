using System;

namespace Light.Serialization.Json.TokenParsers
{
    public abstract class BaseIso8601DateTimeParser<T> : BaseJsonStringToPrimitiveParser<T>
    {
        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.String && requestedType == typeof (T);
        }

        protected static void ExpectCharacter(char character, JsonToken token, ref int currentIndex)
        {
            if (token[currentIndex++] != character)
                throw CreateException(token);
        }

        protected static bool IsEndOfToken(int currentIndex, int tokenLength)
        {
            return currentIndex == tokenLength - 1;
        }

        protected static bool IsTimeZoneIndicator(JsonToken token, ref int currentIndex)
        {
            var character = token[currentIndex];
            return character == 'Z' || character == '+' || character == '-';
        }

        protected static int ReadNumber(int expectedNumberOfDigits, JsonToken token, ref int currentIndex)
        {
            var result = 0;
            for (var base10Position = expectedNumberOfDigits; base10Position > 0; base10Position--, currentIndex++)
            {
                var digit = GetDigit(token, currentIndex);
                result += digit * CalculateBase(base10Position);
            }
            return result;
        }

        protected static int CalculateBase(int base10Position)
        {
            if (base10Position == 1)
                return 1;

            var result = 10;
            for (var i = 2; i < base10Position; i++)
            {
                result *= 10;
            }
            return result;
        }

        protected static int GetDigit(JsonToken token, int currentIndex)
        {
            var character = token[currentIndex];
            if (char.IsDigit(character) == false)
                throw CreateException(token);

            return character - '0';
        }

        protected static JsonDocumentException CreateException(JsonToken token, Exception innerException = null)
        {
            return new JsonDocumentException($"The specified token {token} does not represent a valid date time.", token, innerException);
        }
    }
}