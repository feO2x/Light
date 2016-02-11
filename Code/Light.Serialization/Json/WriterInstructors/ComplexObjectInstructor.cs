using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeDecomposition;
using System;

namespace Light.Serialization.Json.WriterInstructors
{
    public sealed class ComplexObjectInstructor : IJsonWriterInstructor
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
            var writer = serializationContext.Writer;
            
            writer.BeginObject();
            for (var i = 0; i < valueProviders.Count; i++)
            {
                var valueProvider = valueProviders[i];
                var childObject = valueProvider.GetValue(serializationContext.ObjectToBeSerialized);

                if (childObject == null)
                {
                    writer.WriteKey(valueProvider.Name);
                    writer.WriteNull();
                }
                else
                {
                    var childObjectType = childObject.GetType(); // TODO: This might end up in an endless loop if e.g. a property returns a reference to the object itself. Can be solved with a dictionary that contains all objects being serialized (what I wanted to integrate in the first place).
                    writer.WriteKey(valueProvider.Name);
                    serializationContext.SerializeChildObject(childObject, childObjectType, valueProvider.ReferenceType);
                }

                if (i < valueProviders.Count - 1)
                    writer.WriteDelimiter();
            }

            writer.EndObject();
        }
    }
}
