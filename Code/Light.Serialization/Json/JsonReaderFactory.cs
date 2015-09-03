namespace Light.Serialization.Json
{
    public sealed class JsonReaderFactory : IJsonReaderFactory
    {
        public IJsonReader CreateFromString(string json)
        {
            return new SingleBufferJsonReader(json.ToCharArray());
        }
    }
}