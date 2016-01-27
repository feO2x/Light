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
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex),
                                                      $"startIndex must not be less than zero, but you specified {startIndex}.");
            if (startIndex >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex),
                                                      $"startIndex must not be greater or equal to buffer.Length, but you specified {startIndex}.");
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), 
                                                      $"count must not be less than 0, but you specified {length}.");
            if (length > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(length),
                                                      $"count must not be greater than buffer.Length, but you specified {length}.");

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
                if (index >= Length) throw new IndexOutOfRangeException($"index must not be larger than Count ({Length}), but you specified {index}.");
                if (index < 0) throw new IndexOutOfRangeException($"index must not be less than zero, but you specified {index}.");

                return _buffer[(_startIndex + index) % _buffer.Length];
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
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), $"startIndex must not be less than zero, but you specified {startIndex}.");
            if (numberOfCharacters < 1)
                throw new ArgumentOutOfRangeException(nameof(numberOfCharacters), $"numberOfCharacters must not be less than one, but you specified {numberOfCharacters}.");
            var numberOfCharactersLeft = Length - startIndex;
            if (numberOfCharacters > numberOfCharactersLeft)
                throw new ArgumentOutOfRangeException($"You specified that {numberOfCharacters} characters should be inserted into the string, but only {numberOfCharactersLeft} characters are left from your starting position ({startIndex})." );

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
            if (JsonType != JsonTokenType.String)
                throw new InvalidOperationException($"This method should only be called when the JsonType of this token is String, but it is actually {JsonType}.");

            return ToString(1, Length - 2);
        }
    }
}