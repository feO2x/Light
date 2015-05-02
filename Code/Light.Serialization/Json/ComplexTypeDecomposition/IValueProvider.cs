using System;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public interface IValueProvider
    {
        string Name { get; }

        Type ReferenceType { get; }

        object GetValue(object @object);
    }
}