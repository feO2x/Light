
namespace Light.Serialization.FrameworkExtensions
{
    public static class StringExtensions
    {
        public static string SurroundWith(this string @string, string value)
        {
            if (value == string.Empty)
                return @string;
            var charBuffer = new char[@string.Length + (2 * value.Length)];
            for (var i = 0; i < value.Length; i++)
            {
                var characterToBeCopied = value[i];
                charBuffer[i] = characterToBeCopied;
                charBuffer[i + @string.Length + value.Length] = characterToBeCopied;
            }

            for (var i = 0; i < @string.Length; i++)
            {
                charBuffer[i + value.Length] = @string[i];
            }
            return new string(charBuffer);
        }

        public static string SurroundWith(this string @string, char character)
        {
            var characterBuffer = new char[@string.Length + 2];
            characterBuffer[0] = characterBuffer[characterBuffer.Length - 1] = character;
            for (var i = 0; i < @string.Length; i++)
            {
                characterBuffer[i + 1] = @string[i];
            }
            return new string(characterBuffer);
        }

        public static string SurroundWithQuotationMarks(this string @string)
        {
            return @string.SurroundWith('"');
        }

        public static string SurroundWithParantheses(this string @string)
        {
            var length = @string.Length;
            return @string.PadLeft(++length, '(').PadRight(++length, ')');
        }

        public static bool IsSurroundedByQuotationMarks(this string @string)
        {
            if (@string.Length <= 1)
                return false;
            return @string[0] == '"' && @string[@string.Length - 1] == '"';
        }

        public static string MakeFirstCharacterLowercase(this string @string)
        {
            if (char.IsLower(@string[0]))
                return @string;

            return char.ToLowerInvariant(@string[0]) + @string.Substring(1);
        }

        public static string FirstCharacterToLowerAndRemoveAllSpecialCharacters(this string @string)
        {
            int i;
            char character;
            for (i = 0; i < @string.Length; i++)
            {
                character = @string[i];
                if (char.IsLetterOrDigit(character) == false)
                    goto NormalizeString;
            }

            return @string.MakeFirstCharacterLowercase();

            // This section is only used when a new string has to be created because the old one contains special or uppercase characters
            // Otherwise, the passed in string is returned (to minimize the creation of object - your GC will thank you).
            NormalizeString:
            var numberOfSpecialCharacters = 0;

            for (; i < @string.Length; i++)
            {
                if (char.IsLetterOrDigit(@string[i]) == false)
                    numberOfSpecialCharacters++;
            }

            if (numberOfSpecialCharacters == @string.Length)
                throw new DeserializationException($"The specified name {@string} contains only special characters that cannot be normalized.");

            var charArray = new char[@string.Length - numberOfSpecialCharacters];
            int charArrayIndex = 0;

            for (i = 0; i < @string.Length; i++)
            {
                character = @string[i];
                if (char.IsLetterOrDigit(character) == false)
                    continue;

                charArray[charArrayIndex] = character;
                charArrayIndex++;
            }

            if (char.IsLower(charArray[0]))
                return new string(charArray);

            charArray[0] = char.ToLowerInvariant(charArray[0]);

            return new string(charArray);
        }

        public static string ToLowerAndRemoveAllSpecialCharacters(this string @string)
        {
            int i;
            char character;
            for (i = 0; i < @string.Length; i++)
            {
                character = @string[i];
                if (char.IsLetterOrDigit(character) == false || char.IsLower(character) == false)
                    goto NormalizeString;
            }

            return @string;

            // This section is only used when a new string has to be created because the old one contains special or uppercase characters
            // Otherwise, the passed in string is returned (to minimize the creation of object - your GC will thank you).
            NormalizeString:
            var numberOfSpecialCharacters = 0;

            for (; i < @string.Length; i++)
            {
                if (char.IsLetterOrDigit(@string[i]) == false)
                    numberOfSpecialCharacters++;
            }

            if (numberOfSpecialCharacters == @string.Length)
                throw new DeserializationException($"The specified name {@string} contains only special characters that cannot be normalized.");

            var charArray = new char[@string.Length - numberOfSpecialCharacters];

            for (i = 0; i < @string.Length; i++)
            {
                character = @string[i];
                if (char.IsLetterOrDigit(character) == false)
                    continue;

                charArray[i] = char.ToLower(character);
            }

            return new string(charArray);
        }
    }
}
