using Light.GuardClauses;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class ToLowerWithoutSpecialCharactersNormalizer : IInjectableValueNameNormalizer
    {
        public string Normalize(string name)
        {
            name.MustNotBeNull(nameof(name));
            int i;
            char character;
            for (i = 0; i < name.Length; i++)
            {
                character = name[i];
                if (char.IsLetterOrDigit(character) == false || char.IsLower(character) == false)
                    goto NormalizeString;
            }

            return name;

            // This section is only used when a new string has to be created because the old one contains special or uppercase characters
            // Otherwise, the passed in string is returned (to minimize the creation of object - your GC will thank you).
            NormalizeString:
            var numberOfSpecialCharacters = 0;

            for (; i < name.Length; i++)
            {
                if (char.IsLetterOrDigit(name[i]) == false)
                    numberOfSpecialCharacters++;
            }

            if (numberOfSpecialCharacters == name.Length)
                throw new DeserializationException($"The specified name {name} contains only special characters that cannot be normalized.");

            var charArray = new char[name.Length - numberOfSpecialCharacters];

            for (i = 0; i < name.Length; i++)
            {
                character = name[i];
                if(char.IsLetterOrDigit(character) == false)
                    continue;

                charArray[i] = char.ToLower(character);
            }

            return new string(charArray);
        }
    }
}