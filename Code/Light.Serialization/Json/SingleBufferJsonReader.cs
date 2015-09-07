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
            IgnoreWhitespace();

            var firstCharacter = _buffer[_currentIndex++];
            if (char.IsDigit(firstCharacter) || firstCharacter == '-')
                return ReadNumber();
            if (firstCharacter == '"')
                return ReadString();
            if (firstCharacter == 'f' || firstCharacter == 't')
                return ReadBool();
            if (firstCharacter == 'n')
                return ReadNull();

            throw new NotImplementedException();
        }

        private void IgnoreWhitespace()
        {
            while (_buffer[_currentIndex] == ' ')
                _currentIndex++;
        }

        private JsonCharacterBuffer ReadNull()
        {
            var startIndex = _currentIndex - 1;
            while (true)
            {
                if (_currentIndex == _buffer.Length)
                    return new JsonCharacterBuffer(_buffer, startIndex, _currentIndex - startIndex, JsonType.Null);
                _currentIndex++;
            }
        }

        private JsonCharacterBuffer ReadBool()
        {
            var startIndex = _currentIndex - 1;
            while (true)
            {
                if (_currentIndex == _buffer.Length)
                    return new JsonCharacterBuffer(_buffer, startIndex, _currentIndex - startIndex, JsonType.Boolean);
                _currentIndex++;
            }
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

        private JsonCharacterBuffer ReadString()
        {
            var startIndex = _currentIndex - 1;
            while (true)
            {
                if (_currentIndex == _buffer.Length)
                    return new JsonCharacterBuffer(_buffer, startIndex, _currentIndex - startIndex, JsonType.String);
                _currentIndex++;
            }
        }
    }
}
