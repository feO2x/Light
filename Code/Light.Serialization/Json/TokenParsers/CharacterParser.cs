﻿using System;
using System.Globalization;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class CharacterParser : IJsonTokenParser
    {
        private readonly Type _charType = typeof (char);

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.String && requestedType == _charType;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var token = context.Token;
            // The token has at least two elements because it is a JSON string, thus accessing the second one is safe
            var currentCharacter = token[1];
            // Check if the JSON string is an empty string
            if (currentCharacter == JsonSymbols.StringDelimiter)
                throw CreateException(token);
            // If not then check if it is an escape sequence
            if (currentCharacter == JsonSymbols.StringEscapeCharacter)
                return ReadEscapeSequence(token);

            // If it's not an escape sequence, the token size must be three to be a single character (e.g. "a")
            if (token.Length != 3)
                throw CreateException(token);
            return currentCharacter;
        }

        private static char ReadEscapeSequence(JsonToken token)
        {
            var currentCharacter = token[2];
            // Check if the current character indicates a hexadecimal escape sequence
            if (currentCharacter == JsonSymbols.HexadecimalEscapeIndicator)
                return ReadHexadimalEscapeSequence(token);

            // If it is not hexadecimal, then it must be a special escape sequence with only a single character
            if (token.Length != 4)
                throw CreateException(token);

            foreach (var singleEscapedCharacter in JsonSymbols.SingleEscapedCharacters)
            {
                if (singleEscapedCharacter.ValueAfterEscapeCharacter == currentCharacter)
                    return singleEscapedCharacter.EscapedCharacter;
            }
            // If no single escape character could be found, throw an exception because the escaped character cannot be read
            throw CreateException(token);
        }

        private static char ReadHexadimalEscapeSequence(JsonToken token)
        {
            if (token.Length != 8)
                throw CreateException(token);

            var hexadecimalDigitsAsString = token.ToString(3, 4);
            return Convert.ToChar(int.Parse(hexadecimalDigitsAsString, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo));
        }

        private static DeserializationException CreateException(JsonToken token)
        {
            return new DeserializationException($"Cannot deserialize value {token} to a character.");
        }
    }
}