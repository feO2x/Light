using System;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public abstract class BasePrimitiveTypeFormatter<T>
    {
        public Type TargetType { get; } = typeof (T);
    }
}