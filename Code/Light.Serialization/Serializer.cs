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
            SerializeObject(objectGraphRoot, objectGraphRoot.GetType(), typeof(T));

            _writer.EndDocument();
            return _writer.Document;
        }

        private void SerializeObject(object @object, Type actualType, Type referencedType)
        {
            ITypeSerializer targetTypeSerializer;
            if (_typeToSerializerMapping.TryGetValue(actualType, out targetTypeSerializer) == false)
            {
                targetTypeSerializer = FindTargetTypeSerializer(@object, actualType, referencedType);
                if (targetTypeSerializer == null)
                    return;

                _typeToSerializerMapping.Add(actualType, targetTypeSerializer);
            }

            targetTypeSerializer.Serialize(@object, actualType, referencedType, SerializeObject);
        }

        private ITypeSerializer FindTargetTypeSerializer(object @object, Type objectType, Type referencedType)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var typeSerializer in _typeSerializers)
            {
                if (typeSerializer.AppliesToObject(@object, objectType, referencedType))
                    return typeSerializer;
            }
            return null;
        }
    }
}
