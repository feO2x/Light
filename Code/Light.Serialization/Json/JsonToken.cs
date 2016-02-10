using Light.GuardClauses;
using System;

namespace Light.Serialization.Json
{
    public struct JsonToken
    {
        private readonly char[] _buffer;
        private readonly int _startIndex;
        public readonly int Length;
        public readonly JsonTokenType JsonType;
        private readonly bool _isCrossingBufferBoundary;

        public JsonToken(char[] buffer, int startIndex, int length, JsonTokenType jsonType)
        {
            buffer.MustNotBeNull(nameof(buffer));
            startIndex.MustNotBeLessThan(0, nameof(startIndex));
            startIndex.MustNotBeGreaterThanOrEqualTo(buffer.Length, nameof(startIndex));
            length.MustNotBeLessThan(0, nameof(length));
            length.MustNotBeGreaterThan(buffer.Length, nameof(length));

            _buffer = buffer;
            _startIndex = startIndex;
            Length = length;
            JsonType = jsonType;
            _isCrossingBufferBoundary = startIndex + length > buffer.Length;
        }

        public char this[int index]
        {
            get
            {
                index.MustNotBeLessThan(0, nameof(index));
                index.MustNotBeGreaterThanOrEqualTo(Length, nameof(index));

                return _buffer[(_startIndex + index)%_buffer.Length];
            }
        }

        public override string ToString()
        {
            if (_isCrossingBufferBoundary == false)
                return new string(_buffer, _startIndex, Length);

            var characterArray = new char[Length];
            for (var i = 0; i < Length; i++)
            {
                characterArray[i] = this[i];
            }
            return new string(characterArray);
        }

        public string ToString(int startIndex, int numberOfCharacters)
        {
            startIndex.MustNotBeLessThan(0, nameof(startIndex));
            numberOfCharacters.MustNotBeLessThan(1, nameof(numberOfCharacters));
            var numberOfCharactersLeft = Length - startIndex;
            numberOfCharacters.MustNotBeGreaterThan(numberOfCharactersLeft, nameof(numberOfCharacters));

            if (_isCrossingBufferBoundary == false)
                return new string(_buffer, _startIndex + startIndex, numberOfCharacters);

            var characterArray = new char[numberOfCharacters];
            for (var i = 0; i < numberOfCharacters; i++)
            {
                characterArray[i] = this[i];
            }
            return new string(characterArray);
        }

        public string ToStringWithoutQuotationMarks()
        {
            JsonType.MustBe(JsonTokenType.String,
                            new InvalidOperationException($"This method should only be called when the JsonType of this token is String, but it is actually {JsonType}."));

            return ToString(1, Length - 2);
        }
    }
}