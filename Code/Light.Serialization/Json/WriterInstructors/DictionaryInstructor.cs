using System;
using System.Collections;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.ObjectReferencePreservation;

namespace Light.Serialization.Json.WriterInstructors
{
    public sealed class DictionaryInstructor : IDecoratableInstructor
    {
        private readonly IDictionary<Type, IPrimitiveTypeFormatter> _primitiveTypeToFormattersMapping;

        public DictionaryInstructor(IDictionary<Type, IPrimitiveTypeFormatter> primitiveTypeToFormattersMapping)
        {
            primitiveTypeToFormattersMapping.MustNotBeNull(nameof(primitiveTypeToFormattersMapping));

            _primitiveTypeToFormattersMapping = primitiveTypeToFormattersMapping;
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return @object is IDictionary;
        }

        public void Serialize(JsonSerializationContext serializationContext)
        {
            var writer = serializationContext.Writer;
            writer.BeginObject();
            SerializeInner(serializationContext);
            writer.EndObject();
        }

        public void SerializeInner(JsonSerializationContext serializationContext)
        {
            var dictionary = (IDictionary)serializationContext.ObjectToBeSerialized;
            var writer = serializationContext.Writer;

            if (dictionary.Count == 0)
            {
                writer.EndObject();
                return;
            }

            serializationContext.Writer.WriteDelimiter();

            var dicitionaryEnumerator = dictionary.GetEnumerator();
            dicitionaryEnumerator.MoveNext();
            while (true)
            {
                var key = dicitionaryEnumerator.Key;
                if (key == null)
                    writer.WriteKey(JsonSymbols.Null);
                else
                {
                    var keyType = key.GetType();
                    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                    if (_primitiveTypeToFormattersMapping.ContainsKey(keyType))
                    {
                        var typeFormatter = _primitiveTypeToFormattersMapping[keyType];
                        writer.WriteKey(typeFormatter.FormatPrimitiveType(key), typeFormatter.ShouldBeNormalizedKey);
                    }
                    else
                        writer.WriteKey(key.ToString(), false);
                }

                var value = dicitionaryEnumerator.Value;
                if (value == null)
                    writer.WriteNull();
                else
                {
                    var valueType = value.GetType();
                    serializationContext.SerializeChildObject(value, valueType, valueType);
                }

                if (dicitionaryEnumerator.MoveNext())
                    writer.WriteDelimiter();
                else
                    break;
            }
        }
    }
}