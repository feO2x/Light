
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
    }
}
