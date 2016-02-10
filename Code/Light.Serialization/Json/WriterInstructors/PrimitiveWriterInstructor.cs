using System;
using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.Serialization.Json.WriterInstructors
{
    public sealed class PrimitiveWriterInstructor : IJsonWriterInstructor
    {
        private readonly IDictionary<Type, IPrimitiveTypeFormatter> _primitiveTypeToFormattersMapping;

        public PrimitiveWriterInstructor(IDictionary<Type, IPrimitiveTypeFormatter> primitiveTypeToFormattersMapping)
        {
            primitiveTypeToFormattersMapping.MustNotBeNull(nameof(primitiveTypeToFormattersMapping));

            _primitiveTypeToFormattersMapping = primitiveTypeToFormattersMapping;
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return _primitiveTypeToFormattersMapping.ContainsKey(actualType);
        }

        public void Serialize(JsonSerializationContext serializationContext)
        {
            var stringRepresentation = _primitiveTypeToFormattersMapping[serializationContext.ActualType].FormatPrimitiveType(serializationContext.ObjectToBeSerialized);
            serializationContext.Writer.WritePrimitiveValue(stringRepresentation);
        }
    }
}