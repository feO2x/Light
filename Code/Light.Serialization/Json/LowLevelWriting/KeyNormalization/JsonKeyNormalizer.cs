using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.LowLevelWriting.KeyNormalization
{
    public class JsonKeyNormalizer : IJsonKeyNormalizer
    {
        public bool ShouldNormalizeKey { get; } = true;

        public string Normalize(string key)
        {
            return key.FirstCharacterToLowerAndRemoveAllSpecialCharacters();
        }
    }
}