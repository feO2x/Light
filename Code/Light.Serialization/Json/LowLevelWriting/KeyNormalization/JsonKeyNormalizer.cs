using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.LowLevelWriting.KeyNormalization
{
    public class JsonKeyNormalizer : IJsonKeyNormalizer
    {
        public string Normalize(string key)
        {
            return key.FirstCharacterToLowerAndRemoveAllSpecialCharacters();
        }
    }
}