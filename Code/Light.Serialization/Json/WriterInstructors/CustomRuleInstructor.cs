using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeDecomposition;

namespace Light.Serialization.Json.WriterInstructors
{
    public sealed class CustomRuleInstructor : IJsonWriterInstructor
    {
        private readonly Type _targetType;
        private readonly IList<IValueProvider> _valueProviders;

        public CustomRuleInstructor(Type targetType, IList<IValueProvider> valueProviders)
        {
            valueProviders.MustNotBeNull(nameof(valueProviders));
            targetType.MustNotBeNull(nameof(targetType));

            _targetType = targetType;
            _valueProviders = valueProviders;
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return actualType == _targetType;
        }

        public bool Serialize(JsonSerializationContext serializationContext)
        {
            bool decreaseIndentAfterSerialzation = false;
            ComplexObjectWriting.WriteValues(serializationContext, _valueProviders);
            return decreaseIndentAfterSerialzation;
        }
    }
}