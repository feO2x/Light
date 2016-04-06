using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeDecomposition;
using System;

namespace Light.Serialization.Json.WriterInstructors
{
    public sealed class ComplexObjectInstructor : IJsonWriterInstructor
    {
        private IReadableValuesTypeAnalyzer _typeAnalyzer;

        public ComplexObjectInstructor(IReadableValuesTypeAnalyzer typeAnalyzer)
        {
            TypeAnalyzer = typeAnalyzer;
        }

        public IReadableValuesTypeAnalyzer TypeAnalyzer
        {
            get { return _typeAnalyzer; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _typeAnalyzer = value;
            }
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return @object is Delegate == false;
        }

        public bool Serialize(JsonSerializationContext serializationContext)
        {
            var decreaseIndentAfterSerialization = true;
            var valueProviders = _typeAnalyzer.AnalyzeType(serializationContext.ActualType);
            ComplexObjectWriting.WriteValues(serializationContext, valueProviders);
            return decreaseIndentAfterSerialization;
        }
    }
}
