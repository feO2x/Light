using System;
using System.Collections.Generic;
using Light.Serialization.Json.ComplexTypeDecomposition;

namespace Light.Serialization.Json.WriterInstructors
{
    public sealed class ComplexWriterInstructor : IJsonWriterInstructor
    {
        private readonly IDictionary<Type, IList<IValueProvider>> _typeToValueProvidersMapping;
        private readonly IReadableValuesTypeAnalyzer _typeAnalyzer;

        public ComplexWriterInstructor(IReadableValuesTypeAnalyzer typeAnalyzer)
            : this(typeAnalyzer, new Dictionary<Type, IList<IValueProvider>>())
        {
            
        }

        public ComplexWriterInstructor(IReadableValuesTypeAnalyzer typeAnalyzer,
                                         IDictionary<Type, IList<IValueProvider>> typeToValueProvidersMapping)
        {
            if (typeAnalyzer == null) throw new ArgumentNullException(nameof(typeAnalyzer));
            if (typeToValueProvidersMapping == null) throw new ArgumentNullException(nameof(typeToValueProvidersMapping));

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
                    var childObjectType = childObject.GetType();
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
