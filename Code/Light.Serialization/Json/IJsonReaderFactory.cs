namespace Light.Serialization.Json
{
    public interface IJsonReaderFactory
    {
        IJsonReader CreateFromString(string json);
    }
}