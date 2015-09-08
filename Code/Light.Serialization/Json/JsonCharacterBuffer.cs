using System;

namespace Light.Serialization.Json
{
    public struct JsonCharacterBuffer
    {
        private readonly char[] _buffer;
        private readonly int _startIndex;
        public readonly int Count;
        public readonly JsonType JsonType;
        private readonly bool _isCrossingBufferBoundary;

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
            Count = count;
            JsonType = jsonType;
            _isCrossingBufferBoundary = startIndex + count > buffer.Length;
        }

        public char this[int index]
        {
            get
            {
                if (index >= Count) throw new IndexOutOfRangeException($"index must not be larger than Count ({Count}), but you specified {index}.");
                if (index < 0) throw new IndexOutOfRangeException($"index must not be less than zero, but you specified {index}.");

                return _buffer[(_startIndex + index) % _buffer.Length];
            }
        }

        public override string ToString()
        {
            if (_isCrossingBufferBoundary == false)
                return new string(_buffer, _startIndex, Count);

            var characterArray = new char[Count];
            for (var i = 0; i < Count; i++)
            {
                characterArray[i] = this[i];
            }
            return new string(characterArray);
        }
    }
}