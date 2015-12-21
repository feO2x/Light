using System;
using System.Collections.Generic;

namespace Light.Serialization.Json
{
    public sealed class JsonSerializer : ISerializer
    {
        private readonly IList<IJsonTypeSerializer> _typeSerializers;
        private readonly IJsonWriterFactory _writerFactory;
        private readonly Dictionary<Type, IJsonTypeSerializer> _typeToSerializerMapping = new Dictionary<Type, IJsonTypeSerializer>();
        private IJsonWriter _jsonWriter;

        public JsonSerializer(IList<IJsonTypeSerializer> typeSerializers,
                              IJsonWriterFactory writerFactory)
        {
            if (typeSerializers == null) throw new ArgumentNullException(nameof(typeSerializers));
            if (writerFactory == null) throw new ArgumentNullException(nameof(writerFactory));

            _typeSerializers = typeSerializers;
            _writerFactory = writerFactory;
        }

        public string Serialize<T>(T objectGraphRoot)
        {
            return Serialize(objectGraphRoot, typeof (T));
        }

        public string Serialize(object objectGraphRoot, Type requestedType)
        {
            if (objectGraphRoot == null)
                throw new ArgumentNullException(nameof(objectGraphRoot));
            if (requestedType == null)
                throw new ArgumentNullException(nameof(requestedType));

            _jsonWriter = _writerFactory.Create();
            SerializeObject(objectGraphRoot, objectGraphRoot.GetType(), requestedType);

            var json = _writerFactory.FinishWriteProcessAndReleaseResources();
            _jsonWriter = null;
            return json;
        }

        private void SerializeObject(object @object, Type actualType, Type referencedType)
        {
            IJsonTypeSerializer targetJsonTypeSerializer;
            if (_typeToSerializerMapping.TryGetValue(actualType, out targetJsonTypeSerializer) == false)
            {
                targetJsonTypeSerializer = FindTargetTypeSerializer(@object, actualType, referencedType);
                if (targetJsonTypeSerializer == null)
                    throw new SerializationException($"Type {actualType.FullName} cannot be serialized.");

                _typeToSerializerMapping.Add(actualType, targetJsonTypeSerializer);
            }

            targetJsonTypeSerializer.Serialize(new JsonSerializationContext(@object, actualType, referencedType, SerializeObject, _jsonWriter));
        }

        private IJsonTypeSerializer FindTargetTypeSerializer(object @object, Type objectType, Type referencedType)
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
