namespace Light.Serialization.Json
{
    public interface IJsonWriter
    {
        void BeginArray();
        void EndArray();
        void BeginObject();
        void EndObject();
        void WriteKey(string key);
        void WriteDelimiter();
        void WritePrimitiveValue(string @string);
        void WriteNull();
    }
}
