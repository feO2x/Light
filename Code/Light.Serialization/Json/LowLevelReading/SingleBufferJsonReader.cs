using System;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.LowLevelReading
{
    public sealed class SingleBufferJsonReader : IJsonReader
    {
        private readonly char[] _buffer;
        private readonly JsonReaderSymbols _jsonReaderSymbols;
        private int _currentIndex;

        public SingleBufferJsonReader(char[] buffer, JsonReaderSymbols jsonReaderSymbols)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (jsonReaderSymbols == null) throw new ArgumentNullException(nameof(jsonReaderSymbols));

            _buffer = buffer;
            _jsonReaderSymbols = jsonReaderSymbols;
        }

        public JsonToken ReadNextToken()
        {
            var wasEndOfJsonDocumentReached = IgnoreWhiteSpace();

            if (wasEndOfJsonDocumentReached)
                return CreateToken(_currentIndex, JsonTokenType.EndOfDocument);

            var firstCharacter = _buffer[_currentIndex];

            if (char.IsDigit(firstCharacter))
                return ReadPositiveNumber(firstCharacter);
            if (firstCharacter == _jsonReaderSymbols.NegativeSign)
                return ReadNegativeNumber();
            if (firstCharacter == _jsonReaderSymbols.StringDelimiter)
                return ReadString();
            if (firstCharacter == _jsonReaderSymbols.False[0])
                return ReadConstantToken(_jsonReaderSymbols.False, JsonTokenType.False);
            if (firstCharacter == _jsonReaderSymbols.True[0])
                return ReadConstantToken(_jsonReaderSymbols.True, JsonTokenType.True);
            if (firstCharacter == _jsonReaderSymbols.Null[0])
                return ReadConstantToken(_jsonReaderSymbols.Null, JsonTokenType.Null);
            if (firstCharacter == _jsonReaderSymbols.BeginOfArray)
                return ReadSingleCharacterAndCreateToken(JsonTokenType.BeginOfArray);
            if (firstCharacter == _jsonReaderSymbols.EndOfArray)
                return ReadSingleCharacterAndCreateToken(JsonTokenType.EndOfArray);
            if (firstCharacter == _jsonReaderSymbols.BeginOfObject)
                return ReadSingleCharacterAndCreateToken(JsonTokenType.BeginOfObject);
            if (firstCharacter == _jsonReaderSymbols.EndOfObject)
                return ReadSingleCharacterAndCreateToken(JsonTokenType.EndOfObject);
            if (firstCharacter == _jsonReaderSymbols.PairDelimiter)
                return ReadSingleCharacterAndCreateToken(JsonTokenType.PairDelimiter);
            if (firstCharacter == _jsonReaderSymbols.ValueDelimiter)
                return ReadSingleCharacterAndCreateToken(JsonTokenType.ValueDelimiter);

            var startIndex = _currentIndex;
            ReadToEndOfToken();
            var token = CreateToken(startIndex, JsonTokenType.Error);
            throw new TokenNotSupportedException($"The Json Reader cannot recognize the sequence {token} and therefore cannot tranlate it to a valid JsonToken", token);
        }

        private bool IgnoreWhiteSpace()
        {
            while (true)
            {
                if (_currentIndex == _buffer.Length)
                    return true;

                if (char.IsWhiteSpace(_buffer[_currentIndex]) == false)
                    return false;

                _currentIndex++;
            }
        }

        private JsonToken ReadPositiveNumber(char firstCharacter)
        {
            var startIndex = _currentIndex;
            var tokenType = CheckNumber(firstCharacter);
            if (tokenType == JsonTokenType.Error)
                throw ReadToEndOfTokenAndCreateJsonDocumentException(startIndex, DefaultJsonSymbols.Number);

            return CreateToken(startIndex, tokenType);
        }

        private JsonToken ReadNegativeNumber()
        {
            // The first character is definitely a negative sign, otherwise this method would not have been called
            var startIndex = _currentIndex;

            // Advance the current index and check if it is a digit
            _currentIndex++;
            if (IsEndOfToken())
                throw CreateJsonDocumentException(startIndex, DefaultJsonSymbols.Number);

            var currentCharacter = _buffer[_currentIndex];
            // If there is no number, then the document is not formatted properly
            if (char.IsDigit(currentCharacter) == false)
                throw ReadToEndOfTokenAndCreateJsonDocumentException(startIndex, DefaultJsonSymbols.Number);

            var tokenType = CheckNumber(currentCharacter);
            if (tokenType == JsonTokenType.Error)
                throw ReadToEndOfTokenAndCreateJsonDocumentException(startIndex, DefaultJsonSymbols.Number);

            return CreateToken(startIndex, tokenType);
        }

        private JsonTokenType CheckNumber(char firstCharacter)
        {
            // Check if the first character is zero
            if (firstCharacter == '0')
            {
                // If the number ends after the zero, then return an integer type
                _currentIndex++;
                if (IsEndOfToken())
                    return JsonTokenType.IntegerNumber;

                // Else check if there's a decimal part
                var currentCharacter = _buffer[_currentIndex];
                return CheckDecimalPart(currentCharacter);
            }

            // Else the number starts with a digit other than zero
            while (true)
            {
                _currentIndex++;
                // If the number ends now, it's definitely an integer number
                if (IsEndOfToken())
                    return JsonTokenType.IntegerNumber;

                var currentCharacter = _buffer[_currentIndex];
                // If the current character is a digit, then continue the loop to check the next character
                if (char.IsDigit(currentCharacter))
                    continue;

                return CheckDecimalPart(currentCharacter);
            }
        }

        private JsonTokenType CheckDecimalPart(char currentCharacter)
        {
            // Check if there's a decimal point
            if (currentCharacter != _jsonReaderSymbols.DecimalPoint)
                return CheckExponentialPart(currentCharacter);

            // If yes then there must be at least one digit
            _currentIndex++;
            if (IsEndOfToken() || char.IsDigit(_buffer[_currentIndex]) == false)
                return JsonTokenType.Error;

            // Else read in as much digits as possible
            while (true)
            {
                _currentIndex++;

                // If the token ends now, then the number is a correct floating point number
                if (IsEndOfToken())
                    return JsonTokenType.FloatingPointNumber;

                currentCharacter = _buffer[_currentIndex];
                // If the current character is a digit, then continue this loop to check the next character
                if (char.IsDigit(currentCharacter))
                    continue;

                // Otherwise check the exponential part of the number
                return CheckExponentialPart(currentCharacter);
            }
        }

        private JsonTokenType CheckExponentialPart(char currentCharacter)
        {
            // The exponential part has to begin with an appropriate sign
            if (_jsonReaderSymbols.ExponentialSymbols.Contains(currentCharacter) == false)
                return JsonTokenType.Error;

            // If it is an appropriate exponential sign, check if the next character is a possible plus or minus sign
            _currentIndex++;
            if (IsEndOfToken())
                return JsonTokenType.Error;

            currentCharacter = _buffer[_currentIndex];
            if (currentCharacter == _jsonReaderSymbols.PositiveSign || currentCharacter == _jsonReaderSymbols.NegativeSign)
            {
                _currentIndex++;
                if (IsEndOfToken())
                    return JsonTokenType.Error;

                currentCharacter = _buffer[_currentIndex];
            }

            // There must be at least one digit after the exponential symbol (or sign symbol)
            if (char.IsDigit(currentCharacter) == false)
                return JsonTokenType.Error;

            // Read in as much digits as possible
            while (true)
            {
                _currentIndex++;

                if (IsEndOfToken())
                    return JsonTokenType.FloatingPointNumber;

                currentCharacter = _buffer[_currentIndex];
                if (char.IsDigit(currentCharacter))
                    continue;

                return JsonTokenType.Error;
            }
        }

        private JsonToken ReadString()
        {
            // The first digit is a string delimiter, otherwise this method would not have been called
            var startIndex = _currentIndex;

            // Read in all following characters until we get a valid string delimiter that ends the JSON string
            var isPreviousCharacterEscapeCharacter = false;
            while (true)
            {
                CheckNextCharacter:
                _currentIndex++;
                // A string must end with a string delimiter, if the buffer ends now, then the string is erroneous
                if (IsEndOfBuffer())
                    throw CreateJsonDocumentException(startIndex, DefaultJsonSymbols.String);

                var currentCharacter = _buffer[_currentIndex];

                // Check if the previous character was an escape character
                if (isPreviousCharacterEscapeCharacter)
                {
                    // If yes then check if the current character is a specially escaped character that only has one letter
                    foreach (var singleEscapedCharacter in _jsonReaderSymbols.SingleEscapedCharacters)
                    {
                        if (singleEscapedCharacter.ValueAfterEscapeCharacter != currentCharacter) continue;

                        isPreviousCharacterEscapeCharacter = false;
                        goto CheckNextCharacter;
                    }

                    // Otherwise check if this is an escape sequence of four hexadecimal digits
                    if (currentCharacter == _jsonReaderSymbols.HexadecimalEscapeIndicator)
                    {
                        if (CheckFourHexadecimalDigitsOfJsonEscapeSequence() == false)
                            throw ReadToEndOfStringTokenAndCreateJsonDocumentException(startIndex);

                        isPreviousCharacterEscapeCharacter = false;
                    }
                }

                // If not, then treat this character as a normal one
                else if (currentCharacter == _jsonReaderSymbols.StringDelimiter)
                    return ReadSingleCharacterAndCreateToken(startIndex, JsonTokenType.String);

                // Set the boolean value indicating that the next character is part of an escape sequence
                else if (currentCharacter == _jsonReaderSymbols.StringEscapeCharacter)
                    isPreviousCharacterEscapeCharacter = true;
            }
        }

        private bool CheckFourHexadecimalDigitsOfJsonEscapeSequence()
        {
            // There must be exactly 4 digits after the u
            for (var i = 0; i < 4; i++)
            {
                _currentIndex++;

                if (IsEndOfToken())
                    return false;

                if (_buffer[_currentIndex].IsHexadecimal() == false)
                    return false;
            }

            return true;
        }

        private bool IsEndOfToken()
        {
            if (_currentIndex == _buffer.Length)
                return true;
            var currentCharacter = _buffer[_currentIndex];
            return char.IsWhiteSpace(currentCharacter) ||
                   currentCharacter == _jsonReaderSymbols.ValueDelimiter ||
                   currentCharacter == _jsonReaderSymbols.EndOfArray ||
                   currentCharacter == _jsonReaderSymbols.EndOfObject ||
                   currentCharacter == _jsonReaderSymbols.PairDelimiter;
        }

        private bool IsEndOfBuffer()
        {
            return _currentIndex == _buffer.Length;
        }

        private JsonToken ReadConstantToken(string expectedToken, JsonTokenType tokenType)
        {
            var startIndex = _currentIndex;
            for (var i = 1; i < expectedToken.Length; i++)
            {
                _currentIndex++;
                if (_currentIndex == _buffer.Length)
                    throw CreateJsonDocumentException(startIndex, expectedToken);

                if (_buffer[_currentIndex] == expectedToken[i])
                    continue;

                throw ReadToEndOfTokenAndCreateJsonDocumentException(startIndex, expectedToken);
            }

            return ReadSingleCharacterAndCreateToken(startIndex, tokenType);
        }

        private void ReadToEndOfToken()
        {
            while (true)
            {
                if (IsEndOfToken())
                    return;
                _currentIndex++;
            }
        }

        private JsonToken ReadSingleCharacterAndCreateToken(JsonTokenType tokenType)
        {
            return CreateToken(_currentIndex++, tokenType);
        }

        private JsonToken ReadSingleCharacterAndCreateToken(int tokenStartIndex, JsonTokenType tokenType)
        {
            _currentIndex++;
            return CreateToken(tokenStartIndex, tokenType);
        }

        private JsonToken CreateToken(int startIndex, JsonTokenType tokenType)
        {
            return new JsonToken(_buffer, startIndex, _currentIndex - startIndex, tokenType);
        }

        private JsonDocumentException CreateJsonDocumentException(int tokenStartIndex, string expectedJsonType)
        {
            var token = CreateToken(tokenStartIndex, JsonTokenType.Error);
            return new JsonDocumentException($"Cannot deserialize value {token} to {expectedJsonType}.", token);
        }

        private JsonDocumentException ReadToEndOfTokenAndCreateJsonDocumentException(int tokenStartIndex, string expectedJsonType)
        {
            ReadToEndOfToken();
            return CreateJsonDocumentException(tokenStartIndex, expectedJsonType);
        }

        private JsonDocumentException ReadToEndOfStringTokenAndCreateJsonDocumentException(int tokenStartIndex)
        {
            while (true)
            {
                _currentIndex++;
                if (IsEndOfBuffer())
                    break;
                if (_buffer[_currentIndex] != _jsonReaderSymbols.StringDelimiter)
                    continue;

                _currentIndex++;
                break;
            }

            return CreateJsonDocumentException(tokenStartIndex, DefaultJsonSymbols.String);
        }
    }
}