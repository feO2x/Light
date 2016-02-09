using Light.Serialization.FrameworkExtensions;
using System;
using System.Reflection;

namespace Light.Serialization.Json.WriterInstructors
{
    public sealed class EnumerationToStringInstructor : IJsonWriterInstructor
    {
        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return actualType.GetTypeInfo().IsEnum;
        }

        public void Serialize(JsonSerializationContext serializationContext)
        {
            serializationContext.Writer.WritePrimitiveValue(serializationContext.ObjectToBeSerialized.ToString().SurroundWithQuotationMarks());
        }
    }
}
