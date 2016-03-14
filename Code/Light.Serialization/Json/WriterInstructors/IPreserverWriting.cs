namespace Light.Serialization.Json.WriterInstructors
{
    public interface IPreserverWriting
    {
        void WriteReferenceKey(JsonSerializationContext context, string id);
        void WriteIdentifierKey(JsonSerializationContext context, string id);
    }
}