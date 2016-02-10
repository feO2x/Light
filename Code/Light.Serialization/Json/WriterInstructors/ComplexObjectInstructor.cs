using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeDecomposition;
using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.WriterInstructors
{
    public sealed class ComplexObjectInstructor : IJsonWriterInstructor
    {
        private readonly IDictionary<Type, IList<IValueProvider>> _typeToValueProvidersMapping;
        private readonly IReadableValuesTypeAnalyzer _typeAnalyzer;

        public ComplexObjectInstructor(IReadableValuesTypeAnalyzer typeAnalyzer)
            : this(typeAnalyzer, new Dictionary<Type, IList<IValueProvider>>())
        {
            
        }

        public ComplexObjectInstructor(IReadableValuesTypeAnalyzer typeAnalyzer,
                                       IDictionary<Type, IList<IValueProvider>> typeToValueProvidersMapping)
        {
            typeAnalyzer.MustNotBeNull(nameof(typeAnalyzer));
            typeToValueProvidersMapping.MustNotBeNull(nameof(typeToValueProvidersMapping));

            _typeAnalyzer = typeAnalyzer;
            _typeToValueProvidersMapping = typeToValueProvidersMapping;
        }


        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return @object is Delegate == false;
        }

        public void Serialize(JsonSerializationContext serializationContext)
        {
            IList<IValueProvider> valueProviders;
            var actualType = serializationContext.ActualType;
            if (_typeToValueProvidersMapping.TryGetValue(actualType, out valueProviders) == false)
            {
                valueProviders = _typeAnalyzer.AnalyzeType(actualType);
                _typeToValueProvidersMapping.Add(actualType, valueProviders);
            }

            // TODO: what should happen when a complex object has no values to serialize?
            if (valueProviders.Count == 0)
                throw new NotImplementedException("What should happen if an object has no members to serialize? I would recommend to not serialize it by default");

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
