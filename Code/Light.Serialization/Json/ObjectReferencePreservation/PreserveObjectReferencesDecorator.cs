using System;
using Light.GuardClauses;

namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public sealed class PreserveObjectReferencesDecorator : IJsonWriterInstructor
    {
        private readonly IDecoratableInstructor _decoratedInstructor;
        private readonly ObjectReferencePreserver _objectReferencePreserver;

        private string _idSymbol = JsonSymbols.DefaultIdSymbol;
        private string _referenceSymbol = JsonSymbols.DefaultReferenceSymbol;

        public PreserveObjectReferencesDecorator(IDecoratableInstructor decoratedInstructor, ObjectReferencePreserver objectReferencePreserver)
        {
            decoratedInstructor.MustNotBeNull(nameof(decoratedInstructor));
            objectReferencePreserver.MustNotBeNull(nameof(objectReferencePreserver));

            _decoratedInstructor = decoratedInstructor;
            _objectReferencePreserver = objectReferencePreserver;
        }


        public string IdSymbol
        {
            get { return _idSymbol; }
            set
            {
                value.MustNotBeNullOrWhiteSpace(nameof(value));
                _idSymbol = value;
            }
        }

        public string ReferenceSymbol
        {
            get { return _referenceSymbol; }
            set
            {
                value.MustNotBeNullOrWhiteSpace(nameof(value));
                _referenceSymbol = value;
            }
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return _decoratedInstructor.AppliesToObject(@object, actualType, referencedType);
        }

        public void Serialize(JsonSerializationContext serializationContext)
        {
            var objectReferenceInfo = _objectReferencePreserver.GetObjectReferenceInfo(serializationContext.ObjectToBeSerialized);

            var writer = serializationContext.Writer;
            writer.BeginObject();

            if (objectReferenceInfo.WasAlreadySerialized)
            {
                writer.WriteKey(_referenceSymbol, false);
                serializationContext.SerializeValue(objectReferenceInfo.JsonObjectId);
                writer.EndObject();
                return;
            }

            writer.WriteKey(_idSymbol, false);
            serializationContext.SerializeValue(objectReferenceInfo.JsonObjectId);
            _decoratedInstructor.SerializeInner(serializationContext);
            writer.EndObject();
        }
    }
}