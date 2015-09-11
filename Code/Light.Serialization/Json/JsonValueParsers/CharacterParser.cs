using System;
using System.Globalization;

namespace Light.Serialization.Json.JsonValueParsers
{
    public sealed class CharacterParser : IJsonValueParser
    {
        private readonly KnownJsonTokens _knownJsonTokens;
        private readonly Type _charType = typeof (char);

        public CharacterParser()
            : this(new KnownJsonTokens())
        {
        }

        public CharacterParser(KnownJsonTokens knownJsonTokens)
        {
            if (knownJsonTokens == null) throw new ArgumentNullException(nameof(knownJsonTokens));

            _knownJsonTokens = knownJsonTokens;
        }

        public bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer.JsonType == JsonType.String && requestedType == _charType;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var buffer = context.Buffer;
            // The buffer has at least two elements, thus accessing the second one is safe
            var currentCharacter = buffer[1];
            // Check if the JSON string is an empty string
            if (currentCharacter == _knownJsonTokens.StringDelimiter)
                throw CreateException(buffer);
            // If not then check if it is an escape sequence
            if (currentCharacter == _knownJsonTokens.StringEscapeCharacter)
                return ReadEscapeSequence(buffer);

            // If it's not an escape sequence, the buffer size must be three to be a single character (e.g. "a")
            if (buffer.Count != 3)
                throw CreateException(buffer);
            return currentCharacter;
        }

        private char ReadEscapeSequence(JsonCharacterBuffer buffer)
        {
            var currentCharacter = buffer[2];
            // Check if the current character indicates a hexadecimal escape sequence
            if (currentCharacter == _knownJsonTokens.HexadecimalEscapeIndicator)
                return ReadHexadimalEscapeSequence(buffer);

            // If it is not hexadecimal, then it must be a special escape sequence with only a single character
            if (buffer.Count != 4)
                throw CreateException(buffer);

            foreach (var singleEscapedCharacter in _knownJsonTokens.SingleEscapedCharacters)
            {
                if (singleEscapedCharacter.ValueAfterEscapeCharacter == currentCharacter)
                    return singleEscapedCharacter.EscapedCharacter;
            }
            // If no single escape character could be found, throw an exception because the escaped character cannot be read
            throw CreateException(buffer);
        }

        private static char ReadHexadimalEscapeSequence(JsonCharacterBuffer buffer)
        {
            if (buffer.Count != 8)
                throw CreateException(buffer);

            var hexadecimalDigitsAsString = buffer.ToString(3, 4);
            return Convert.ToChar(int.Parse(hexadecimalDigitsAsString, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo));
        }

        private static DeserializationException CreateException(JsonCharacterBuffer buffer)
        {
            return new DeserializationException($"Cannot deserialize value {buffer} to a character.");
        }
    }
}