using System;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class DateTimeParser : BaseIso8601DateTimeParser<DateTime>, IJsonStringToPrimitiveParser
    {
        public DateTimeKind DefaultDateTimeKind = DateTimeKind.Utc;

        public bool CanBeCached => true;

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

            year = ReadNumber(4, token, ref currentIndex);
            ExpectCharacter('-', token, ref currentIndex);
            month = ReadNumber(2, token, ref currentIndex);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTime;

            ExpectCharacter('-', token, ref currentIndex);
            day = ReadNumber(2, token, ref currentIndex);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTime;

            ExpectCharacter('T', token, ref currentIndex);
            kind = DateTimeKind.Unspecified;
            hour = ReadNumber(2, token, ref currentIndex);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTime;
            if (IsTimeZoneIndicator(token, ref currentIndex))
                goto CheckTimeZoneIndicator;

            ExpectCharacter(':', token, ref currentIndex);
            minute = ReadNumber(2, token, ref currentIndex);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTime;
            if (IsTimeZoneIndicator(token, ref currentIndex))
                goto CheckTimeZoneIndicator;

            ExpectCharacter(':', token, ref currentIndex);
            second = ReadNumber(2, token, ref currentIndex);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTime;
            if (IsTimeZoneIndicator(token, ref currentIndex))
                goto CheckTimeZoneIndicator;

            ExpectCharacter('.', token, ref currentIndex);
            millisecond = ReadNumber(3, token, ref currentIndex);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTime;

            CheckTimeZoneIndicator:
            var character = token[currentIndex++];
            if (character == 'Z')
                kind = DateTimeKind.Utc;
            else if (character == '+' || character == '-')
            {
                kind = DateTimeKind.Local;
                ReadNumber(2, token, ref currentIndex);
                if (IsEndOfToken(currentIndex, token.Length))
                    goto CreateDateTime;

                ExpectCharacter(':', token, ref currentIndex);
                ReadNumber(2, token, ref currentIndex);
            }

            if (IsEndOfToken(currentIndex, token.Length) == false)
                throw CreateException(token);

            CreateDateTime:
            try
            {
                return new DateTime(year, month, day, hour, minute, second, millisecond, kind);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw CreateException(token, ex);
        }
        }
    }
}