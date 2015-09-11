using Light.Core;
using System;

namespace Light.Serialization.Json
{
    public sealed class SingleBufferJsonReader : IJsonReader
    {
        private readonly char[] _buffer;
        private readonly KnownJsonTokens _knownJsonTokens;
        private int _currentIndex;

        public SingleBufferJsonReader(char[] buffer)
            : this(buffer, new KnownJsonTokens())
        {
            
        }

        public SingleBufferJsonReader(char[] buffer, KnownJsonTokens knownJsonTokens)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (knownJsonTokens == null) throw new ArgumentNullException(nameof(knownJsonTokens));

            _buffer = buffer;
            _knownJsonTokens = knownJsonTokens;
        }

        public JsonCharacterBuffer ReadNextValue()
        {
            IgnoreWhitespace();

            var firstCharacter = _buffer[_currentIndex++];
            if (char.IsDigit(firstCharacter))
                return ReadPositiveNumber(firstCharacter);
            if (firstCharacter == _knownJsonTokens.NegativeSign)
                return ReadNegativeNumber();
            if (firstCharacter == _knownJsonTokens.StringDelimiter)
                return ReadString();
            if (firstCharacter == _knownJsonTokens.FalseToken[0])
                return ReadConstantToken(_knownJsonTokens.FalseToken, JsonType.False);
            if (firstCharacter == _knownJsonTokens.TrueToken[0])
                return ReadConstantToken(_knownJsonTokens.TrueToken, JsonType.True);
            if (firstCharacter == _knownJsonTokens.NullToken[0])
                return ReadConstantToken(_knownJsonTokens.NullToken, JsonType.Null);
            if (firstCharacter == _knownJsonTokens.StartCollectionCharacter)
                return CreateBuffer(_currentIndex - 1, JsonType.Array);

            throw new NotImplementedException();
        }

        public bool CheckEndOfCollection()
        {
            IgnoreWhitespace();
            var currentCharacter = _buffer[_currentIndex++];
            if (currentCharacter == _knownJsonTokens.ValueSeperator)
                return false;
            if (currentCharacter == _knownJsonTokens.StopCollectionCharacter)
                return true;

            throw new DeserializationException($"Unexpected JSON token: expected either either end of collection ({_knownJsonTokens.StopCollectionCharacter}) or value separator ({_knownJsonTokens.ValueSeperator}), but found {currentCharacter}.");
        }

        private void IgnoreWhitespace()
        {
            while (char.IsWhiteSpace(_buffer[_currentIndex]))
                _currentIndex++;
        }

        private JsonCharacterBuffer ReadPositiveNumber(char firstCharacter)
        {
            var startIndex = _currentIndex - 1;
            // The first character is definitely a digit, otherwise this method would not have been called
            // Check if it is zero
            if (firstCharacter == '0')
            {
                // If yes, then there must follow a decimal point, an exponent, or the number must end
                if (IsNumberFinishedOrNumberWithDecimalPart())
                    return CreateBuffer(startIndex, JsonType.Number);

                ReadUntilEndOfToken();
                throw CreateDeserializationException(startIndex, JsonType.Number);
            }
            // Read all digits until we hit the end or a decimal point
            while (true)
            {
                _currentIndex++;
                if (IsEndOfToken())
                    return CreateBuffer(startIndex, JsonType.Number);

                var currentCharacter = _buffer[_currentIndex];
                if (char.IsDigit(currentCharacter))
                    continue;

                if (CheckDecimalPartOfNumber(currentCharacter))
                    return CreateBuffer(startIndex, JsonType.Number);

                throw CreateDeserializationException(startIndex, JsonType.Number);
            }
        }

        private JsonCharacterBuffer ReadNegativeNumber()
        {
            var startIndex = _currentIndex - 1;
            // The first character is definitely a negative sign, otherwise this method would not have been called
            // There must be at least one digit
            if (IsEndOfToken())
                throw CreateDeserializationException(startIndex, JsonType.Number);
            
            var currentCharacter = _buffer[_currentIndex];
            // Check if it is zero
            if (currentCharacter == '0')
            {
                // If yes, then there must follow a decimal point, an exponent, or the number must end
                if (IsNumberFinishedOrNumberWithDecimalPart())
                    return CreateBuffer(startIndex, JsonType.Number);

                ReadUntilEndOfToken();
                throw CreateDeserializationException(startIndex, JsonType.Number);
            }

            // Read all digits until we hit the end or a decimal point
            while (true)
            {
                _currentIndex++;
                if (IsEndOfToken())
                    return CreateBuffer(startIndex, JsonType.Number);

                currentCharacter = _buffer[_currentIndex];
                if (char.IsDigit(currentCharacter))
                    continue;

                if (CheckDecimalPartOfNumber(currentCharacter))
                    return CreateBuffer(startIndex, JsonType.Number);

                throw CreateDeserializationException(startIndex, JsonType.Number);
            }
        }

        private bool CheckDecimalPartOfNumber(char currentCharacter)
        {
            if (currentCharacter != _knownJsonTokens.DecimalPoint)
                return CheckExponentOfNumber(currentCharacter);

            // Here we definitely have a decimal point
            // If the token ends now, this is an error
            _currentIndex++;
            if (IsEndOfToken())
                return false;

            // There must be at least one digit
            currentCharacter = _buffer[_currentIndex++];
            if (char.IsDigit(currentCharacter) == false)
                return false;

            while (true)
            {
                if (IsEndOfToken())
                    return true;

                currentCharacter = _buffer[_currentIndex++];
                if (char.IsDigit(currentCharacter) == false)
                    return false;
            }
        }

