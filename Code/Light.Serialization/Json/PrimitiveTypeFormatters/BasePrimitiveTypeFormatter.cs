using System;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public abstract class BasePrimitiveTypeFormatter<T>
    {
        // ReSharper disable once InconsistentNaming
        private readonly Type _targetType = typeof (T);

        public Type TargetType
        {
            get { return _targetType; }
        }
    }
}