using System;

namespace Light.Serialization.Json.TypeSerializers
{
    public sealed class EnumerationSerializer : ITypeSerializer
    {
        private readonly IJsonWriter _writer;

        public EnumerationSerializer(IJsonWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");

            _writer = writer;
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return actualType.IsEnum;
        }

        public void Serialize(object @object, Type actualType, Type referencedType, Action<object, Type, Type> serializeChildObject)
        {
            _writer.WriteRaw(@object.ToString());
        }
    }
}
