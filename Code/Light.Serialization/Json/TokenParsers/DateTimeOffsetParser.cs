using System;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class DateTimeOffsetParser : BaseIso8601DateTimeParser<DateTimeOffset>, IJsonStringToPrimitiveParser
    {
        public TimeSpan DefaultOffset = TimeSpan.Zero;

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

        private DateTimeOffset Parse(JsonToken token)
        {
            int year,
                month,
                day = 1,
                hour = 0,
                minute = 0,
                second = 0,
                millisecond = 0;
            var offset = DefaultOffset;

            var currentIndex = 1;
            year = ReadNumber(4, token, ref currentIndex);
            ExpectCharacter('-', token, ref currentIndex);
            month = ReadNumber(2, token, ref currentIndex);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTimeOffset;

            ExpectCharacter('-', token, ref currentIndex);
            day = ReadNumber(2, token, ref currentIndex);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTimeOffset;

            ExpectCharacter('T', token, ref currentIndex);
            hour = ReadNumber(2, token, ref currentIndex);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTimeOffset;
            if (IsTimeZoneIndicator(token, ref currentIndex))
                goto CheckTimeZoneIndicator;

            ExpectCharacter(':', token, ref currentIndex);
            minute = ReadNumber(2, token, ref currentIndex);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTimeOffset;
            if (IsTimeZoneIndicator(token, ref currentIndex))
                goto CheckTimeZoneIndicator;

            ExpectCharacter(':', token, ref currentIndex);
            second = ReadNumber(2, token, ref currentIndex);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTimeOffset;
            if (IsTimeZoneIndicator(token, ref currentIndex))
                goto CheckTimeZoneIndicator;

            ExpectCharacter('.', token, ref currentIndex);
            millisecond = ReadNumber(3, token, ref currentIndex);
            if (IsEndOfToken(currentIndex, token.Length))
                goto CreateDateTimeOffset;

            CheckTimeZoneIndicator:
            var character = token[currentIndex++];
            if (character == 'Z')
                offset = TimeSpan.Zero;
            else if (character == '+' || character == '-')
            {
                var hourOffset = ReadNumber(2, token, ref currentIndex);
                if (IsEndOfToken(currentIndex, token.Length))
                {
                    offset = TimeSpan.FromHours(character == '+' ? hourOffset : -hourOffset);
                    goto CreateDateTimeOffset;
                }

                ExpectCharacter(':', token, ref currentIndex);
                var minuteOffset = ReadNumber(2, token, ref currentIndex);
                if (character == '-')
                {
                    hourOffset = -hourOffset;
                    minuteOffset = -minuteOffset;
                }
                offset = new TimeSpan(hourOffset, minuteOffset, 0);
            }
            else
                throw CreateException(token);

            if (IsEndOfToken(currentIndex, token.Length) == false)
                throw CreateException(token);

            CreateDateTimeOffset:
            try
            {
                return new DateTimeOffset(year, month, day, hour, minute, second, millisecond, offset);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw CreateException(token, ex);
            }
        }
    }
}