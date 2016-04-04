using System;
using System.Collections;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.WriterInstructors;

namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public sealed class PreserveObjectReferencesDecorator : IJsonWriterInstructor
    {
        private readonly IDecoratableInstructor _decoratedInstructor;
        private readonly ObjectSerializationReferencePreserver _objectSerializationReferencePreserver;

        private string _idSymbol = JsonSymbols.DefaultIdSymbol;
        private string _referenceSymbol = JsonSymbols.DefaultReferenceSymbol;

        public PreserveObjectReferencesDecorator(IDecoratableInstructor decoratedInstructor, ObjectSerializationReferencePreserver objectSerializationReferencePreserver)
        {
            decoratedInstructor.MustNotBeNull(nameof(decoratedInstructor));
            objectSerializationReferencePreserver.MustNotBeNull(nameof(objectSerializationReferencePreserver));

            _decoratedInstructor = decoratedInstructor;
            _objectSerializationReferencePreserver = objectSerializationReferencePreserver;
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
            var objectReferenceInfo = _objectSerializationReferencePreserver.GetObjectReferenceInfo(serializationContext.ObjectToBeSerialized);

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
            if (_decoratedInstructor is DictionaryInstructor && ((IDictionary)serializationContext.ObjectToBeSerialized).Count > 0)
                writer.WriteDelimiter();
            _decoratedInstructor.SerializeInner(serializationContext);
            writer.EndObject();
        }
    }
}