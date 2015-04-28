using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.TypeSerializers
{
    public sealed class JsonPrimitiveTypeSerializer : ITypeSerializer
    {
        private readonly IJsonWriter _writer;
        private readonly IDictionary<Type, IPrimitiveTypeFormatter> _primitiveTypeToFormatterMapping;

        public JsonPrimitiveTypeSerializer(IJsonWriter writer,
                                           IDictionary<Type, IPrimitiveTypeFormatter> primitiveTypeToFormatterMapping)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            if (primitiveTypeToFormatterMapping == null) throw new ArgumentNullException("primitiveTypeToFormatterMapping");

            _writer = writer;
            _primitiveTypeToFormatterMapping = primitiveTypeToFormatterMapping;
        }

        public int SequenceID { get; set; }

        public bool AppliesToObject(object @object, Type objectType)
        {
            return _primitiveTypeToFormatterMapping.ContainsKey(objectType);
        }

        public void Serialize(object @object, Type objectType, Action<object, Type> serializeChildObject)
        {
            var stringRepresentation = _primitiveTypeToFormatterMapping[objectType].FormatPrimitiveType(@object);
            _writer.WriteRaw(stringRepresentation);
        }
    }
}
