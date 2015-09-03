using System;

namespace Light.Serialization.Json
{
    public sealed class SingleBufferJsonReader : IJsonReader
    {
        private readonly char[] _buffer;
        private int _currentIndex;

        public SingleBufferJsonReader(char[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            _buffer = buffer;
        }

        public JsonCharacterBuffer ReadNextValue()
        {
            var nextCharacter = _buffer[_currentIndex++];
            if (char.IsDigit(nextCharacter) || nextCharacter == '-')
                return ReadNumber();

            throw new NotImplementedException();
        }

        private JsonCharacterBuffer ReadNumber()
        {
            var startIndex = _currentIndex - 1;
            while (true)
            {
                if (_currentIndex == _buffer.Length)
                    return new JsonCharacterBuffer(_buffer, startIndex, _currentIndex - startIndex, JsonType.Number);
                _currentIndex++;
            }
        }
    }
}
