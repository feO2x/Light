using System;

namespace Light.Serialization.Json
{
    public interface IJsonTypeSerializer
    {
        bool AppliesToObject(object @object, Type actualType, Type referencedType);

        void Serialize(JsonSerializationContext serializationContext);
    }
}