namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public interface IObjectDeserializationReferencePreserver
    {
        void AddReference(int id, object @object);
        bool TryGetReference(int id, out object @object);
    }
}