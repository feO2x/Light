using System;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.TypeSerializers
{
    public sealed class EnumerationToStringSerializer : IJsonTypeSerializer
    {
        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return actualType.IsEnum;
        }

        public void Serialize(JsonSerializationContext serializationContext)
        {
            serializationContext.Writer.WriteRaw(serializationContext.ObjectToBeSerialized.ToString().SurroundWithQuotationMarks());
        }
    }
}
