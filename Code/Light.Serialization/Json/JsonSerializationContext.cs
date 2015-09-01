using System;

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
            if (objectToBeSerialized == null) throw new ArgumentNullException(nameof(objectToBeSerialized));
            if (actualType == null) throw new ArgumentNullException(nameof(actualType));
            if (referencedType == null) throw new ArgumentNullException(nameof(referencedType));
            if (serializeChildObject == null) throw new ArgumentNullException(nameof(serializeChildObject));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            ObjectToBeSerialized = objectToBeSerialized;
            ActualType = actualType;
            ReferencedType = referencedType;
            SerializeChildObject = serializeChildObject;
            Writer = writer;
        }
    }
}