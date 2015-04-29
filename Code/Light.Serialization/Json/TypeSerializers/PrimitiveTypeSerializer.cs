using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.TypeSerializers
{
    public sealed class PrimitiveTypeSerializer : ITypeSerializer
    {
        private readonly IJsonWriter _writer;
        private readonly IDictionary<Type, IPrimitiveTypeFormatter> _primitiveTypeToFormatterMapping;

        public PrimitiveTypeSerializer(IJsonWriter writer,
                                           IDictionary<Type, IPrimitiveTypeFormatter> primitiveTypeToFormatterMapping)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            if (primitiveTypeToFormatterMapping == null) throw new ArgumentNullException("primitiveTypeToFormatterMapping");

            _writer = writer;
            _primitiveTypeToFormatterMapping = primitiveTypeToFormatterMapping;
        }

        public int SequenceID { get; set; }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return _primitiveTypeToFormatterMapping.ContainsKey(actualType);
        }

        public void Serialize(object @object, Type actualType, Type referencedType, Action<object, Type, Type> serializeChildObject)
        {
            var stringRepresentation = _primitiveTypeToFormatterMapping[actualType].FormatPrimitiveType(@object);
            _writer.WriteRaw(stringRepresentation);
        }
    }
}
