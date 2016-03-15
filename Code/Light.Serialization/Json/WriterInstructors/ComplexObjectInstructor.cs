using System;
using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.ObjectReferencePreservation;

namespace Light.Serialization.Json.WriterInstructors
{
    public sealed class ComplexObjectInstructor : IDecoratableComplexInstructor
    {
        private readonly IReadableValuesTypeAnalyzer _typeAnalyzer;

        public ComplexObjectInstructor(IReadableValuesTypeAnalyzer typeAnalyzer)
        {
            typeAnalyzer.MustNotBeNull(nameof(typeAnalyzer));

            _typeAnalyzer = typeAnalyzer;
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return @object is Delegate == false;
        }

        public void Serialize(JsonSerializationContext serializationContext)
        {
            var valueProviders = _typeAnalyzer.AnalyzeType(serializationContext.ActualType);
            ComplexObjectWriting.WriteValues(serializationContext, valueProviders);
        }

        public void SerializeInner(JsonSerializationContext serializationContext)
        {
            var valueProviders = _typeAnalyzer.AnalyzeType(serializationContext.ActualType);
            if (valueProviders.Count == 0) return;
            serializationContext.Writer.WriteDelimiter();
            ComplexObjectWriting.WriteInnerValues(serializationContext, valueProviders);
        }
    }
}