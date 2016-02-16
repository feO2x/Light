namespace Light.Serialization.Json
{
    public interface IJsonWriter
    {
        void BeginArray();
        void EndArray();
        void BeginObject();
        void EndObject();
        void WriteKey(string key, bool shouldNormalizeKey = true);
        void WriteDelimiter();
        void WritePrimitiveValue(string @string);
        void WriteNull();
    }
}
