using System;
using Light.GuardClauses;

namespace Light.Serialization.Json
{
    public struct JsonSerializationContext
    {
        public readonly object ObjectToBeSerialized;
        public readonly Type ActualType;
        public readonly Type ReferencedType;
        public readonly Action<object, Type, Type> SerializeChildObject;
        public readonly IJsonWriter Writer;


        public JsonSerializationContext(object objectToBeSerialized,
                                        Type actualType,
                                        Type referencedType,
                                        Action<object, Type, Type> serializeChildObject,
                                        IJsonWriter writer)
        {
            objectToBeSerialized.MustNotBeNull(nameof(objectToBeSerialized));
            actualType.MustNotBeNull(nameof(actualType));
            referencedType.MustNotBeNull(nameof(referencedType));
            serializeChildObject.MustNotBeNull(nameof(serializeChildObject));
            writer.MustNotBeNull(nameof(writer));
            
            ObjectToBeSerialized = objectToBeSerialized;
            ActualType = actualType;
            ReferencedType = referencedType;
            SerializeChildObject = serializeChildObject;
            Writer = writer;
        }

        public void SerializeValue<T>(T value)
        {
            SerializeChildObject(value, typeof (T), typeof (T));
        }
    }
}