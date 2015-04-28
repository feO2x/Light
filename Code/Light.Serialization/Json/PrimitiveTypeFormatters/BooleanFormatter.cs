using System;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class BooleanFormatter : IPrimitiveTypeFormatter
    {
        private readonly Type _targetType = typeof (bool);

        public Type TargetType
        {
            get { return _targetType; }
        }

        public string FormatPrimitiveType(object @object)
        {
            return @object.ToString().ToLower();
        }
    }
}