        private bool IsNumberFinishedOrNumberWithDecimalPart()
        {
            // Check if the end of the buffer is reached.
            return IsEndOfToken() || CheckDecimalPartOfNumber(_buffer[_currentIndex]);
        }

        private bool CheckExponentOfNumber(char currentCharacter)
        {
            // Check if the e or E sign is present
            if (_knownJsonTokens.ExponentialTokens.Contains(currentCharacter) == false)
                return false;

            _currentIndex++;
            if (IsEndOfToken())
                return false;

            // Check if possible + or - sign is present
            currentCharacter = _buffer[_currentIndex];
            if (currentCharacter == _knownJsonTokens.NegativeSign || currentCharacter == _knownJsonTokens.PositiveSign)
            {
                _currentIndex++;
                if (IsEndOfToken())
                    return false;
            }

            // Read all digits of the exponent
            // There must be at least one digit
            currentCharacter = _buffer[_currentIndex++];
            if (char.IsDigit(currentCharacter) == false)
                return false;

            while (true)
            {
                if (IsEndOfToken())
                    return true;

                currentCharacter = _buffer[_currentIndex++];
                if (char.IsDigit(currentCharacter) == false)
                    return false;
            }
        }

        private JsonCharacterBuffer ReadString()
        {
            // The first digit is a string delimiter, otherwise this method would not have been called
            var startIndex = _currentIndex - 1;
            
            // Read in all following characters until we get a valid string delimiter that ends the JSON string
            var isPreviousCharacterEscapeCharacter = false;
            while (true)
            {
                var currentCharacter = _buffer[_currentIndex++];

                // Check if the previous character was an escape character
                if (isPreviousCharacterEscapeCharacter)
                {
                    // If yes then check if the current character is a specially escaped character that only has one letter
                    foreach (var singleEscapedCharacter in _knownJsonTokens.SingleEscapedCharacters)
                    {
                        if (singleEscapedCharacter.ValueAfterEscapeCharacter != currentCharacter) continue;

                        isPreviousCharacterEscapeCharacter = false;
                        goto CheckEndOfBuffer;
                    }

                    // Otherwise check if this is an escape sequence of four hexadecimal digits
                    if (currentCharacter == _knownJsonTokens.HexadecimalEscapeIndicator)
                    {
                        if (CheckFourHexadecimalDigitsOfJsonEscapeSequence() == false)
                            goto ThrowException;

                        isPreviousCharacterEscapeCharacter = false;
                        goto CheckEndOfBuffer;
                    }
                    // When non of the above conditions fit, then the string is not a valid Json string
                    ThrowException:
                    ReadUntilEndOfToken();
                    throw CreateDeserializationException(startIndex, JsonType.String);
                }

                // If not, then treat this character as a normal one
                if (currentCharacter == _knownJsonTokens.StringDelimiter)
                    return CreateBuffer(startIndex, JsonType.String);

                // Set the boolean value indicating that the next character is part of an escape sequence
                if (currentCharacter == _knownJsonTokens.StringEscapeCharacter)
                    isPreviousCharacterEscapeCharacter = true;

                // If the end of the buffer is reached for the next index, then throw an exception
                // because this string is not a valid JSON string
                CheckEndOfBuffer:
                if (IsEndOfBuffer())
                    throw CreateDeserializationException(startIndex, JsonType.String);
            }
        }

        private bool CheckFourHexadecimalDigitsOfJsonEscapeSequence()
        {
            // There must be exactly 4 digits after the u
            for (var i = 0; i < 4; i++)
            {
                if (IsEndOfToken())
                    return false;

                if (_buffer[_currentIndex++].IsHexadecimal() == false)
                    return false;
            }

            return true;
        }

        private bool IsEndOfToken()
        {
            return _currentIndex == _buffer.Length || char.IsWhiteSpace(_buffer[_currentIndex]);
        }

        private bool IsEndOfBuffer()
        {
            return _currentIndex == _buffer.Length;
        }

        private JsonCharacterBuffer ReadConstantToken(string expectedToken, JsonType type)
        {
            var startIndex = _currentIndex - 1;
            for (var i = 1; i < expectedToken.Length; i++)
            {
                if (_currentIndex == _buffer.Length)
                    throw CreateDeserializationException(startIndex, type);

                if (_buffer[_currentIndex++] == expectedToken[i])
                    continue;

                ReadUntilEndOfToken();
                throw CreateDeserializationException(startIndex, type);
            }

            return CreateBuffer(startIndex, type);
        }

        private void ReadUntilEndOfToken()
        {
            while (true)
            {
                if (IsEndOfToken())
                    break;
                _currentIndex++;
            }
        }

        private JsonCharacterBuffer CreateBuffer(int startIndex, JsonType type)
        {
            return new JsonCharacterBuffer(_buffer, startIndex, _currentIndex - startIndex, type);
        }

        private DeserializationException CreateDeserializationException(int startIndex, JsonType type)
        {
            var buffer = CreateBuffer(startIndex, type);
            return new DeserializationException($"Cannot deserialize value {buffer} to {type}.");
        }
    }
}
