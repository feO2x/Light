namespace Light.Core
{
    public static class CharExtensions
    {
        public static bool IsHexadecimal(this char character, bool onlyLowercaseLetters = false)
        {
            if (onlyLowercaseLetters == false)
                character = char.ToLowerInvariant(character);

            switch (character)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                    return true;
                default:
                    return false;
            }
        }
    }
}
