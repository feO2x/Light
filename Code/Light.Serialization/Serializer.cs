using System;
using System.Collections.Generic;

namespace Light.Serialization
{
    public sealed class Serializer : ISerializer
    {
        private readonly IDocumentWriter _writer;
        private readonly IList<ITypeSerializer> _typeSerializers;
        private readonly Dictionary<Type, ITypeSerializer> _typeToSerializerMapping = new Dictionary<Type, ITypeSerializer>();

        public Serializer(IDocumentWriter writer,
                          IList<ITypeSerializer> typeSerializers)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            if (typeSerializers == null) throw new ArgumentNullException("typeSerializers");

            _writer = writer;
            _typeSerializers = typeSerializers;
        }

        public string Serialize<T>(T objectGraphRoot)
        {
            if (objectGraphRoot == null)
                throw new ArgumentNullException("objectGraphRoot");

            _writer.BeginDocument();
            SerializeObject(objectGraphRoot, objectGraphRoot.GetType());

            _writer.EndDocument();
            return _writer.Document;
        }

        private void SerializeObject(object @object, Type objectType)
        {
            ITypeSerializer targetTypeSerializer;
            if (_typeToSerializerMapping.TryGetValue(objectType, out targetTypeSerializer) == false)
            {
                targetTypeSerializer = FindTargetTypeSerializer(@object, objectType);
                if (targetTypeSerializer == null)
                    return;

                _typeToSerializerMapping.Add(objectType, targetTypeSerializer);
            }

            targetTypeSerializer.Serialize(@object, objectType, SerializeObject);
        }

        private ITypeSerializer FindTargetTypeSerializer(object @object, Type objectType)
        {
            foreach (var typeSerializer in _typeSerializers)
            {
                if (typeSerializer.AppliesToObject(@object, objectType))
                    return typeSerializer;
            }
            return null;
        }
    }
}
