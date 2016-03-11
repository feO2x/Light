using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.LowLevelWriting.KeyNormalization
{
    public class JsonKeyNormalizer : IJsonKeyNormalizer
    {
        public bool ForceNormalizeKey { get; } = false;
        public bool ForceNotToNormalizeKey { get; } = false;

        public string Normalize(string key)
        {
            return key.FirstCharacterToLowerAndRemoveAllSpecialCharacters();
        }
    }
}