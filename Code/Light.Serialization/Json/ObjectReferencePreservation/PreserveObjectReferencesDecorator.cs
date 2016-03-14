using System;
using Light.GuardClauses;

namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public sealed class PreserveObjectReferencesDecorator : IJsonWriterInstructor
    {
        private readonly IJsonWriterInstructor _decoratedInstructor;

        public PreserveObjectReferencesDecorator(IJsonWriterInstructor decoratedInstructor)
        {
            decoratedInstructor.MustNotBeNull(nameof(decoratedInstructor));

            _decoratedInstructor = decoratedInstructor;
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return _decoratedInstructor.AppliesToObject(@object, actualType, referencedType);
        }

        public void Serialize(JsonSerializationContext serializationContext)
        {
            // Check if the object to be serialized has been serialized in the document before

            // if not, forward call to actual instructor
            _decoratedInstructor.Serialize(serializationContext);
        }
    }
}