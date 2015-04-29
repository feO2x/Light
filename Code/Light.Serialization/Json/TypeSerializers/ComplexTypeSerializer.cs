using Light.Serialization.Json.ComplexTypeDecomposition;
using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.TypeSerializers
{
    public sealed class ComplexTypeSerializer : ITypeSerializer
    {
        private readonly IDictionary<Type, IList<IValueProvider>> _typeToValueProvidersMapping;
        private readonly IReadableValuesTypeAnalyzer _typeAnalyzer;
        private readonly IJsonWriter _writer;

        public ComplexTypeSerializer(IDictionary<Type, IList<IValueProvider>> typeToValueProvidersMapping,
                                         IReadableValuesTypeAnalyzer typeAnalyzer,
                                         IJsonWriter writer)
        {
            if (typeToValueProvidersMapping == null) throw new ArgumentNullException("typeToValueProvidersMapping");
            if (typeAnalyzer == null) throw new ArgumentNullException("typeAnalyzer");
            if (writer == null) throw new ArgumentNullException("writer");

            _typeToValueProvidersMapping = typeToValueProvidersMapping;
            _typeAnalyzer = typeAnalyzer;
            _writer = writer;
        }


        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return @object is Delegate == false;
        }

        public void Serialize(object @object, Type actualType, Type referencedType, Action<object, Type, Type> serializeChildObject)
        {
            IList<IValueProvider> valueProviders;
            if (_typeToValueProvidersMapping.TryGetValue(actualType, out valueProviders) == false)
            {
                valueProviders = _typeAnalyzer.AnalyzeType(actualType);
                _typeToValueProvidersMapping.Add(actualType, valueProviders);
            }

            // TODO: what should happen when a complex object has no values to serialize?
            if (valueProviders.Count == 0)
                throw new NotImplementedException("What should happen if an object has no members to serialize? I would recommend to not serialize it by default");

            _writer.BeginComplexObject();

            for (var i = 0; i < valueProviders.Count; i++)
            {
                var valueProvider = valueProviders[i];
                var childObject = valueProvider.GetValue(@object);

                if (childObject == null)
                {
                    _writer.WriteKey(valueProvider.Name);
                    _writer.WriteNull();
                }
                else
                {
                    var childObjectType = childObject.GetType();
                    _writer.WriteKey(valueProvider.Name);
                    serializeChildObject(childObject, childObjectType, valueProvider.ReferencedType);
                }

                if (i < valueProviders.Count - 1)
                    _writer.WriteDelimiter();
            }

            _writer.EndComplexObject();
        }
    }
}
