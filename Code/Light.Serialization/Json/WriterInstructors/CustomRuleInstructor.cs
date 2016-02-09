using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeDecomposition;

namespace Light.Serialization.Json.WriterInstructors
{
    public class CustomRuleInstructor : IJsonWriterInstructor
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
            var writer = serializationContext.Writer;
            if(_valueProviders.Count == 0) { 
                writer.BeginObject();
                writer.EndObject();
                return;
            }

            writer.BeginObject();


            //TODO: same as in complexObjectInstrutor - 
            for (var i = 0; i < _valueProviders.Count; i++)
            {
                var valueProvider = _valueProviders[i];
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

                if (i < _valueProviders.Count - 1)
                    writer.WriteDelimiter();
            }

            writer.EndObject();
        }
    }
}