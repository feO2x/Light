using System;
using System.Globalization;

namespace Light.Serialization.Json.JsonValueParsers
{
    public sealed class StringParser : IJsonValueParser
    {
        private readonly KnownJsonTokens _knownJsonTokens;
        private readonly Type _stringType = typeof (string);

        public StringParser()
            : this(new KnownJsonTokens())
        {
            
        }

        public StringParser(KnownJsonTokens knownJsonTokens)
        {
            if (knownJsonTokens == null) throw new ArgumentNullException(nameof(knownJsonTokens));

            _knownJsonTokens = knownJsonTokens;
        }

        public bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer.JsonType == JsonType.String && requestedType == _stringType;
        }

        public object ParseValue(JsonCharacterBuffer buffer, Type requestedType)
        {
            // If the buffer has only two character, then it is an empty string.
            if (buffer.Count == 2)
                return string.Empty;

            // The first and the last character in the buffer are the JSON string delimiters (").
            // Thus the following loop runs from 1 to Count - 2
            for (var i = 1; i < buffer.Count - 1; i++)
            {
                // Check if the buffer contains characters that need to be escaped
                var currentCharacter = buffer[i];
                if (currentCharacter == _knownJsonTokens.StringEscapeCharacter)
                    return ConvertEscapeSequencesInBuffer(buffer, i + 1);
            }
            // If none could be found, then return the inner characters without the surrounding quotation marks
            return buffer.ToString(1, buffer.Count - 2);
        }

        private string ConvertEscapeSequencesInBuffer(JsonCharacterBuffer buffer, int currentBufferIndex)
        {
            // Calculate how many characters we need for the new char array
            // This means that we have to run one through the whole buffer
            // and check how many single escape sequences and hexadecimal
            // escape sequences can be found
            var numberOfSingleEscapeSequences = 0;
            var numberOfHexadecimalEscapeSequences = 0;
            var isPreviousCharacterTheStringEscapeCharacter = true;
            char currentCharacter;

            while (currentBufferIndex < buffer.Count - 1)
            {
                currentCharacter = buffer[currentBufferIndex++];
                // Check if the current character is part of an escape sequence
                if (isPreviousCharacterTheStringEscapeCharacter)
                {
                    // If it is a hexadecimal character then set the index after the escape sequence
                    if (currentCharacter == _knownJsonTokens.HexadecimalEscapeIndicator)
                    {
                        currentBufferIndex += 4;
                        numberOfHexadecimalEscapeSequences++;
                    }
                    // else it can only be a single character escape sequence
                    else
                    {
                        numberOfSingleEscapeSequences++;
                    }
                    isPreviousCharacterTheStringEscapeCharacter = false;
                    continue;
                }
                // Check if this character is the beginning of an escape sequence
                if (currentCharacter == _knownJsonTokens.StringEscapeCharacter)
                    isPreviousCharacterTheStringEscapeCharacter = true;
            }

            // Calculate the actual number of characters that we need for the string
            var numberOfCharacters = buffer.Count - 2 - numberOfSingleEscapeSequences - (numberOfHexadecimalEscapeSequences * 5);
            var characterArray = new char[numberOfCharacters];
            currentBufferIndex = 1;  // Start copying from the first character after the initial JSON string delimiter

            // Fill the character array, escape where necessary
            for (var i = 0; i < numberOfCharacters; i++)
            {
                currentCharacter = buffer[currentBufferIndex++];
                if (currentCharacter == _knownJsonTokens.StringEscapeCharacter)
                {
                    characterArray[i] = ReadEscapeSequence(buffer, ref currentBufferIndex);
                    continue;
                }
                characterArray[i] = currentCharacter;
            }

            return new string(characterArray);
        }

        private char ReadEscapeSequence(JsonCharacterBuffer buffer, ref int currentBufferIndex)
        {
            var currentCharacter = buffer[currentBufferIndex++];
            // Check if the second character in the escape sequence indicates a hexadecimal escape sequence
            if (currentCharacter == _knownJsonTokens.HexadecimalEscapeIndicator)
                return ReadHexadecimalEscapeSequence(buffer, ref currentBufferIndex);

            // If not, then the escape sequence must be one with a single character
            foreach (var singleEscapedCharacter in _knownJsonTokens.SingleEscapedCharacters)
            {
                if (currentCharacter == singleEscapedCharacter.ValueAfterEscapeCharacter)
                    return singleEscapedCharacter.EscapedCharacter;
            }

            throw new DeserializationException("This exception should never be thrown because the foreach loop above will find exactly one single escape character that fits. However, if you see this exception message nontheless, then please check if you accidently altered KnownJsonTokens.SingleEscapedCharacters.");
        }

        private char ReadHexadecimalEscapeSequence(JsonCharacterBuffer buffer, ref int currentBufferIndex)
        {
            var hexadecimalDigitsAsString = buffer.ToString(currentBufferIndex, 4);
            currentBufferIndex += 4;  // Increase buffer index to point to the first character after the hexadecimal escape sequence
            return Convert.ToChar(int.Parse(hexadecimalDigitsAsString, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo));
        }
    }
}
