using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class DefaultCharacterEscaper : ICharacterEscaper
    {
        private readonly IList<char> _escapedCharacters;
        private const char X000F = '\x000f';

        public DefaultCharacterEscaper()
        {
            var escapedCharacters = new List<char> { '"', '\\' };

            // Unicode C0 block
            for (var i = 0; i < ' '; i++)
            {
                escapedCharacters.Add((char)i);
            }

            // Unicode C1 block
            for (int i = '\u007F'; i <= '\u009f'; i++)
            {
                escapedCharacters.Add((char)i);
            }

            // Unicode Line Separator and Paragraph Seperator
            escapedCharacters.Add('\u2028');
            escapedCharacters.Add('\u2029');

            _escapedCharacters = escapedCharacters;
        }

        public DefaultCharacterEscaper(IList<char> escapedCharacters)
        {
            if (escapedCharacters == null) throw new ArgumentNullException("escapedCharacters");

            _escapedCharacters = escapedCharacters;
        }

        public char[] Escape(char character)
        {
            if (_escapedCharacters.Contains(character) == false)
                return null;

            // The following characters are escaped in a special way
            switch (character)
            {
                case '\t':
                    return new[] { '\\', 't' };
                case '\n':
                    return new[] { '\\', 'n' };
                case '\r':
                    return new[] { '\\', 'r' };
                case '\f':
                    return new[] { '\\', 'f' };
                case '\b':
                    return new[] { '\\', 'b' };
                case '\\':
                    return new[] { '\\', '\\' };
                case '"':
                    return new[] { '\\', '"' };
            }

            // All other characters will be represented in the "\uxxxx" format
            var buffer = new[]
                         {
                             '\\',
                             'u',
                             ConvertIntToChar((character >> 12) & X000F),
                             ConvertIntToChar((character >> 8) & X000F),
                             ConvertIntToChar((character >> 4) & X000F),
                             ConvertIntToChar(character & X000F)
                         };
            return buffer;

        }

        private static char ConvertIntToChar(int value)
        {
            if (value <= 9)
                return (char)(value + 48);

            return (char)((value - 10) + 97);
        }
    }
}