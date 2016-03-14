using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.LowLevelWriting
{
    public sealed class FirstCharacterToLowerAndRemoveAllSpecialCharactersNormalizer : IJsonKeyNormalizer
    {
        public string Normalize(string key)
        {
            return key.FirstCharacterToLowerAndRemoveAllSpecialCharacters();
        }
    }
}