namespace Light.Serialization.Json.LowLevelReading
{
    public sealed class SingleBufferJsonReaderFactory : IJsonReaderFactory
    {
        public IJsonReader CreateFromString(string json)
        {
            return new SingleBufferJsonReader(json.ToCharArray());
        }
    }
}