using System;
using System.Collections.Generic;
using Light.Core;

namespace Light.Serialization.Json
{
    public sealed class SingleBufferJsonReader : IJsonReader
    {
        private readonly char[] _buffer;
        private int _currentIndex;

        private string _trueToken = "true";
        public string TrueToken
        {
            get { return _trueToken; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _trueToken = value;
            }
        }

        private string _falseToken = "false";
        public string FalseToken
        {
            get { return _falseToken; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _falseToken = value;
            }
        }

        private string _nullToken = "null";
        public string NullToken
        {
            get { return _nullToken; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _nullToken = value;
            }
        }

        private char _stringDelimiter = '"';
        public char StringDelimiter
        {
            get { return _stringDelimiter; }
            set { _stringDelimiter = value; }
        }

        private char _decimalPoint = '.';

        public char DecimalPoint
        {
            get { return _decimalPoint; }
            set { _decimalPoint = value; }
        }

        private IList<char> _exponentialTokens = new[] { 'e', 'E' };
        public IList<char> ExponentialTokens
        {
            get { return _exponentialTokens; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _exponentialTokens = value;
            }
        }

        private char _positiveSign = '+';
        public char PositiveSign
        {
            get { return _positiveSign; }
            set { _positiveSign = value; }
        }

        private char _negativeSign = '-';
        public char NegativeSign
        {
            get { return _negativeSign; }
            set { _negativeSign = value; }
        }

        private char _stringEscapeCharacter = '\\';

        public char StringEscapeCharacter
        {
            get { return _stringEscapeCharacter; }
            set { _stringEscapeCharacter = value; }
        }

        public SingleBufferJsonReader(char[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            _buffer = buffer;
        }

        public JsonCharacterBuffer ReadNextValue()
        {
            IgnoreWhitespace();

            var firstCharacter = _buffer[_currentIndex++];
            if (char.IsDigit(firstCharacter))
                return ReadPositiveNumber(firstCharacter);
            if (firstCharacter == _negativeSign)
                return ReadNegativeNumber();
            if (firstCharacter == _stringDelimiter)
                return ReadString();
            if (firstCharacter == _falseToken[0])
                return ReadConstantToken(_falseToken, JsonType.False);
            if (firstCharacter == _trueToken[0])
                return ReadConstantToken(_trueToken, JsonType.True);
            if (firstCharacter == _nullToken[0])
                return ReadConstantToken(_nullToken, JsonType.Null);

            throw new NotImplementedException();
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
            if (currentCharacter != _decimalPoint)
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
            if (_exponentialTokens.Contains(currentCharacter) == false)
                return false;

            _currentIndex++;
            if (IsEndOfToken())
                return false;

            // Check if possible + or - sign is present
            currentCharacter = _buffer[_currentIndex];
            if (currentCharacter == _negativeSign || currentCharacter == _positiveSign)
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
                    switch (currentCharacter)
                    {
                        // b, f, n, r, t, \, /, and " are valid characters after are character escape
                        case 'b':
                        case 'f':
                        case 'n':
                        case 'r':
                        case 't':
                        case '\\':
                        case '/':
                            isPreviousCharacterEscapeCharacter = false;
                            goto CheckEndOfBuffer;
                        case 'u':   // In case of u, four hexadecimal digits must follow after the \u (e.g. "\u002f" or "\u002F")
                        case 'U':
                            if (CheckFourHexadecimalDigitsOfJsonEscapeSequence() == false)
                            {
                                ReadUntilEndOfToken();
                                throw CreateDeserializationException(startIndex, JsonType.String);
                            }
                            // If everything is ok, then reset the boolean to false
                            isPreviousCharacterEscapeCharacter = false;
                            goto CheckEndOfBuffer;
                        default:
                            // If none of the above where found, then the string is not a valid Json string
                            ReadUntilEndOfToken();
                            throw CreateDeserializationException(startIndex, JsonType.String);
                    }
                }

                // If not, then treat this character as a normal one
                if (currentCharacter == _stringDelimiter)
                    return CreateBuffer(startIndex, JsonType.String);

                // Set the boolean value indicating that the next character is part of an escape sequence
                if (currentCharacter == _stringEscapeCharacter)
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
