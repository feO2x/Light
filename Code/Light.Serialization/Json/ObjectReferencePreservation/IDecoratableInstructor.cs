namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public interface IDecoratableInstructor : IJsonWriterInstructor
    {
        void SerializeInner(JsonSerializationContext serializationContext);
    }
}