using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.TypeSerializers
{
    public sealed class PrimitiveJsonTypeSerializer : IJsonTypeSerializer
    {
        private readonly IDictionary<Type, IPrimitiveTypeFormatter> _primitiveTypeToFormattersMapping;

        public PrimitiveJsonTypeSerializer(IDictionary<Type, IPrimitiveTypeFormatter> primitiveTypeToFormattersMapping)
        {
            if (primitiveTypeToFormattersMapping == null) throw new ArgumentNullException(nameof(primitiveTypeToFormattersMapping));

            _primitiveTypeToFormattersMapping = primitiveTypeToFormattersMapping;
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return _primitiveTypeToFormattersMapping.ContainsKey(actualType);
        }

        public void Serialize(JsonSerializationContext serializationContext)
        {
            var stringRepresentation = _primitiveTypeToFormattersMapping[serializationContext.ActualType].FormatPrimitiveType(serializationContext.ObjectToBeSerialized);
            serializationContext.Writer.WriteRaw(stringRepresentation);
        }
    }
}