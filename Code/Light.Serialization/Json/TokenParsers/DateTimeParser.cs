using System;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class DateTimeParser : BaseJsonStringToPrimitiveParser<DateTime>, IJsonStringToPrimitiveParser
    {
        private readonly Type _dateTimeType = typeof (DateTime);

        public DateTimeKind DefaultDateTimeKind = DateTimeKind.Utc;

        public bool CanBeCached => true;

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.String && requestedType == _dateTimeType;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            return Parse(context.Token);
        }

        public ParseResult TryParse(JsonToken token)
        {
            try
            {
                return new ParseResult(true, Parse(token));
            }
            catch (JsonDocumentException)
            {
                return new ParseResult(false);
            }
        }

        private DateTime Parse(JsonToken token)
        {
            int year,
                month,
                day = 1,
                hour = 0,
                minute = 0,
                second = 0,
                millisecond = 0;

            var currentIndex = 1;
            var kind = DefaultDateTimeKind;

            year = ReadNumber(4, ref currentIndex, ref token);
            ExpectCharacter('-', ref currentIndex, ref token);
            month = ReadNumber(2, ref currentIndex, ref token);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTime;

            ExpectCharacter('-', ref currentIndex, ref token);
            day = ReadNumber(2, ref currentIndex, ref token);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTime;

            ExpectCharacter('T', ref currentIndex, ref token);
            kind = DateTimeKind.Unspecified;
            hour = ReadNumber(2, ref currentIndex, ref token);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTime;
            if (IsTimeZoneIndicator(ref currentIndex, ref token))
                goto CheckTimeZoneIndicator;

            ExpectCharacter(':', ref currentIndex, ref token);
            minute = ReadNumber(2, ref currentIndex, ref token);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTime;
            if (IsTimeZoneIndicator(ref currentIndex, ref token))
                goto CheckTimeZoneIndicator;

            ExpectCharacter(':', ref currentIndex, ref token);
            second = ReadNumber(2, ref currentIndex, ref token);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTime;
            if (IsTimeZoneIndicator(ref currentIndex, ref token))
                goto CheckTimeZoneIndicator;

            ExpectCharacter('.', ref currentIndex, ref token);
            millisecond = ReadNumber(3, ref currentIndex, ref token);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTime;

            CheckTimeZoneIndicator:
            var character = token[currentIndex++];
            if (character == 'Z')
                kind = DateTimeKind.Utc;
            else if (character == '+' || character == '-')
            {
                kind = DateTimeKind.Local;
                ReadNumber(2, ref currentIndex, ref token);
                if (IsEndOfToken(currentIndex, token.Length))
                    goto CreateDateTime;

                ExpectCharacter(':', ref currentIndex, ref token);
                ReadNumber(2, ref currentIndex, ref token);
            }

            CreateDateTime:
            try
            {
                return new DateTime(year, month, day, hour, minute, second, millisecond, kind);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw CreateException(ref token, ex);
            }
        }

        private static void ExpectCharacter(char character, ref int currentIndex, ref JsonToken token)
        {
            if (token[currentIndex++] != character)
                throw CreateException(ref token);
        }

        private static bool IsEndOfToken(int currentIndex, int tokenLength)
        {
            return currentIndex == tokenLength - 1;
        }

        private static bool IsTimeZoneIndicator(ref int currentIndex, ref JsonToken token)
        {
            var character = token[currentIndex];
            return character == 'Z' || character == '+' || character == '-';
        }

        private static int ReadNumber(int expectedNumberOfDigits, ref int currentIndex, ref JsonToken token)
        {
            var result = 0;
            for (var base10Position = expectedNumberOfDigits; base10Position > 0; base10Position--, currentIndex++)
            {
                var digit = GetDigit(ref token, currentIndex);
                result += digit * CalculateBase(base10Position);
            }
            return result;
        }

        private static int CalculateBase(int base10Position)
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

        private static int GetDigit(ref JsonToken token, int currentIndex)
        {
            var character = token[currentIndex];
            if (char.IsDigit(character) == false)
                throw CreateException(ref token);

            return character - '0';
        }

        private static JsonDocumentException CreateException(ref JsonToken token, Exception innerException = null)
        {
            return new JsonDocumentException($"The specified token {token} does not represent a valid date time.", token, innerException);
        }
    }
}