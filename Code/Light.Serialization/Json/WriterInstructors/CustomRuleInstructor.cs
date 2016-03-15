using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.ObjectReferencePreservation;

namespace Light.Serialization.Json.WriterInstructors
{
    public sealed class CustomRuleInstructor : IDecoratableInstructor
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

        public void Serialize(JsonSerializationContext serializationContext)
        {
            ComplexObjectWriting.WriteValues(serializationContext, _valueProviders);
        }

        public void SerializeInner(JsonSerializationContext serializationContext)
        {
            if (_valueProviders.Count == 0) return;
            serializationContext.Writer.WriteDelimiter();
            ComplexObjectWriting.WriteInnerValues(serializationContext, _valueProviders);
        }
    }
}