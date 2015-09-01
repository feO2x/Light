namespace Light.Serialization.Json
{
    public interface IJsonWriter
    {
        void BeginCollection();
        void EndCollection();
        void BeginComplexObject();
        void EndComplexObject();
        void WriteKey(string key);
        void WriteDelimiter();
        void WriteRaw(string @string);
        void WriteNull();
    }
}
