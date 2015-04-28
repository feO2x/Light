using System;

namespace Light.Serialization
{
    public interface ITypeSerializer
    {
        bool AppliesToObject(object @object, Type objectType);
        void Serialize(object @object, Type objectType, Action<object, Type> serializeChildObject);
    }
}
