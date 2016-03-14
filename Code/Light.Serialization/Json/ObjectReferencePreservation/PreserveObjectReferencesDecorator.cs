using System;
using Light.GuardClauses;
using Light.Serialization.Json.WriterInstructors;

namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public sealed class PreserveObjectReferencesDecorator : IJsonWriterInstructor
    {
        private readonly IJsonWriterInstructor _decoratedInstructor;
        private readonly ObjectReferencePreserver _objectReferencePreserver;
        private readonly IPreserverWriting _preserverWriting;

        public PreserveObjectReferencesDecorator(IJsonWriterInstructor decoratedInstructor, ObjectReferencePreserver objectReferencePreserver, IPreserverWriting preserverWriting)
        {
            decoratedInstructor.MustNotBeNull(nameof(decoratedInstructor));
            objectReferencePreserver.MustNotBeNull(nameof(objectReferencePreserver));
            preserverWriting.MustNotBeNull(nameof(preserverWriting));

            _decoratedInstructor = decoratedInstructor;
            _objectReferencePreserver = objectReferencePreserver;
            _preserverWriting = preserverWriting;
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return _decoratedInstructor.AppliesToObject(@object, actualType, referencedType);
        }

        public void Serialize(JsonSerializationContext serializationContext)
        {
            var objectReferenceInfo = _objectReferencePreserver.GetObjectReferenceInfo(serializationContext.ObjectToBeSerialized);

            if (objectReferenceInfo.WasAlreadySerialized)
            {
                _preserverWriting.WriteReferenceKey(serializationContext, objectReferenceInfo.JsonObjectId.ToString());
                return;
            }

            _preserverWriting.WriteIdentifierKey(serializationContext, objectReferenceInfo.JsonObjectId.ToString());
            _decoratedInstructor.Serialize(serializationContext);
        }
    }
}