using System;

namespace Light.Serialization
{
    public interface ITypeSerializer
    {
        bool AppliesToObject(object @object, Type actualType, Type referencedType);

        void Serialize(object @object,
                       Type actualType,
                       Type referencedType,
                       Action<object, Type, Type> serializeChildObject);
    }
}