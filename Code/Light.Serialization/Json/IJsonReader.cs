namespace Light.Serialization.Json
{
    public interface IJsonReader
    {
        JsonToken ReadNextToken();
    }
}