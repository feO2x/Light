using System;

namespace Light.Serialization.Json
{
    public struct JsonCharacterBuffer
    {
        private readonly char[] _buffer;
        private readonly int _startIndex;
        private readonly int _count;
        private readonly JsonType _jsonType;

        public JsonCharacterBuffer(char[] buffer, int startIndex, int count, JsonType jsonType)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex),
                                                      $"startIndex must not be less than zero, but you specified {startIndex}.");
            if (startIndex >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex),
                                                      $"startIndex must not be greater or equal to buffer.Length, but you specified {startIndex}.");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), 
                                                      $"count must not be less than 0, but you specified {count}.");
            if (count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count),
                                                      $"count must not be greater than buffer.Length, but you specified {count}.");


            _buffer = buffer;
            _startIndex = startIndex;
            _count = count;
            _jsonType = jsonType;
        }

        public int Count => _count;

        public JsonType JsonType => _jsonType;

        public char this[int index]
        {
            get
            {
                if (index >= _count) throw new IndexOutOfRangeException($"index must not be larger than Count ({_count}), but you specified {index}.");
                if (index < 0) throw new IndexOutOfRangeException($"index must not be less than zero, but you specified {index}.");

                return _buffer[(_startIndex + index) % _count];
            }
        }

        private bool IsOverflowing => _startIndex + _count > _buffer.Length;

        public override string ToString()
        {
            if (IsOverflowing == false)
                return new string(_buffer, _startIndex, _count);

            var characterArray = new char[_count];
            for (var i = 0; i < _count; i++)
            {
                characterArray[i] = this[i];
            }
            return new string(characterArray);
        }
    }
}