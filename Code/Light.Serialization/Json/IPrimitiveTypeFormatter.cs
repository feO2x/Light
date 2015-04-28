using System;

namespace Light.Serialization.Json
{
    public interface IPrimitiveTypeFormatter
    {
        Type TargetType { get; }
        string FormatPrimitiveType(object @object);
    }
}
