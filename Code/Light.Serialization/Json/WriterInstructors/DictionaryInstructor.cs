using System;
using System.Collections;
using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.Serialization.Json.WriterInstructors
{
    public sealed class DictionaryInstructor : IJsonWriterInstructor
    {
        public readonly IDictionary<Type, IPrimitiveTypeFormatter> PrimitiveTypeToFormattersMapping;

        public DictionaryInstructor(IDictionary<Type, IPrimitiveTypeFormatter> primitiveTypeToFormattersMapping)
        {
            primitiveTypeToFormattersMapping.MustNotBeNull(nameof(primitiveTypeToFormattersMapping));

            PrimitiveTypeToFormattersMapping = primitiveTypeToFormattersMapping;
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return @object is IDictionary;
        }

        public bool Serialize(JsonSerializationContext serializationContext)
        {
            var dictionary = (IDictionary) serializationContext.ObjectToBeSerialized;
            var decreaseIndentAfterSerialization = true;
            var writer = serializationContext.Writer;
            writer.BeginObject();

            if (dictionary.Count == 0)
            {
                writer.EndObject();
                return decreaseIndentAfterSerialization;
            }

            var dicitionaryEnumerator = dictionary.GetEnumerator();
            dicitionaryEnumerator.MoveNext();

            var indexDictionaryIteration = 0;
            while (true)
            {
                var key = dicitionaryEnumerator.Key;
                var firstDictionaryIteration = false;
                if (key == null)
                    writer.WriteKey(JsonSymbols.Null);
                else
                {
                    var keyType = key.GetType();
                    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                    if (PrimitiveTypeToFormattersMapping.ContainsKey(keyType))
                    {
                        var typeFormatter = PrimitiveTypeToFormattersMapping[keyType];
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
                    if (indexDictionaryIteration == 0)
                        firstDictionaryIteration = true;
                    serializationContext.SerializeChildObject(value, valueType, valueType, firstDictionaryIteration);
                }

                if (dicitionaryEnumerator.MoveNext())
                    writer.WriteDelimiter();
                else
                    break;

                indexDictionaryIteration++;
            }
            writer.EndObject();

            return decreaseIndentAfterSerialization;
        }
    }
}