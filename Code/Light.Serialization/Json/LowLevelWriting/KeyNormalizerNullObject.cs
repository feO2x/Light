namespace Light.Serialization.Json.LowLevelWriting
{
    public sealed class KeyNormalizerNullObject : IJsonKeyNormalizer
    {
        public string Normalize(string key)
        {
            return key;
        }
    }
}