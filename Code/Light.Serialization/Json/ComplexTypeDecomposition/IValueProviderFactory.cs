using System;
using System.Reflection;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public interface IValueProviderFactory
    {
        IValueProvider Create(Type targetType, PropertyInfo propertyInfo);

        IValueProvider Create(Type targetType, FieldInfo fieldInfo);
    }
}
