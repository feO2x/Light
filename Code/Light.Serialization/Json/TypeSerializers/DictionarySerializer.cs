using System;
using System.Collections;
using System.Collections.Generic;

namespace Light.Serialization.Json.TypeSerializers
{
    public sealed class DictionarySerializer : IJsonTypeSerializer
    {
        private readonly IDictionary<Type, IPrimitiveTypeFormatter> _primitiveTypeToFormattersMapping;

        public DictionarySerializer(IDictionary<Type, IPrimitiveTypeFormatter> primitiveTypeToFormattersMapping)
        {
            if (primitiveTypeToFormattersMapping == null) throw new ArgumentNullException("primitiveTypeToFormattersMapping");

            _primitiveTypeToFormattersMapping = primitiveTypeToFormattersMapping;
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return @object is IDictionary;
        }

        public void Serialize(JsonSerializationContext serializationContext)
        {
            var dictionary = (IDictionary) serializationContext.ObjectToBeSerialized;

            if (dictionary.Count == 0)
                throw new NotImplementedException("What should happen if a dictionary is empty?");

            var writer = serializationContext.Writer;
            writer.BeginComplexObject();

            var dicitionaryEnumerator = dictionary.GetEnumerator();
            dicitionaryEnumerator.MoveNext();

            while (true)
            {
                var key = dicitionaryEnumerator.Key;
                if (key == null)
                    throw new NotImplementedException("What should happen if a key is null?");

                var keyType = key.GetType();
                // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                if (_primitiveTypeToFormattersMapping.ContainsKey(keyType))
                    writer.WriteKey(_primitiveTypeToFormattersMapping[keyType].FormatPrimitiveType(key));
                else
                    writer.WriteKey(key.ToString());

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
            writer.EndComplexObject();
        }
    }
}