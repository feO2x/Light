using System;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public interface IValueProvider
    {
        string Name { get; }

        Type ReferencedType { get; }

        object GetValue(object @object);
    }
}