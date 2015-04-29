using System;
using System.Collections;
using System.Collections.Generic;

namespace Light.Serialization.Json.TypeSerializers
{
    public sealed class DictionarySerializer : ITypeSerializer
    {
        private readonly IDictionary<Type, IPrimitiveTypeFormatter> _primitiveTypeToFormattersMapping;
        private readonly IJsonWriter _writer;

        public DictionarySerializer(IDictionary<Type, IPrimitiveTypeFormatter> primitiveTypeToFormattersMapping,
                                    IJsonWriter writer)
        {
            if (primitiveTypeToFormattersMapping == null) throw new ArgumentNullException("primitiveTypeToFormattersMapping");

            _primitiveTypeToFormattersMapping = primitiveTypeToFormattersMapping;
            _writer = writer;
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return @object is IDictionary;
        }

        public void Serialize(object @object, Type actualType, Type referencedType, Action<object, Type, Type> serializeChildObject)
        {
            var dictionary = (IDictionary) @object;

            if (dictionary.Count == 0)
                throw new NotImplementedException("What should happen if a dictionary is empty?");

            _writer.BeginComplexObject();

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
                    _writer.WriteKey(_primitiveTypeToFormattersMapping[keyType].FormatPrimitiveType(key));
                else
                    _writer.WriteKey(key.ToString());

                var value = dicitionaryEnumerator.Value;
                if (value == null)
                    _writer.WriteNull();
                else
                {
                    var valueType = value.GetType();
                    serializeChildObject(value, valueType, valueType);
                }

                if (dicitionaryEnumerator.MoveNext())
                    _writer.WriteDelimiter();
                else
                    break;
            }
            _writer.EndComplexObject();
        }
    }
}