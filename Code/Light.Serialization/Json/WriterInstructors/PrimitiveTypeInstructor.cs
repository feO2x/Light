using System;
using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.Serialization.Json.WriterInstructors
{
    public sealed class PrimitiveTypeInstructor : IJsonWriterInstructor
    {
        public readonly IDictionary<Type, IPrimitiveTypeFormatter> PrimitiveTypeToFormattersMapping;

        public PrimitiveTypeInstructor(IDictionary<Type, IPrimitiveTypeFormatter> primitiveTypeToFormattersMapping)
        {
            primitiveTypeToFormattersMapping.MustNotBeNull(nameof(primitiveTypeToFormattersMapping));

            PrimitiveTypeToFormattersMapping = primitiveTypeToFormattersMapping;
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return PrimitiveTypeToFormattersMapping.ContainsKey(actualType);
        }

        public void Serialize(JsonSerializationContext serializationContext)
        {
            var typeFormatter = PrimitiveTypeToFormattersMapping[serializationContext.ActualType];
            var stringRepresentation = typeFormatter.FormatPrimitiveType(serializationContext.ObjectToBeSerialized);
            serializationContext.Writer.WritePrimitiveValue(stringRepresentation);
        }
    }
}