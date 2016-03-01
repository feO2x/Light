﻿using Light.GuardClauses;
using Light.Serialization.Json.LowLevelReading;
using System;
using System.Globalization;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class StringParser : IJsonTokenParser
    {
        private readonly JsonReaderSymbols _jsonReaderSymbols;
        private readonly Type _stringType = typeof (string);

        public StringParser(JsonReaderSymbols jsonReaderSymbols)
        {
            jsonReaderSymbols.MustNotBeNull(nameof(jsonReaderSymbols));

            _jsonReaderSymbols = jsonReaderSymbols;
        }

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.String && requestedType == _stringType;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            return ParseValue(context.Token);
        }

        public string ParseValue(JsonToken token)
        {
            // If the token has only two character, then it is an empty string.
            if (token.Length == 2)
                return string.Empty;

            // The first and the last character in the token are the JSON string delimiters (").
            // Thus the following loop runs from 1 to Length - 2
            for (var i = 1; i < token.Length - 1; i++)
            {
                // Check if the token contains characters that need to be escaped
                var currentCharacter = token[i];
                if (currentCharacter == _jsonReaderSymbols.StringEscapeCharacter)
                    return ConvertEscapeSequencesInToken(token, i + 1);
            }
            // If none could be found, then return the inner characters without the surrounding quotation marks
            return token.ToStringWithoutQuotationMarks();
        }

        private string ConvertEscapeSequencesInToken(JsonToken token, int currentTokenIndex)
        {
            // Calculate how many characters we need for the new char array
            // This means that we have to run through the whole token
            // and check how many single escape sequences and hexadecimal
            // escape sequences can be found
            var numberOfSingleEscapeSequences = 0;
            var numberOfHexadecimalEscapeSequences = 0;
            var isPreviousCharacterTheStringEscapeCharacter = true;
            char currentCharacter;

            while (currentTokenIndex < token.Length - 1)
            {
                currentCharacter = token[currentTokenIndex++];
                // Check if the current character is part of an escape sequence
                if (isPreviousCharacterTheStringEscapeCharacter)
                {
                    // If it is a hexadecimal character then set the index after the escape sequence
                    if (currentCharacter == _jsonReaderSymbols.HexadecimalEscapeIndicator)
                    {
                        currentTokenIndex += 4;
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
                if (currentCharacter == _jsonReaderSymbols.StringEscapeCharacter)
                    isPreviousCharacterTheStringEscapeCharacter = true;
            }

            // Calculate the actual number of characters that we need for the string
            var numberOfCharacters = token.Length - 2 - numberOfSingleEscapeSequences - (numberOfHexadecimalEscapeSequences * 5);
            var characterArray = new char[numberOfCharacters];
            currentTokenIndex = 1;  // Start copying from the first character after the initial JSON string delimiter

            // Fill the character array, escape where necessary
            for (var i = 0; i < numberOfCharacters; i++)
            {
                currentCharacter = token[currentTokenIndex++];
                if (currentCharacter == _jsonReaderSymbols.StringEscapeCharacter)
                {
                    characterArray[i] = ReadEscapeSequence(token, ref currentTokenIndex);
                    continue;
                }
                characterArray[i] = currentCharacter;
            }

            return new string(characterArray);
        }

        private char ReadEscapeSequence(JsonToken buffer, ref int currentBufferIndex)
        {
            var currentCharacter = buffer[currentBufferIndex++];
            // Check if the second character in the escape sequence indicates a hexadecimal escape sequence
            if (currentCharacter == _jsonReaderSymbols.HexadecimalEscapeIndicator)
                return ReadHexadecimalEscapeSequence(buffer, ref currentBufferIndex);

            // If not, then the escape sequence must be one with a single character
            foreach (var singleEscapedCharacter in _jsonReaderSymbols.SingleEscapedCharacters)
            {
                if (currentCharacter == singleEscapedCharacter.ValueAfterEscapeCharacter)
                    return singleEscapedCharacter.EscapedCharacter;
            }

            throw new DeserializationException("This exception should never be thrown because the foreach loop above will find exactly one single escape character that fits. However, if you see this exception message nontheless, then please check if you accidently altered JsonReaderSymbols.SingleEscapedCharacters.");
        }

        private static char ReadHexadecimalEscapeSequence(JsonToken buffer, ref int currentBufferIndex)
        {
            var hexadecimalDigitsAsString = buffer.ToString(currentBufferIndex, 4);
            currentBufferIndex += 4;  // Increase token index to point to the first character after the hexadecimal escape sequence
            return Convert.ToChar(int.Parse(hexadecimalDigitsAsString, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo));
        }
    }
}
