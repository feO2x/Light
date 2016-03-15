namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public interface IDecoratableComplexInstructor : IJsonWriterInstructor
    {
        void SerializeInner(JsonSerializationContext serializationContext);
    }
}